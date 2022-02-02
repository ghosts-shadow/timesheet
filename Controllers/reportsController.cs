using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    public class reportsController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        public List<towemp> transfercom(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom,
            long?[] proto)
        {
            var change = 0;
            var tranreplist = new List<towemp>();
            if (!search.IsNullOrWhiteSpace())
            {
                if (long.TryParse(search, out var empid))
                {
                    tranreplist = db.towemps.Where(x => x.LabourMaster.EMPNO == empid).ToList();
                    change++;
                }
            }

            if (datefrom.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x => x.effectivedate >= datefrom);
                }
                else if (change == 0)
                {
                    tranreplist = db.towemps.Where(x => x.effectivedate >= datefrom).ToList();
                }
            }

            if (dateto.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x => x.effectivedate <= dateto);
                }
                else if (change == 0)
                {
                    tranreplist = db.towemps.Where(x => x.effectivedate <= dateto).ToList();
                }
            }

            if (profrom != null && profrom.Count() > 0)
            {
                if (tranreplist.Count != 0)
                {
                    var templist = new List<towemp>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(tranreplist.FindAll(x => x.towref.mp_from == l));
                    }

                    tranreplist = templist;
                }
                else if (change == 0)
                {
                    var templist = new List<towemp>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(db.towemps.ToList().FindAll(x => x.towref.mp_from == l));
                    }

                    tranreplist = templist;
                }
            }

            if (proto != null && proto.Count() > 0)
            {
                if (tranreplist.Count != 0)
                {
                    var templist = new List<towemp>();
                    ;
                    foreach (var l in proto)
                    {
                        templist.AddRange(tranreplist.FindAll(x => x.towref.mp_to == l));
                    }

                    tranreplist = templist;
                }
                else if (change == 0)
                {
                    var templist = new List<towemp>();
                    foreach (var l in proto)
                    {
                        templist.AddRange(db.towemps.ToList().FindAll(x => x.towref.mp_to == l));
                    }

                    tranreplist = templist;
                }
            }

            return tranreplist;
        }

        // GET: reports
        public ActionResult transferreport(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom,
            long?[] proto)
        {
            var protolist = new List<ProjectList>();
            var profromlist = new List<ProjectList>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") ||
                                    this.User.IsInRole("logistics_officer") || this.User.IsInRole("Admin_View")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                protolist = new List<ProjectList>();
                foreach (var i in scid) protolist.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                protolist = this.db.ProjectLists.ToList();
            }

            
            profromlist = this.db.ProjectLists.ToList(); ;
            ViewBag.profromlist = new SelectList(profromlist, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            ViewBag.protolist = new SelectList(protolist, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            var change = 0;
            var tranreplist = transfercom(search, datefrom, dateto, profrom, proto);

            ViewBag.search = search;
            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            ViewBag.profrom = profrom;
            ViewBag.proto = proto;
            return View(tranreplist);
        }

        public void transferreportexel(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom,
            long?[] proto)
        {
            var change = 0;
            var tranreplist = transfercom(search, datefrom, dateto, profrom, proto);
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("transferred_workers");
            var mansuplierlist = db.ManPowerSuppliers.ToList();
            Sheet.Cells["A1"].Value = "EMPNO";
            Sheet.Cells["B1"].Value = "employee name";
            Sheet.Cells["C1"].Value = "Supplier";
            Sheet.Cells["D1"].Value = "Position";
            Sheet.Cells["E1"].Value = "effectivedate";
            Sheet.Cells["F1"].Value = "from";
            Sheet.Cells["G1"].Value = "to";
            var i = 2;
            foreach (var tr in tranreplist)
            {
                Sheet.Cells[string.Format("A{0}", i)].Value = tr.LabourMaster.EMPNO;
                Sheet.Cells[string.Format("B{0}", i)].Value = tr.LabourMaster.Person_Name;
                Sheet.Cells[string.Format("C{0}", i)].Value = tr.LabourMaster.ManPowerSupplier.Supplier;
                Sheet.Cells[string.Format("D{0}", i)].Value = tr.LabourMaster.Position;
                Sheet.Cells[string.Format("E{0}", i)].Value = tr.effectivedate.Value.ToString("M");
                Sheet.Cells[string.Format("F{0}", i)].Value = tr.towref.ProjectList1.PROJECT_NAME;
                Sheet.Cells[string.Format("G{0}", i)].Value = tr.towref.ProjectList1.PROJECT_NAME;
                i++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =transfer.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }

        public List<overtimeemployeelist> overtimecom(string search, DateTime? datefrom, DateTime? dateto,
            long?[] profrom)
        {
            var change = 0;
            var tranreplist = new List<overtimeemployeelist>();
            if (!search.IsNullOrWhiteSpace())
            {
                if (long.TryParse(search, out var empid))
                {
                    tranreplist = db.overtimeemployeelists.Where(x => x.LabourMaster.EMPNO == empid).ToList();
                    change++;
                }
            }

            if (datefrom.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x => x.effectivedate >= datefrom);
                }
                else if (change == 0)
                {
                    tranreplist = db.overtimeemployeelists.Where(x => x.effectivedate >= datefrom).ToList();
                }
            }

            if (dateto.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x => x.effectivedate <= dateto);
                }
                else if (change == 0)
                {
                    tranreplist = db.overtimeemployeelists.Where(x => x.effectivedate <= dateto).ToList();
                }
            }

            if (profrom != null && profrom.Count() > 0)
            {
                if (tranreplist.Count != 0)
                {
                    var templist = new List<overtimeemployeelist>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(tranreplist.FindAll(x => x.overtimeref.overtimepro == l));
                    }

                    tranreplist = templist;
                }
                else if (change == 0)
                {
                    var templist = new List<overtimeemployeelist>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(
                            db.overtimeemployeelists.ToList().FindAll(x => x.overtimeref.overtimepro == l));
                    }

                    tranreplist = templist;
                }
            }

            return tranreplist;
        }

        public ActionResult overtimereport(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom)
        {
            var protolist = new List<ProjectList>();
            var profromlist = new List<ProjectList>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") ||
                                    this.User.IsInRole("logistics_officer") || this.User.IsInRole("Admin_View")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                protolist = new List<ProjectList>();
                foreach (var i in scid) protolist.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                protolist = this.db.ProjectLists.ToList();
            }

            profromlist = protolist;
            ViewBag.profromlist = new SelectList(profromlist, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            var tranreplist = overtimecom(search, datefrom, dateto, profrom);

            ViewBag.search = search;
            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            ViewBag.profrom = profrom;
            return View(tranreplist);
        }

        public void overtimereportexel(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom)
        {
            var change = 0;
            var tranreplist = overtimecom(search, datefrom, dateto, profrom);
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("overtimereport");
            var mansuplierlist = db.ManPowerSuppliers.ToList();
            Sheet.Cells["A1"].Value = "EMPNO";
            Sheet.Cells["B1"].Value = "employee name";
            Sheet.Cells["C1"].Value = "Supplier";
            Sheet.Cells["D1"].Value = "Position";
            Sheet.Cells["E1"].Value = "effectivedate";
            Sheet.Cells["F1"].Value = "project";
            var i = 2;
            foreach (var tr in tranreplist)
            {
                Sheet.Cells[string.Format("A{0}", i)].Value = tr.LabourMaster.EMPNO;
                Sheet.Cells[string.Format("B{0}", i)].Value = tr.LabourMaster.Person_Name;
                Sheet.Cells[string.Format("C{0}", i)].Value = tr.LabourMaster.ManPowerSupplier.Supplier;
                Sheet.Cells[string.Format("D{0}", i)].Value = tr.LabourMaster.Position;
                Sheet.Cells[string.Format("E{0}", i)].Value = tr.effectivedate.Value.ToString("M");
                Sheet.Cells[string.Format("F{0}", i)].Value = tr.overtimeref.ProjectList.PROJECT_NAME;
                i++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =overtime.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }

        public List<Attendance> timesheetcom(string search, DateTime? datefrom, DateTime? dateto,
            long?[] profrom)
        {
            var change = 0;
            var tranreplist = new List<Attendance>();
            if (!search.IsNullOrWhiteSpace())
            {
                if (long.TryParse(search, out var empid))
                {
                    tranreplist = db.Attendances.Where(x => x.LabourMaster.EMPNO == empid).ToList();
                    change++;
                }
            }

            if (datefrom.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x =>
                        x.MainTimeSheet.TMonth.Year >= datefrom.Value.Year &&
                        x.MainTimeSheet.TMonth.Month >= datefrom.Value.Month);
                }
                else if (change == 0)
                {
                    tranreplist = db.Attendances.Where(x =>
                        x.MainTimeSheet.TMonth.Year >= datefrom.Value.Year &&
                        x.MainTimeSheet.TMonth.Month >= datefrom.Value.Month).ToList();
                }
            }

            if (dateto.HasValue)
            {
                if (tranreplist.Count != 0)
                {
                    tranreplist = tranreplist.FindAll(x =>
                        x.MainTimeSheet.TMonth.Year <= dateto.Value.Year &&
                        x.MainTimeSheet.TMonth.Month <= dateto.Value.Month);
                }
                else if (change == 0)
                {
                    tranreplist = db.Attendances.Where(x =>
                        x.MainTimeSheet.TMonth.Year <= dateto.Value.Year &&
                        x.MainTimeSheet.TMonth.Month <= dateto.Value.Month).ToList();
                }
            }

            if (profrom != null && profrom.Count() > 0)
            {
                if (tranreplist.Count != 0)
                {
                    var templist = new List<Attendance>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(tranreplist.FindAll(x => x.MainTimeSheet.Project == l));
                    }

                    tranreplist = templist;
                }
                else if (change == 0)
                {
                    var templist = new List<Attendance>();
                    foreach (var l in profrom)
                    {
                        templist.AddRange(db.Attendances.ToList().FindAll(x => x.MainTimeSheet.Project == l));
                    }

                    tranreplist = templist;
                }
            }

            var timereport = new List<Attendance>();
            foreach (var at in tranreplist.OrderByDescending(x=>x.MainTimeSheet.TMonth).ThenBy(x=>x.LabourMaster.EMPNO))
            {
                if (!timereport.Exists(x=>x.EmpID == at.EmpID && x.MainTimeSheet.TMonth.Month == at.MainTimeSheet.TMonth.Month && x.MainTimeSheet.TMonth.Year == at.MainTimeSheet.TMonth.Year))
                {
                    timereport.Add(at);
                }
            }
            return timereport;
        }

        public ActionResult timesheetreport(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom)
        {
            var protolist = new List<ProjectList>();
            var profromlist = new List<ProjectList>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") ||
                                    this.User.IsInRole("logistics_officer") || this.User.IsInRole("Admin_View")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                protolist = new List<ProjectList>();
                foreach (var i in scid) protolist.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                protolist = this.db.ProjectLists.ToList();
            }

            profromlist = protolist;
            ViewBag.profromlist = new SelectList(profromlist, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            var tranreplist = timesheetcom(search, datefrom, dateto, profrom);

            ViewBag.search = search;
            ViewBag.datefrom = datefrom;
            ViewBag.dateto = dateto;
            ViewBag.profrom = profrom;
            return View(tranreplist);
        }

        public void timesheetreportexel(string search, DateTime? datefrom, DateTime? dateto, long?[] profrom)
        {
            var change = 0;
            var tranreplist = timesheetcom(search, datefrom, dateto, profrom);
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("overtimereport");
            var mansuplierlist = db.ManPowerSuppliers.ToList();
            Sheet.Cells["A1"].Value = "Emp ID";
            Sheet.Cells["B1"].Value = "Emp Name";
            Sheet.Cells["C1"].Value = "Position";
            Sheet.Cells["D1"].Value = "Supplier";
            Sheet.Cells["E1"].Value = "Month";
            Sheet.Cells["F1"].Value = "Project";
            Sheet.Cells["G1"].Value = "1";
            Sheet.Cells["H1"].Value = "2";
            Sheet.Cells["I1"].Value = "3";
            Sheet.Cells["J1"].Value = "4";
            Sheet.Cells["K1"].Value = "5";
            Sheet.Cells["L1"].Value = "6";
            Sheet.Cells["M1"].Value = "7";
            Sheet.Cells["N1"].Value = "8";
            Sheet.Cells["O1"].Value = "9";
            Sheet.Cells["P1"].Value = "10";
            Sheet.Cells["Q1"].Value = "11";
            Sheet.Cells["R1"].Value = "12";
            Sheet.Cells["S1"].Value = "13";
            Sheet.Cells["T1"].Value = "14";
            Sheet.Cells["U1"].Value = "15";
            Sheet.Cells["V1"].Value = "16";
            Sheet.Cells["W1"].Value = "17";
            Sheet.Cells["X1"].Value = "18";
            Sheet.Cells["Y1"].Value = "19";
            Sheet.Cells["Z1"].Value = "20";
            Sheet.Cells["AA1"].Value = "21";
            Sheet.Cells["AB1"].Value = "22";
            Sheet.Cells["AC1"].Value = "23";
            Sheet.Cells["AD1"].Value = "24";
            Sheet.Cells["AE1"].Value = "25";
            Sheet.Cells["AF1"].Value = "26";
            Sheet.Cells["AG1"].Value = "27";
            Sheet.Cells["AH1"].Value = "28";
            Sheet.Cells["AI1"].Value = "29";
            Sheet.Cells["AJ1"].Value = "30";
            Sheet.Cells["AK1"].Value = "31";
            Sheet.Cells["AL1"].Value = "TotalHours";
            Sheet.Cells["AM1"].Value = "total normalTime";
            Sheet.Cells["AN1"].Value = "TotalOverTime";
            Sheet.Cells["AO1"].Value = "TotalAbsent";
            Sheet.Cells["AP1"].Value = "TotalVL";
            Sheet.Cells["AQ1"].Value = "TotalTransefer";
            Sheet.Cells["AR1"].Value = "TotalSickLeave";
            Sheet.Cells["AS1"].Value = "FridayHours";
            Sheet.Cells["AT1"].Value = "HolidayHours";
            var i = 2;
            foreach (var tr in tranreplist)
            {
                Sheet.Cells[string.Format("A{0}", i)].Value = tr.LabourMaster.EMPNO;
                Sheet.Cells[string.Format("B{0}", i)].Value = tr.LabourMaster.Person_Name;
                Sheet.Cells[string.Format("C{0}", i)].Value = tr.LabourMaster.ManPowerSupplier.Supplier;
                Sheet.Cells[string.Format("D{0}", i)].Value = tr.LabourMaster.Position;
                Sheet.Cells[string.Format("E{0}", i)].Value = tr.MainTimeSheet.TMonth.ToString("M");
                Sheet.Cells[string.Format("F{0}", i)].Value = tr.MainTimeSheet.ProjectList.PROJECT_NAME;
                if (!tr.C1.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("G{0}", i)].Value = tr.C1;
                }
                else
                {
                    Sheet.Cells[string.Format("G{0}", i)].Value = "0";
                }

                if (!tr.C2.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("H{0}", i)].Value = tr.C2;
                }
                else
                {
                    Sheet.Cells[string.Format("H{0}", i)].Value = "0";
                }

                if (!tr.C3.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("I{0}", i)].Value = tr.C3;
                }
                else
                {
                    Sheet.Cells[string.Format("I{0}", i)].Value = "0";
                }

                if (!tr.C4.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("J{0}", i)].Value = tr.C4;
                }
                else
                {
                    Sheet.Cells[string.Format("J{0}", i)].Value = "0";
                }

                if (!tr.C5.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("K{0}", i)].Value = tr.C5;
                }
                else
                {
                    Sheet.Cells[string.Format("K{0}", i)].Value = "0";
                }

                if (!tr.C6.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("L{0}", i)].Value = tr.C6;
                }
                else
                {
                    Sheet.Cells[string.Format("L{0}", i)].Value = "0";
                }

                if (!tr.C7.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("M{0}", i)].Value = tr.C7;
                }
                else
                {
                    Sheet.Cells[string.Format("M{0}", i)].Value = "0";
                }

                if (!tr.C8.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("N{0}", i)].Value = tr.C8;
                }
                else
                {
                    Sheet.Cells[string.Format("N{0}", i)].Value = "0";
                }

                if (!tr.C9.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("O{0}", i)].Value = tr.C9;
                }
                else
                {
                    Sheet.Cells[string.Format("O{0}", i)].Value = "0";
                }

                if (!tr.C10.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("P{0}", i)].Value = tr.C10;
                }
                else
                {
                    Sheet.Cells[string.Format("P{0}", i)].Value = "0";
                }

                if (!tr.C11.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("Q{0}", i)].Value = tr.C11;
                }
                else
                {
                    Sheet.Cells[string.Format("Q{0}", i)].Value = "0";
                }

                if (!tr.C12.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("R{0}", i)].Value = tr.C12;
                }
                else
                {
                    Sheet.Cells[string.Format("R{0}", i)].Value = "0";
                }

                if (!tr.C13.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("S{0}", i)].Value = tr.C13;
                }
                else
                {
                    Sheet.Cells[string.Format("S{0}", i)].Value = "0";
                }

                if (!tr.C14.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("T{0}", i)].Value = tr.C14;
                }
                else
                {
                    Sheet.Cells[string.Format("T{0}", i)].Value = "0";
                }

                if (!tr.C15.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("U{0}", i)].Value = tr.C15;
                }
                else
                {
                    Sheet.Cells[string.Format("U{0}", i)].Value = "0";
                }

                if (!tr.C16.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("V{0}", i)].Value = tr.C16;
                }
                else
                {
                    Sheet.Cells[string.Format("V{0}", i)].Value = "0";
                }

                if (!tr.C17.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("W{0}", i)].Value = tr.C17;
                }
                else
                {
                    Sheet.Cells[string.Format("W{0}", i)].Value = "0";
                }

                if (!tr.C18.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("X{0}", i)].Value = tr.C18;
                }
                else
                {
                    Sheet.Cells[string.Format("X{0}", i)].Value = "0";
                }

                if (!tr.C19.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("Y{0}", i)].Value = tr.C19;
                }
                else
                {
                    Sheet.Cells[string.Format("Y{0}", i)].Value = "0";
                }

                if (!tr.C20.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("Z{0}", i)].Value = tr.C20;
                }
                else
                {
                    Sheet.Cells[string.Format("Z{0}", i)].Value = "0";
                }

                if (!tr.C21.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AA{0}", i)].Value = tr.C21;
                }
                else
                {
                    Sheet.Cells[string.Format("AA{0}", i)].Value = "0";
                }

                if (!tr.C22.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AB{0}", i)].Value = tr.C22;
                }
                else
                {
                    Sheet.Cells[string.Format("AB{0}", i)].Value = "0";
                }

                if (!tr.C23.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AC{0}", i)].Value = tr.C23;
                }
                else
                {
                    Sheet.Cells[string.Format("AC{0}", i)].Value = "0";
                }

                if (!tr.C24.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AD{0}", i)].Value = tr.C24;
                }
                else
                {
                    Sheet.Cells[string.Format("AD{0}", i)].Value = "0";
                }

                if (!tr.C25.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AE{0}", i)].Value = tr.C25;
                }
                else
                {
                    Sheet.Cells[string.Format("AE{0}", i)].Value = "0";
                }

                if (!tr.C26.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AF{0}", i)].Value = tr.C26;
                }
                else
                {
                    Sheet.Cells[string.Format("AF{0}", i)].Value = "0";
                }

                if (!tr.C27.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AG{0}", i)].Value = tr.C27;
                }
                else
                {
                    Sheet.Cells[string.Format("AG{0}", i)].Value = "0";
                }

                if (!tr.C28.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AH{0}", i)].Value = tr.C28;
                }
                else
                {
                    Sheet.Cells[string.Format("AH{0}", i)].Value = "0";
                }

                if (!tr.C29.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AI{0}", i)].Value = tr.C29;
                }
                else
                {
                    Sheet.Cells[string.Format("AI{0}", i)].Value = "0";
                }

                if (!tr.C30.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AJ{0}", i)].Value = tr.C30;
                }
                else
                {
                    Sheet.Cells[string.Format("AJ{0}", i)].Value = "0";
                }

                if (!tr.C31.IsNullOrWhiteSpace())
                {
                    Sheet.Cells[string.Format("AK{0}", i)].Value = tr.C31;
                }
                else
                {
                    Sheet.Cells[string.Format("AK{0}", i)].Value = "0";
                }
                var a = tr.TotalHours;
                var b = tr.TotalOverTime;
                var c = tr.FridayHours;
                var d = tr.Holidays;
                var e = tr.TotalAbsent;
                var f = tr.TotalVL;
                var g = tr.TotalTransefer;
                var h = tr.TotalSickLeave;
                var a_b = new long?();
                if (a != null || a != 0)
                {
                    if (d == null)
                    {
                        d = 0;
                    }
                    if (b == null)
                    {
                        b = 0;
                    }
                    if (c == null)
                    {
                        c = 0;
                    }
                    if (b != null || b != 0)
                    {
                        a_b = a - b - c - d;
                    }
                    else
                    {
                        a_b = a - c - d;
                    }
                }
                else
                {
                    a_b = 0;
                }
                Sheet.Cells[string.Format("AL{0}", i)].Value = a;
                Sheet.Cells[string.Format("AM{0}", i)].Value = a_b;
                Sheet.Cells[string.Format("AN{0}", i)].Value = b;
                Sheet.Cells[string.Format("AO{0}", i)].Value = e;
                Sheet.Cells[string.Format("AP{0}", i)].Value = f;
                Sheet.Cells[string.Format("AQ{0}", i)].Value = g;
                Sheet.Cells[string.Format("AR{0}", i)].Value = h;
                Sheet.Cells[string.Format("AS{0}", i)].Value = c;
                Sheet.Cells[string.Format("AT{0}", i)].Value = d;




                i++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =overtime.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }
    }
}