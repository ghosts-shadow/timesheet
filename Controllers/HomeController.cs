namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using OfficeOpenXml;

    using onlygodknows.Models;

    using PagedList;

    public class HomeController : Controller
    {

        string errr = "";
        public long ID;

        public long manid;

        public long pid;

        public DateTime Tmonth;

        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        private string errorm ="";
        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "sdiniz")]
        public ActionResult AEdit(
            [Bind(
                Include =
                    "ID,EmpID,SubMain,C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30,C31,TotalHours,TotalOverTime,TotalAbsent,AccommodationDeduction,FoodDeduction,TotalWorkingDays,TotalVL,TotalTransefer,TotalSickLeave,FridayHours,Holidays,ManPowerSupply,CompID,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,xABST,nABST,xOT,nnOT")]
            Attendance attendance)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("AIndex", "Home");
            }

            return this.View(attendance);
        }

        [Authorize(Users = "sdiniz")]
        public ActionResult AEdit(long? id)
        {
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == this.ID
                    select LabourMaster;

            this.ViewBag.EmpID = new SelectList(d.OrderBy(m => m.EMPNO), "EmpID", "EMPNO");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var attendance = this.db.Attendances.Find(id);
            if (attendance == null) return this.HttpNotFound();
            return this.View(attendance);
        }

        [Authorize(Users = "sdiniz")]
        public ActionResult AIndex(int? page, int? pagesize)
        {
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.EmpID = new SelectList(d.OrderBy(m => m.EMPNO), "ID", "EMPNO");
            var data = new[]
                           {
                               new SelectListItem { Text = "0", Value = "0" },
                               new SelectListItem { Text = "1", Value = "1" },
                               new SelectListItem { Text = "2", Value = "2" },
                               new SelectListItem { Text = "3", Value = "3" },
                               new SelectListItem { Text = "4", Value = "4" },
                               new SelectListItem { Text = "5", Value = "5" },
                               new SelectListItem { Text = "6", Value = "6" },
                               new SelectListItem { Text = "7", Value = "7" },
                               new SelectListItem { Text = "8", Value = "8" },
                               new SelectListItem { Text = "9", Value = "9" },
                               new SelectListItem { Text = "10", Value = "10" },
                               new SelectListItem { Text = "11", Value = "11" },
                               new SelectListItem { Text = "12", Value = "12" },
                               new SelectListItem { Text = "13", Value = "13" },
                               new SelectListItem { Text = "14", Value = "14" },
                               new SelectListItem { Text = "15", Value = "15" },
                               new SelectListItem { Text = "16", Value = "16" },
                               new SelectListItem { Text = "17", Value = "17" },
                               new SelectListItem { Text = "18", Value = "18" },
                               new SelectListItem { Text = "19", Value = "19" },
                               new SelectListItem { Text = "20", Value = "20" },
                               new SelectListItem { Text = "21", Value = "21" },
                               new SelectListItem { Text = "22", Value = "22" },
                               new SelectListItem { Text = "23", Value = "23" },
                               new SelectListItem { Text = "24", Value = "24" },
                               new SelectListItem { Text = "S", Value = "S" },
                               new SelectListItem { Text = "A", Value = "A" },
                               new SelectListItem { Text = "T", Value = "T" },
                               new SelectListItem { Text = "V", Value = "V" }
                           };
            this.ViewBag.C1 = data;
            this.ViewBag.C2 = data;
            this.ViewBag.C3 = data;
            this.ViewBag.C4 = data;
            this.ViewBag.C5 = data;
            this.ViewBag.C6 = data;
            this.ViewBag.C7 = data;
            this.ViewBag.C8 = data;
            this.ViewBag.C9 = data;
            this.ViewBag.C10 = data;
            this.ViewBag.C11 = data;
            this.ViewBag.C12 = data;
            this.ViewBag.C13 = data;
            this.ViewBag.C14 = data;
            this.ViewBag.C15 = data;
            this.ViewBag.C16 = data;
            this.ViewBag.C17 = data;
            this.ViewBag.C18 = data;
            this.ViewBag.C19 = data;
            this.ViewBag.C20 = data;
            this.ViewBag.C21 = data;
            this.ViewBag.C22 = data;
            this.ViewBag.C23 = data;
            this.ViewBag.C24 = data;
            this.ViewBag.C25 = data;
            this.ViewBag.C26 = data;
            this.ViewBag.C27 = data;
            this.ViewBag.C28 = data;
            this.ViewBag.C29 = data;
            this.ViewBag.C30 = data;
            this.ViewBag.C31 = data;
            var model1 = new timesheetViewModel
            {
                Attendancecollection = this.db.Attendances.Where(x => x.SubMain.Equals(aa.ID))
                                     .Include(x => x.LabourMaster).OrderByDescending(m => m.ID)
            };
            return this.View(model1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "sdiniz")]
        public ActionResult AIndex(
            [Bind(
                Include =
                    "ID,EmpID,SubMain,C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30,C31,TotalHours,TotalOverTime,TotalAbsent,AccommodationDeduction,FoodDeduction,TotalWorkingDays,TotalVL,TotalTransefer,TotalSickLeave,FridayHours,Holidays,ManPowerSupply,CompID,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,xABST,nABST,xOT,nnOT")]
            Attendance attendance)
        {
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.EmpID = new SelectList(d.OrderBy(m => m.EMPNO), "EmpID", "EMPNO");
            var data = new[]
                           {
                               new SelectListItem { Text = "0", Value = "0" },
                               new SelectListItem { Text = "1", Value = "1" },
                               new SelectListItem { Text = "2", Value = "2" },
                               new SelectListItem { Text = "3", Value = "3" },
                               new SelectListItem { Text = "4", Value = "4" },
                               new SelectListItem { Text = "5", Value = "5" },
                               new SelectListItem { Text = "6", Value = "6" },
                               new SelectListItem { Text = "7", Value = "7" },
                               new SelectListItem { Text = "8", Value = "8" },
                               new SelectListItem { Text = "9", Value = "9" },
                               new SelectListItem { Text = "10", Value = "10" },
                               new SelectListItem { Text = "11", Value = "11" },
                               new SelectListItem { Text = "12", Value = "12" },
                               new SelectListItem { Text = "13", Value = "13" },
                               new SelectListItem { Text = "14", Value = "14" },
                               new SelectListItem { Text = "15", Value = "15" },
                               new SelectListItem { Text = "16", Value = "16" },
                               new SelectListItem { Text = "17", Value = "17" },
                               new SelectListItem { Text = "18", Value = "18" },
                               new SelectListItem { Text = "19", Value = "19" },
                               new SelectListItem { Text = "20", Value = "20" },
                               new SelectListItem { Text = "21", Value = "21" },
                               new SelectListItem { Text = "22", Value = "22" },
                               new SelectListItem { Text = "23", Value = "23" },
                               new SelectListItem { Text = "24", Value = "24" },
                               new SelectListItem { Text = "S", Value = "S" },
                               new SelectListItem { Text = "A", Value = "A" },
                               new SelectListItem { Text = "T", Value = "T" },
                               new SelectListItem { Text = "V", Value = "V" }
                           };
            this.ViewBag.C1 = data;
            this.ViewBag.C2 = data;
            this.ViewBag.C3 = data;
            this.ViewBag.C4 = data;
            this.ViewBag.C5 = data;
            this.ViewBag.C6 = data;
            this.ViewBag.C7 = data;
            this.ViewBag.C8 = data;
            this.ViewBag.C9 = data;
            this.ViewBag.C10 = data;
            this.ViewBag.C11 = data;
            this.ViewBag.C12 = data;
            this.ViewBag.C13 = data;
            this.ViewBag.C14 = data;
            this.ViewBag.C15 = data;
            this.ViewBag.C16 = data;
            this.ViewBag.C17 = data;
            this.ViewBag.C18 = data;
            this.ViewBag.C19 = data;
            this.ViewBag.C20 = data;
            this.ViewBag.C21 = data;
            this.ViewBag.C22 = data;
            this.ViewBag.C23 = data;
            this.ViewBag.C24 = data;
            this.ViewBag.C25 = data;
            this.ViewBag.C26 = data;
            this.ViewBag.C27 = data;
            this.ViewBag.C28 = data;
            this.ViewBag.C29 = data;
            this.ViewBag.C30 = data;
            this.ViewBag.C31 = data;
            if (this.ModelState.IsValid)
            {
                this.db.Attendances.Add(attendance);
                this.db.SaveChanges();

                return this.RedirectToAction("AIndex");
            }

            var model1 = new timesheetViewModel { attendance = attendance };
            return this.View(model1);
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Asearch(int? page, int? pagesize, string search)
        {
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);
            var aa = a.First();
            var pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var defaSize = 100;
            if (pagesize != 0) defaSize = pagesize ?? 100;

            var list = this.db.Attendances.Include(x => x.LabourMaster).OrderByDescending(m => m.ID)
                .ToPagedList(pageIndex, defaSize);
            this.ViewBag.pagesize = defaSize;
            if (!search.IsNullOrWhiteSpace())
            {
                long aid;
                if (long.TryParse(search, out aid))
                {
                    if (aid > 3)
                    {
                        var lidf = this.db.LabourMasters.Where(x => x.EMPNO.Equals(aid)).ToList();
                        if (lidf.Count != 0)
                        {
                            var lid = lidf.First();
                            list = this.db.Attendances.Where(x => x.EmpID.Equals(lid.ID)).Include(x => x.LabourMaster)
                                .OrderByDescending(m => m.ID).ToPagedList(pageIndex, defaSize);
                        }
                    }
                    else
                    {
                        list = this.db.Attendances.OrderByDescending(m => m.ID).Include(x => x.LabourMaster)
                            .ToPagedList(pageIndex, defaSize);
                    }
                }
            }
            else
            {
                list = this.db.Attendances.OrderByDescending(m => m.ID).ToPagedList(pageIndex, defaSize);
            }

            return this.View(list);
        }

        public ActionResult Contact()
        {
            this.ViewBag.Message = "Your contact page.";

            return this.View();
        }
        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult csearch(DateTime? mtsmonth, long? csp, long? csmps, string MM)
        {
            var ll = new List<Attendance>();
            DateTime date;
            if (mtsmonth.HasValue)
            {
                DateTime.TryParse(mtsmonth.ToString(), out date);
                ViewBag.dateee = date.ToShortDateString();
            }
            else
            {
                date = DateTime.Now;
                ViewBag.dateee = date;
            }

            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }

                this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME");

            }
            else
            {
                this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");


            }
            this.ViewBag.csp1 = csp;
            this.ViewBag.csmps1 = csmps;
            this.ViewBag.mtsmonth1 = date;
            this.db.Database.CommandTimeout = 300;
            ViewBag.MM = MM;
            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            var list = this.db.Attendances.Include(x => x.LabourMaster).ToList();
            var abis = new MainTimeSheet();
            if (csmps.HasValue && csp.HasValue && mtsmonth.HasValue)
            {
                DateTime.TryParse(mtsmonth.Value.ToString(), out var dm);
                long.TryParse(csp.ToString(), out var lcsp);
                long.TryParse(csmps.ToString(), out var lcsmps);
                var ab = this.db.MainTimeSheets
                    .Where(
                        x => x.TMonth.Month.Equals(dm.Month) && x.TMonth.Year.Equals(dm.Year)
                                                             && x.ManPowerSupplier.Equals(lcsmps)
                                                             && x.Project.Equals(lcsp)).OrderBy(x => x.ID)
                    .ToPagedList(1, 100);
                if (ab.Count != 0)
                {
                    abis = ab.First();
                    return this.View(
                        this.db.Attendances.Where(x => x.SubMain.Equals(abis.ID)).Include(x => x.LabourMaster).OrderByDescending(x => x.ID)
                            .ToPagedList(1, 100));
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "the combination does not exist");
                    return this.View(ll.OrderByDescending(x => x.ID).ToPagedList(1, 100));
                }
            }


            return this.View(ll.OrderByDescending(x => x.ID).ToPagedList(1, 100));
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult MCreate()
        {
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                var t = new List<ProjectList>();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }
                this.ViewBag.Project = new SelectList(t, "ID", "PROJECT_NAME");

            }
            else
            {
                this.ViewBag.Project = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");


            }
            this.ViewBag.ManPowerSupplier = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            return this.View();
        }

        // POST: MainTimeSheets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult MCreate(
            [Bind(
                Include =
                    "ID,TMonth,ManPowerSupplier,Project,Ref,RefNo,UserName,PCName,SAV,Location,Note,Startit,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,TMnth")]
            MainTimeSheet mainTimeSheet)
        {
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                var t = new List<ProjectList>();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }

                this.ViewBag.Project = new SelectList(t, "ID", "PROJECT_NAME");

            }
            else
            {
                this.ViewBag.Project = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");


            }
            this.ViewBag.ManPowerSupplier = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            this.Tmonth = mainTimeSheet.TMonth.Date;
            this.ID = mainTimeSheet.ID;
            this.manid = mainTimeSheet.ManPowerSupplier;
            this.pid = mainTimeSheet.Project;
            if (this.ModelState.IsValid)
            {

                var apall = this.db.approvals.ToList();
                if (!apall.Exists(
                        x => x.MPS_id == mainTimeSheet.ManPowerSupplier && x.P_id == mainTimeSheet.Project
                                                                        && x.adate == mainTimeSheet.TMonth
                                                                        && (x.status.Equals("submitted") || x.status.Equals("approved"))))
                {
                    
                        this.db.MainTimeSheets.Add(mainTimeSheet);
                        this.db.SaveChanges();
                        return this.RedirectToAction("tests");
                    
                    
                }
                else
                    {
                        string errr1 = "timesheet already " + apall.Find(
                                               x => x.MPS_id == mainTimeSheet.ManPowerSupplier
                                                    && x.P_id == mainTimeSheet.Project
                                                    && x.adate == mainTimeSheet.TMonth)
                                           .status;
                        ModelState.AddModelError(string.Empty, errr1);
                    }
                    
            }
            return this.View(mainTimeSheet);
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult tests()
        {
            var at = new Attendance();
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToLongDateString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.name = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Person_Name");
            this.ViewBag.position = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Position");
            var data = new[]
                           {
                               new SelectListItem { Text = "", Value = "" },
                               new SelectListItem { Text = "1", Value = "1" },
                               new SelectListItem { Text = "2", Value = "2" },
                               new SelectListItem { Text = "3", Value = "3" },
                               new SelectListItem { Text = "4", Value = "4" },
                               new SelectListItem { Text = "5", Value = "5" },
                               new SelectListItem { Text = "6", Value = "6" },
                               new SelectListItem { Text = "7", Value = "7" },
                               new SelectListItem { Text = "8", Value = "8" },
                               new SelectListItem { Text = "9", Value = "9" },
                               new SelectListItem { Text = "10", Value = "10" },
                               new SelectListItem { Text = "11", Value = "11" },
                               new SelectListItem { Text = "12", Value = "12" },
                               new SelectListItem { Text = "13", Value = "13" },
                               new SelectListItem { Text = "14", Value = "14" },
                               new SelectListItem { Text = "15", Value = "15" },
                               new SelectListItem { Text = "16", Value = "16" },
                               new SelectListItem { Text = "17", Value = "17" },
                               new SelectListItem { Text = "18", Value = "18" },
                               new SelectListItem { Text = "19", Value = "19" },
                               new SelectListItem { Text = "20", Value = "20" },
                               new SelectListItem { Text = "21", Value = "21" },
                               new SelectListItem { Text = "22", Value = "22" },
                               new SelectListItem { Text = "23", Value = "23" },
                               new SelectListItem { Text = "24", Value = "24" },
                               new SelectListItem { Text = "S", Value = "S" },
                               new SelectListItem { Text = "A", Value = "A" },
                               new SelectListItem { Text = "T", Value = "T" },
                               new SelectListItem { Text = "V", Value = "V" }
                           };
            this.ViewBag.hours = data;
            return this.View();
        }
        [SuppressMessage(
            "StyleCop.CSharp.LayoutRules",
            "SA1503:CurlyBracketsMustNotBeOmitted",
            Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult tests(testlist test)
        {
            var at = new Attendance();
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToLongDateString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.name = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Person_Name");
            this.ViewBag.position = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Position");

            var data = new[]
                           {
                               new SelectListItem { Text = "", Value = "" },
                               new SelectListItem { Text = "1", Value = "1" },
                               new SelectListItem { Text = "2", Value = "2" },
                               new SelectListItem { Text = "3", Value = "3" },
                               new SelectListItem { Text = "4", Value = "4" },
                               new SelectListItem { Text = "5", Value = "5" },
                               new SelectListItem { Text = "6", Value = "6" },
                               new SelectListItem { Text = "7", Value = "7" },
                               new SelectListItem { Text = "8", Value = "8" },
                               new SelectListItem { Text = "9", Value = "9" },
                               new SelectListItem { Text = "10", Value = "10" },
                               new SelectListItem { Text = "11", Value = "11" },
                               new SelectListItem { Text = "12", Value = "12" },
                               new SelectListItem { Text = "13", Value = "13" },
                               new SelectListItem { Text = "14", Value = "14" },
                               new SelectListItem { Text = "15", Value = "15" },
                               new SelectListItem { Text = "16", Value = "16" },
                               new SelectListItem { Text = "17", Value = "17" },
                               new SelectListItem { Text = "18", Value = "18" },
                               new SelectListItem { Text = "19", Value = "19" },
                               new SelectListItem { Text = "20", Value = "20" },
                               new SelectListItem { Text = "21", Value = "21" },
                               new SelectListItem { Text = "22", Value = "22" },
                               new SelectListItem { Text = "23", Value = "23" },
                               new SelectListItem { Text = "24", Value = "24" },
                               new SelectListItem { Text = "S", Value = "S" },
                               new SelectListItem { Text = "A", Value = "A" },
                               new SelectListItem { Text = "T", Value = "T" },
                               new SelectListItem { Text = "V", Value = "V" }
                           };
            this.ViewBag.hours = data;
            var oldmts = new List<MainTimeSheet>();
            if (this.ModelState.IsValid)
            {
                var check = new List<Attendance>();
                var oldmts1 = new MainTimeSheet();
                var apall = this.db.approvals.ToList();
                foreach (var list in test.Tests)
                {
                    var trial = list.date = aa.TMonth;
                    oldmts = this.db.MainTimeSheets
                        .Where(
                            x => x.TMonth.Month.Equals(list.date.Month) && x.TMonth.Year.Equals(list.date.Year)
                                                                        && x.ManPowerSupplier.Equals(
                                                                            aa.ManPowerSupplier)
                                                                        && x.Project.Equals(aa.Project)).OrderByDescending(x => x.ID).ToList();
                    

                        if (oldmts.Count != 0)
                        {
                            oldmts1 = oldmts.Last();

                            // First();
                            check = this.db.Attendances
                                .Where(z => z.EmpID.Equals(list.empno) && z.SubMain.Equals(oldmts1.ID)).ToList();
                        }
                        else
                        {
                            check = this.db.Attendances.Where(z => z.EmpID.Equals(list.empno) && z.SubMain.Equals(aa.ID))
                                .ToList();
                        }

                        {
                            var tb = this.db;
                            var lo = this.db.MainTimeSheets.Where(
                                e => e.ManPowerSupplier.Equals(aa.ManPowerSupplier) && e.TMonth.Year.Equals(list.date.Year)
                                                                                    && e.TMonth.Month.Equals(
                                                                                        list.date.Month)).ToList();
                            List<Attendance> lo1 = new List<Attendance>();
                            foreach (var same in lo)
                            {
                                var at1 = this.db.Attendances.Where(e => e.SubMain.Equals(same.ID)).ToList();
                                if (at1 != null)
                                {
                                    foreach (var at2 in at1)
                                    {
                                        lo1.Add(at2);
                                    }
                                }
                            }

                            List<Attendance> lo3 = new List<Attendance>();
                            foreach (var lo2 in lo1)
                            {
                                if (list.empno == lo2.EmpID)
                                {
                                    lo3.Add(lo2);
                                }
                            }

                            long com = 0;
                            foreach (var at1 in lo3)
                            {
                                if (check.Count != 0) at = check.First();
                                if (!at1.SubMain.Equals(at.SubMain))
                                {


                                    if (list.date.Day == 1)
                                    {
                                        long.TryParse(at1.C1, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 2)
                                    {
                                        long.TryParse(at1.C2, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 3)
                                    {
                                        long.TryParse(at1.C3, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 4)
                                    {
                                        long.TryParse(at1.C4, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 5)
                                    {
                                        long.TryParse(at1.C5, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 6)
                                    {
                                        long.TryParse(at1.C6, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 7)
                                    {
                                        long.TryParse(at1.C7, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 8)
                                    {
                                        long.TryParse(at1.C8, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }

                                    }
                                    if (list.date.Day == 9)
                                    {
                                        long.TryParse(at1.C9, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }
                                    if (list.date.Day == 10)
                                    {
                                        long.TryParse(at1.C10, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 11)
                                    {
                                        long.TryParse(at1.C11, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 12)
                                    {
                                        long.TryParse(at1.C12, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 13)
                                    {
                                        long.TryParse(at1.C13, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 14)
                                    {
                                        long.TryParse(at1.C14, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 15)
                                    {
                                        long.TryParse(at1.C15, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 16)
                                    {
                                        long.TryParse(at1.C16, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 17)
                                    {
                                        long.TryParse(at1.C17, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 18)
                                    {
                                        long.TryParse(at1.C18, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 19)
                                    {
                                        long.TryParse(at1.C19, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 20)
                                    {
                                        long.TryParse(at1.C20, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 21)
                                    {
                                        long.TryParse(at1.C21, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 22)
                                    {
                                        long.TryParse(at1.C22, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 23)
                                    {
                                        long.TryParse(at1.C23, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 24)
                                    {
                                        long.TryParse(at1.C24, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 25)
                                    {
                                        long.TryParse(at1.C25, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 26)
                                    {
                                        long.TryParse(at1.C26, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 27)
                                    {
                                        long.TryParse(at1.C27, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 28)
                                    {
                                        long.TryParse(at1.C28, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 29)
                                    {
                                        long.TryParse(at1.C29, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 30)
                                    {
                                        long.TryParse(at1.C30, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }

                                    if (list.date.Day == 31)
                                    {
                                        long.TryParse(at1.C31, out long k);
                                        long.TryParse(list.hours, out long l);
                                        com = k + l;
                                        if (com > 24)
                                        {
                                            var dd = this.db.LabourMasters.Find(list.empno);
                                            string errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                             + " for the day is "
                                                                                             + com.ToString()
                                                                                             + "hrs which is greater then 24hrs";
                                            ModelState.AddModelError(string.Empty, errorm);
                                            return View(test);
                                        }
                                    }
                                }
                            }
                        }
                        if (check.Count != 0) at = check.First();
                        else at.SubMain = oldmts1.ID;
                        at.EmpID = list.empno;
                        list.date = aa.TMonth;
                        {
                            if (list.date.Day == 1) at.C1 = list.hours;
                            if (list.date.Day == 2) at.C2 = list.hours;
                            if (list.date.Day == 3) at.C3 = list.hours;
                            if (list.date.Day == 4) at.C4 = list.hours;
                            if (list.date.Day == 5) at.C5 = list.hours;
                            if (list.date.Day == 6) at.C6 = list.hours;
                            if (list.date.Day == 7) at.C7 = list.hours;
                            if (list.date.Day == 8) at.C8 = list.hours;
                            if (list.date.Day == 9) at.C9 = list.hours;
                            if (list.date.Day == 10) at.C10 = list.hours;
                            if (list.date.Day == 11) at.C11 = list.hours;
                            if (list.date.Day == 12) at.C12 = list.hours;
                            if (list.date.Day == 13) at.C13 = list.hours;
                            if (list.date.Day == 14) at.C14 = list.hours;
                            if (list.date.Day == 15) at.C15 = list.hours;
                            if (list.date.Day == 16) at.C16 = list.hours;
                            if (list.date.Day == 17) at.C17 = list.hours;
                            if (list.date.Day == 18) at.C18 = list.hours;
                            if (list.date.Day == 19) at.C19 = list.hours;
                            if (list.date.Day == 20) at.C20 = list.hours;
                            if (list.date.Day == 21) at.C21 = list.hours;
                            if (list.date.Day == 22) at.C22 = list.hours;
                            if (list.date.Day == 23) at.C23 = list.hours;
                            if (list.date.Day == 24) at.C24 = list.hours;
                            if (list.date.Day == 25) at.C25 = list.hours;
                            if (list.date.Day == 26) at.C26 = list.hours;
                            if (list.date.Day == 27) at.C27 = list.hours;
                            if (list.date.Day == 28) at.C28 = list.hours;
                            if (list.date.Day == 29) at.C29 = list.hours;
                            if (list.date.Day == 30) at.C30 = list.hours;
                            if (list.date.Day == 31) at.C31 = list.hours;
                        }
                        if (check.Count != 0)
                        {
                            this.db.Entry(at).State = EntityState.Modified;
                            this.db.SaveChanges();
                        }
                        else
                        {
                            this.db.Attendances.Add(at);
                            this.db.SaveChanges();
                        }

                        if (oldmts.Count != 0)
                        {
                            oldmts1 = oldmts.Last();

                            // First();
                            check = this.db.Attendances
                                .Where(z => z.EmpID.Equals(list.empno) && z.SubMain.Equals(oldmts1.ID)).ToList();
                        }
                        else
                        {
                            check = this.db.Attendances.Where(z => z.EmpID.Equals(list.empno) && z.SubMain.Equals(aa.ID))
                                .ToList();
                        }

                        if (check.Count != 0) at = check.First();
                        else at.SubMain = oldmts1.ID;
                        long fri1 = 0;
                        var date = new DateTime(list.date.Year, list.date.Month, 1);
                        for (var i = 0; i < DateTime.DaysInMonth(list.date.Year, list.date.Month); i++)
                        {
                            if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                            {
                                if (date.Day == 1)
                                {
                                    long.TryParse(at.C1, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 2)
                                {
                                    long.TryParse(at.C2, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 3)
                                {
                                    long.TryParse(at.C3, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 4)
                                {
                                    long.TryParse(at.C4, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 5)
                                {
                                    long.TryParse(at.C5, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 6)
                                {
                                    long.TryParse(at.C6, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 7)
                                {
                                    long.TryParse(at.C7, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 8)
                                {
                                    long.TryParse(at.C8, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 9)
                                {
                                    long.TryParse(at.C9, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 10)
                                {
                                    long.TryParse(at.C10, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 11)
                                {
                                    long.TryParse(at.C11, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 12)
                                {
                                    long.TryParse(at.C11, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 13)
                                {
                                    long.TryParse(at.C13, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 14)
                                {
                                    long.TryParse(at.C14, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 15)
                                {
                                    long.TryParse(at.C15, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 16)
                                {
                                    long.TryParse(at.C16, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 17)
                                {
                                    long.TryParse(at.C17, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 18)
                                {
                                    long.TryParse(at.C18, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 19)
                                {
                                    long.TryParse(at.C19, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 20)
                                {
                                    long.TryParse(at.C20, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 21)
                                {
                                    long.TryParse(at.C21, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 22)
                                {
                                    long.TryParse(at.C22, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 23)
                                {
                                    long.TryParse(at.C23, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 24)
                                {
                                    long.TryParse(at.C24, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 25)
                                {
                                    long.TryParse(at.C25, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 26)
                                {
                                    long.TryParse(at.C26, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 27)
                                {
                                    long.TryParse(at.C27, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 28)
                                {
                                    long.TryParse(at.C28, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 29)
                                {
                                    long.TryParse(at.C29, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 30)
                                {
                                    long.TryParse(at.C30, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                if (date.Day == 31)
                                {
                                    long.TryParse(at.C31, out var tl);
                                    fri1 = fri1 + tl;
                                }

                                long.TryParse(list.hours, out var fri);
                                at.FridayHours = fri1;
                            }

                            date = date.AddDays(1);
                        }

                        {
                            at.TotalHours = 0;
                            long.TryParse(at.C1, out var tl);
                            long.TryParse(at.C2, out var tl1);
                            long.TryParse(at.C3, out var tl2);
                            long.TryParse(at.C4, out var tl3);
                            long.TryParse(at.C5, out var tl4);
                            long.TryParse(at.C6, out var tl5);
                            long.TryParse(at.C7, out var tl6);
                            long.TryParse(at.C8, out var tl7);
                            long.TryParse(at.C9, out var tl8);
                            long.TryParse(at.C10, out var tl9);
                            long.TryParse(at.C11, out var tl10);
                            long.TryParse(at.C12, out var tl11);
                            long.TryParse(at.C13, out var tl12);
                            long.TryParse(at.C14, out var tl13);
                            long.TryParse(at.C15, out var tl14);
                            long.TryParse(at.C16, out var tl15);
                            long.TryParse(at.C17, out var tl16);
                            long.TryParse(at.C18, out var tl17);
                            long.TryParse(at.C19, out var tl18);
                            long.TryParse(at.C20, out var tl19);
                            long.TryParse(at.C21, out var tl20);
                            long.TryParse(at.C22, out var tl21);
                            long.TryParse(at.C23, out var tl22);
                            long.TryParse(at.C24, out var tl23);
                            long.TryParse(at.C25, out var tl24);
                            long.TryParse(at.C26, out var tl25);
                            long.TryParse(at.C27, out var tl26);
                            long.TryParse(at.C28, out var tl27);
                            long.TryParse(at.C29, out var tl28);
                            long.TryParse(at.C30, out var tl29);
                            long.TryParse(at.C31, out var tl30);
                            long.TryParse(at.TotalHours.ToString(), out var atlg);
                            at.TotalHours = tl + tl1 + tl2 + tl3 + tl4 + tl5 + tl6 + tl7 + tl8 + tl9 + tl10 + tl11 + tl12
                                            + tl13 + tl14 + tl15 + tl16 + tl17 + tl18 + tl19 + tl20 + tl21 + tl22 + tl23
                                            + tl24 + tl25 + tl26 + tl27 + tl28 + tl29 + tl30;
                            double.TryParse(b.NormalTimeUpto.ToString(), out var tho);
                            {
                                var t = new List<long>();
                                at.TotalOverTime = 0;
                                t.Add(tl);
                                t.Add(tl1);
                                t.Add(tl2);
                                t.Add(tl3);
                                t.Add(tl4);
                                t.Add(tl5);
                                t.Add(tl6);
                                t.Add(tl7);
                                t.Add(tl8);
                                t.Add(tl9);
                                t.Add(tl10);
                                t.Add(tl11);
                                t.Add(tl12);
                                t.Add(tl13);
                                t.Add(tl14);
                                t.Add(tl15);
                                t.Add(tl16);
                                t.Add(tl17);
                                t.Add(tl18);
                                t.Add(tl19);
                                t.Add(tl20);
                                t.Add(tl21);
                                t.Add(tl22);
                                t.Add(tl23);
                                t.Add(tl24);
                                t.Add(tl25);
                                t.Add(tl26);
                                t.Add(tl27);
                                t.Add(tl28);
                                t.Add(tl29);
                                t.Add(tl30);
                                long tho1 = 0;
                                foreach (var l in t)
                                    if (l > tho)
                                    {
                                        tho1 += l - (long)tho;
                                        at.TotalOverTime = tho1;
                                    }
                            }
                        }
                        {
                            at.TotalSickLeave = 0;
                            long ts = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                                if (at.C1.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C2.IsNullOrWhiteSpace())
                                if (at.C2.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C3.IsNullOrWhiteSpace())
                                if (at.C3.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C4.IsNullOrWhiteSpace())
                                if (at.C4.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C5.IsNullOrWhiteSpace())
                                if (at.C5.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C6.IsNullOrWhiteSpace())
                                if (at.C6.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C7.IsNullOrWhiteSpace())
                                if (at.C7.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C8.IsNullOrWhiteSpace())
                                if (at.C8.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C9.IsNullOrWhiteSpace())
                                if (at.C9.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C10.IsNullOrWhiteSpace())
                                if (at.C10.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C11.IsNullOrWhiteSpace())
                                if (at.C11.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C12.IsNullOrWhiteSpace())
                                if (at.C12.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C13.IsNullOrWhiteSpace())
                                if (at.C13.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C14.IsNullOrWhiteSpace())
                                if (at.C14.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C15.IsNullOrWhiteSpace())
                                if (at.C15.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C16.IsNullOrWhiteSpace())
                                if (at.C16.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C17.IsNullOrWhiteSpace())
                                if (at.C17.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C18.IsNullOrWhiteSpace())
                                if (at.C18.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C19.IsNullOrWhiteSpace())
                                if (at.C19.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C20.IsNullOrWhiteSpace())
                                if (at.C20.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C21.IsNullOrWhiteSpace())
                                if (at.C21.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C22.IsNullOrWhiteSpace())
                                if (at.C22.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C23.IsNullOrWhiteSpace())
                                if (at.C23.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C24.IsNullOrWhiteSpace())
                                if (at.C24.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C25.IsNullOrWhiteSpace())
                                if (at.C25.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C26.IsNullOrWhiteSpace())
                                if (at.C26.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C27.IsNullOrWhiteSpace())
                                if (at.C27.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C28.IsNullOrWhiteSpace())
                                if (at.C28.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C29.IsNullOrWhiteSpace())
                                if (at.C29.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C30.IsNullOrWhiteSpace())
                                if (at.C30.Equals("S"))
                                    ts = ts + 1;
                            if (!at.C31.IsNullOrWhiteSpace())
                                if (at.C31.Equals("S"))
                                    ts = ts + 1;

                            at.TotalSickLeave = ts;
                        }
                        {
                            at.TotalVL = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                                if (at.C1.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C2.IsNullOrWhiteSpace())
                                if (at.C2.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C3.IsNullOrWhiteSpace())
                                if (at.C3.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C4.IsNullOrWhiteSpace())
                                if (at.C4.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C5.IsNullOrWhiteSpace())
                                if (at.C5.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C6.IsNullOrWhiteSpace())
                                if (at.C6.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C7.IsNullOrWhiteSpace())
                                if (at.C7.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C8.IsNullOrWhiteSpace())
                                if (at.C8.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C9.IsNullOrWhiteSpace())
                                if (at.C9.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C10.IsNullOrWhiteSpace())
                                if (at.C10.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C11.IsNullOrWhiteSpace())
                                if (at.C11.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C12.IsNullOrWhiteSpace())
                                if (at.C12.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C13.IsNullOrWhiteSpace())
                                if (at.C13.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C14.IsNullOrWhiteSpace())
                                if (at.C14.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C15.IsNullOrWhiteSpace())
                                if (at.C15.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C16.IsNullOrWhiteSpace())
                                if (at.C16.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C17.IsNullOrWhiteSpace())
                                if (at.C17.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C18.IsNullOrWhiteSpace())
                                if (at.C18.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C19.IsNullOrWhiteSpace())
                                if (at.C19.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C20.IsNullOrWhiteSpace())
                                if (at.C20.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C21.IsNullOrWhiteSpace())
                                if (at.C21.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C22.IsNullOrWhiteSpace())
                                if (at.C22.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C23.IsNullOrWhiteSpace())
                                if (at.C23.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C24.IsNullOrWhiteSpace())
                                if (at.C24.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C25.IsNullOrWhiteSpace())
                                if (at.C25.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C26.IsNullOrWhiteSpace())
                                if (at.C26.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C27.IsNullOrWhiteSpace())
                                if (at.C27.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C28.IsNullOrWhiteSpace())
                                if (at.C28.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C29.IsNullOrWhiteSpace())
                                if (at.C29.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C30.IsNullOrWhiteSpace())
                                if (at.C30.Equals("V"))
                                    tv = tv + 1;
                            if (!at.C31.IsNullOrWhiteSpace())
                                if (at.C31.Equals("V"))
                                    tv = tv + 1;

                            at.TotalVL = tv;
                        }
                        {
                            at.TotalAbsent = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                                if (at.C1.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C2.IsNullOrWhiteSpace())
                                if (at.C2.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C3.IsNullOrWhiteSpace())
                                if (at.C3.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C4.IsNullOrWhiteSpace())
                                if (at.C4.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C5.IsNullOrWhiteSpace())
                                if (at.C5.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C6.IsNullOrWhiteSpace())
                                if (at.C6.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C7.IsNullOrWhiteSpace())
                                if (at.C7.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C8.IsNullOrWhiteSpace())
                                if (at.C8.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C9.IsNullOrWhiteSpace())
                                if (at.C9.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C10.IsNullOrWhiteSpace())
                                if (at.C10.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C11.IsNullOrWhiteSpace())
                                if (at.C11.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C12.IsNullOrWhiteSpace())
                                if (at.C12.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C13.IsNullOrWhiteSpace())
                                if (at.C13.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C14.IsNullOrWhiteSpace())
                                if (at.C14.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C15.IsNullOrWhiteSpace())
                                if (at.C15.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C16.IsNullOrWhiteSpace())
                                if (at.C16.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C17.IsNullOrWhiteSpace())
                                if (at.C17.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C18.IsNullOrWhiteSpace())
                                if (at.C18.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C19.IsNullOrWhiteSpace())
                                if (at.C19.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C20.IsNullOrWhiteSpace())
                                if (at.C20.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C21.IsNullOrWhiteSpace())
                                if (at.C21.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C22.IsNullOrWhiteSpace())
                                if (at.C22.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C23.IsNullOrWhiteSpace())
                                if (at.C23.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C24.IsNullOrWhiteSpace())
                                if (at.C24.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C25.IsNullOrWhiteSpace())
                                if (at.C25.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C26.IsNullOrWhiteSpace())
                                if (at.C26.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C27.IsNullOrWhiteSpace())
                                if (at.C27.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C28.IsNullOrWhiteSpace())
                                if (at.C28.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C29.IsNullOrWhiteSpace())
                                if (at.C29.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C30.IsNullOrWhiteSpace())
                                if (at.C30.Equals("A"))
                                    tv = tv + 1;
                            if (!at.C31.IsNullOrWhiteSpace())
                                if (at.C31.Equals("A"))
                                    tv = tv + 1;

                            at.TotalAbsent = tv;
                        }
                        {
                            at.TotalTransefer = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                                if (at.C1.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C2.IsNullOrWhiteSpace())
                                if (at.C2.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C3.IsNullOrWhiteSpace())
                                if (at.C3.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C4.IsNullOrWhiteSpace())
                                if (at.C4.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C5.IsNullOrWhiteSpace())
                                if (at.C5.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C6.IsNullOrWhiteSpace())
                                if (at.C6.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C7.IsNullOrWhiteSpace())
                                if (at.C7.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C8.IsNullOrWhiteSpace())
                                if (at.C8.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C9.IsNullOrWhiteSpace())
                                if (at.C9.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C10.IsNullOrWhiteSpace())
                                if (at.C10.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C11.IsNullOrWhiteSpace())
                                if (at.C11.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C12.IsNullOrWhiteSpace())
                                if (at.C12.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C13.IsNullOrWhiteSpace())
                                if (at.C13.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C14.IsNullOrWhiteSpace())
                                if (at.C14.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C15.IsNullOrWhiteSpace())
                                if (at.C15.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C16.IsNullOrWhiteSpace())
                                if (at.C16.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C17.IsNullOrWhiteSpace())
                                if (at.C17.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C18.IsNullOrWhiteSpace())
                                if (at.C18.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C19.IsNullOrWhiteSpace())
                                if (at.C19.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C20.IsNullOrWhiteSpace())
                                if (at.C20.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C21.IsNullOrWhiteSpace())
                                if (at.C21.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C22.IsNullOrWhiteSpace())
                                if (at.C22.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C23.IsNullOrWhiteSpace())
                                if (at.C23.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C24.IsNullOrWhiteSpace())
                                if (at.C24.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C25.IsNullOrWhiteSpace())
                                if (at.C25.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C26.IsNullOrWhiteSpace())
                                if (at.C26.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C27.IsNullOrWhiteSpace())
                                if (at.C27.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C28.IsNullOrWhiteSpace())
                                if (at.C28.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C29.IsNullOrWhiteSpace())
                                if (at.C29.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C30.IsNullOrWhiteSpace())
                                if (at.C30.Equals("T"))
                                    tv = tv + 1;
                            if (!at.C31.IsNullOrWhiteSpace())
                                if (at.C31.Equals("T"))
                                    tv = tv + 1;

                            at.TotalTransefer = tv;
                        }
                        at.status = "panding";
                        if (check.Count != 0)
                        {
                            this.db.Entry(at).State = EntityState.Modified;
                            this.db.SaveChanges();
                        }
                        else
                        {
                            this.db.Attendances.Add(at);
                            this.db.SaveChanges();
                        }
                    
                }
            return this.RedirectToAction("MCreate");
            }
            return this.View(test);
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult download(DateTime? mtsmonth2, long? csp2, long? csmps2)
        {
            this.errorm = this.TempData["mydata"] as string;
            this.ModelState.AddModelError(string.Empty, this.errorm);
            DateTime date;
            var final1 = new List<test>();
            if (mtsmonth2.HasValue)
            {
                DateTime.TryParse(mtsmonth2.ToString(), out date);
                ViewBag.dateee = date.ToShortDateString();
            }
            else
            {
                date = DateTime.Now;
                ViewBag.dateee = date.ToShortDateString();
            }
            var apall = this.db.approvals.ToList();
            this.ViewBag.csp1 = csp2;
            this.ViewBag.csmps1 = csmps2;
            this.ViewBag.mtsmonth1 = date;
            this.db.Database.CommandTimeout = 300;
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }

                this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME");

            }
            else
            {
                this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");


            }
            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            var list = this.db.Attendances.Include(x => x.LabourMaster).ToList();
            if (csmps2.HasValue && csp2.HasValue && mtsmonth2.HasValue)
            {
                DateTime.TryParse(mtsmonth2.Value.ToString(), out var dm);
                long.TryParse(csp2.ToString(), out var lcsp);
                long.TryParse(csmps2.ToString(), out var lcsmps);
                var ab = this.db.MainTimeSheets
                    .Where(x => x.TMonth.Month.Equals(dm.Month) && x.TMonth.Year.Equals(dm.Year) && x.ManPowerSupplier.Equals(lcsmps) && x.Project.Equals(lcsp))
                    .OrderBy(x => x.ID).ToList();


                foreach (var abis in ab)
                {
                    var ass = this.db.Attendances.Where(x => x.SubMain.Equals(abis.ID)).Include(x => x.LabourMaster).ToList();
                    /**/
                    foreach (var attendance in ass)
                    {
                        var et = new test();
                        var epno = this.db.LabourMasters.Find(attendance.EmpID);
                        et.empno = epno.EMPNO;
                        if (apall.Exists(x => x.A_id == attendance.ID && x.adate == dm))
                        {
                            if (apall.Exists(x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm))
                            {
                                et.approved_by = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).Ausername;
                                et.status = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).status;
                                et.submitted_by = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).Susername;
                            }
                        }

                        if (date.Day == 1) et.hours = attendance.C1;

                        if (date.Day == 2) et.hours = attendance.C2;

                        if (date.Day == 3) et.hours = attendance.C3;

                        if (date.Day == 4) et.hours = attendance.C4;

                        if (date.Day == 5) et.hours = attendance.C5;

                        if (date.Day == 6) et.hours = attendance.C6;

                        if (date.Day == 7) et.hours = attendance.C7;

                        if (date.Day == 8) et.hours = attendance.C8;

                        if (date.Day == 9) et.hours = attendance.C9;

                        if (date.Day == 10) et.hours = attendance.C10;

                        if (date.Day == 11) et.hours = attendance.C11;

                        if (date.Day == 12) et.hours = attendance.C12;

                        if (date.Day == 13) et.hours = attendance.C13;

                        if (date.Day == 14) et.hours = attendance.C14;

                        if (date.Day == 15) et.hours = attendance.C15;

                        if (date.Day == 16) et.hours = attendance.C16;

                        if (date.Day == 17) et.hours = attendance.C17;

                        if (date.Day == 18) et.hours = attendance.C18;

                        if (date.Day == 19) et.hours = attendance.C19;

                        if (date.Day == 20) et.hours = attendance.C20;

                        if (date.Day == 21) et.hours = attendance.C21;

                        if (date.Day == 22) et.hours = attendance.C22;

                        if (date.Day == 23) et.hours = attendance.C23;

                        if (date.Day == 24) et.hours = attendance.C24;

                        if (date.Day == 25) et.hours = attendance.C25;

                        if (date.Day == 26) et.hours = attendance.C26;

                        if (date.Day == 27) et.hours = attendance.C27;

                        if (date.Day == 28) et.hours = attendance.C28;

                        if (date.Day == 29) et.hours = attendance.C29;

                        if (date.Day == 30) et.hours = attendance.C30;

                        if (date.Day == 31) et.hours = attendance.C31;

                        if (!final1.Exists(x => x.empno.Equals(et.empno)))
                        {
                            if (et.hours != null)
                            {
                                final1.Add(et);
                            }
                        }

                    }
                }
                return this.View(final1.OrderBy(x => x.empno).ToPagedList(1, 100));

            }
            return this.View(final1.OrderBy(x => x.empno).ToPagedList(1, 100));

        }
        [Authorize(Roles = "Employee")]
        public ActionResult approval(DateTime? mtsmonth2, long? csp2, long? csmps2)
        {
            var final1 = new List<test>();
            if (csmps2.HasValue && csp2.HasValue && mtsmonth2.HasValue)
            {
                DateTime.TryParse(mtsmonth2.Value.ToString(), out var dm);
                long.TryParse(csp2.ToString(), out var lcsp);
                long.TryParse(csmps2.ToString(), out var lcsmps);
                var ab = this.db.MainTimeSheets
                    .Where(
                        x => x.TMonth.Month.Equals(dm.Month) && x.TMonth.Year.Equals(dm.Year)
                                                             && x.ManPowerSupplier.Equals(lcsmps)
                                                             && x.Project.Equals(lcsp)).OrderBy(x => x.ID).ToList();
                var apall = this.db.approvals.ToList();
                foreach (var abis in ab)
                {
                    var ass = this.db.Attendances.Where(x => x.SubMain.Equals(abis.ID)).Include(x => x.LabourMaster)
                        .ToList();
                    /**/
                    foreach (var attendance in ass)
                    {
                        if (!apall.Exists(x => x.A_id == attendance.ID && x.adate==dm))
                        {
                            if (User.IsInRole("Employee"))
                            {
                                attendance.status = "submitted for " + dm.Day;
                                this.db.Entry(attendance).State = EntityState.Modified;
                                this.db.SaveChanges();
                                var ap = new approval();
                                ap.MPS_id = csmps2;
                                ap.P_id = csp2;
                                ap.adate = dm;
                                ap.status = "submitted";
                                ap.A_id = attendance.ID;
                                ap.Susername = User.Identity.Name;
                                ap.Empno = attendance.LabourMaster.EMPNO;
                                this.db.approvals.Add(ap);
                                this.db.SaveChanges();
                            }
                        }
                        else
                        {
                            var aaa = apall.FindAll(x => x.A_id == attendance.ID);
                            foreach (var aa1 in aaa)
                            {
                                if (aa1.status == "submitted" && aa1.adate == dm)
                            {
                                this.errorm =  "\n already submitted;";
                                this.ModelState.AddModelError(string.Empty, this.errorm);
                                TempData["mydata"] = this.errorm;
                            }
                            if (aa1.status.Contains("rejected")  && aa1.adate == dm)
                            {
                                if (User.IsInRole("Employee"))
                                {
                                    attendance.status = "submitted for " + dm.Day;
                                    this.db.Entry(attendance).State = EntityState.Modified;
                                    this.db.SaveChanges();
                                    aa1.status = "submitted";
                                    this.db.Entry(aa1).State = EntityState.Modified;
                                    this.db.SaveChanges();
                                }
                            }
                            if (aa1.status == "approved" && aa1.adate == dm)
                            {
                                this.errorm = "\n already approved;";
                                this.ModelState.AddModelError(string.Empty, this.errorm);
                                TempData["mydata"] = this.errorm;
                            }
                            }
                        }
                    }
                }
            }

            return this.RedirectToAction("download");
        }

        [Authorize(Roles = "Admin")]
        public void DownloadExcel(DateTime? mtsmonth1, long? csp1, long? csmps1)
        {
            List<Attendance> passexel;
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("Attendances");
            var row = 4;
            var pcount = 5;
            DateTime.TryParse(mtsmonth1.ToString(), out var date);
            var p = this.db.ProjectLists.Find(csp1);
            var m = this.db.ManPowerSuppliers.Find(csmps1);
            Sheet.Cells["A1"].Value = p.PROJECT_NAME;
            Sheet.Cells["C1"].Value = m.Supplier;
            Sheet.Cells["E1"].Value = date.ToLongDateString();

            Sheet.Cells["A3"].Value = "Emp ID";
            Sheet.Cells["B3"].Value = "Hrs";
            Sheet.Cells["C3"].Value = "Emp ID";
            Sheet.Cells["D3"].Value = "Hrs";
            Sheet.Cells["E3"].Value = "Emp ID";
            Sheet.Cells["F3"].Value = "Hrs";
            Sheet.Cells["G3"].Value = "Emp ID";
            Sheet.Cells["H3"].Value = "Hrs";
            Sheet.Cells["I3"].Value = "Emp ID";
            Sheet.Cells["J3"].Value = "Hrs";
            long.TryParse(csmps1.ToString(), out var mcs);
            long.TryParse(csp1.ToString(), out var pcs);
            var Msum = this.db.MainTimeSheets.Where(
                y => y.Project == pcs && y.ManPowerSupplier == mcs && y.TMonth.Month == date.Month
                     && y.TMonth.Year == date.Year).ToList();
            foreach (var sum in Msum)
            {
                passexel = this.db.Attendances.Where(x => x.SubMain.Equals(sum.ID)).OrderByDescending(x => x.ID)
                    .ToList();
                for (var i = 0; i < passexel.Count; i = i + 5)
                {
                    if (date.Day == 1)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C1;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C1;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C1;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C1;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C1;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 2)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C2;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C2;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C2;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C2;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C2;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 3)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C3;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C3;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C3;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C3;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C3;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 4)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C4;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C4;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C4;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C4;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C4;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 5)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C5;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C5;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C5;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C5;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C5;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 6)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C6;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C6;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 2].C6;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C6;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C6;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 7)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C7;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C7;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C7;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C7;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C7;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 8)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C8;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C8;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C8;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C8;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C8;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 9)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C9;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C9;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C9;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C9;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C9;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 10)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C10;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C10;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C10;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C10;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C10;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 11)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C11;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C11;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C11;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C11;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C11;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 12)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C12;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C12;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C12;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C12;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C12;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 13)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C13;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C13;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C13;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C13;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C13;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 14)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C14;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C14;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C14;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C14;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C14;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 15)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C15;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C15;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C15;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C15;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C15;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 16)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C16;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C16;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C16;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C16;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C16;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 17)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C17;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C17;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C17;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C17;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C17;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 18)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C18;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C18;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C18;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C18;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C18;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 19)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C19;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C19;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C19;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C19;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C19;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 20)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C20;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C20;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C20;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C20;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C20;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 21)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C21;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C21;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C21;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C21;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C21;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 22)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C22;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C22;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C22;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C22;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C22;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 23)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C23;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C23;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C23;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C23;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C23;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 24)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C24;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C24;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C24;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C24;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C24;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 25)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C25;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C25;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C25;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C25;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C25;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 26)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C26;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C26;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C26;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C26;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C26;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 27)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C27;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C27;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C27;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C27;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C27;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 28)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C28;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C28;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C28;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C28;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C28;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 29)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C29;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C29;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C29;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C29;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C29;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 30)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C30;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C30;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C30;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C30;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C30;
                                    }
                                }
                            }
                        }
                    }

                    if (date.Day == 31)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                        Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C31;
                        if (passexel.Count > 1)
                        {
                            Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i + 1].LabourMaster.EMPNO;
                            Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i + 1].C31;
                            if (passexel.Count > 2)
                            {
                                Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i + 2].LabourMaster.EMPNO;
                                Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i + 2].C31;
                                if (passexel.Count > 3)
                                {
                                    Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i + 3].LabourMaster.EMPNO;
                                    Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i + 3].C31;
                                    if (passexel.Count > 4)
                                    {
                                        Sheet.Cells[string.Format("I{0}", row)].Value =
                                            passexel[i + 4].LabourMaster.EMPNO;
                                        Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i + 4].C31;
                                    }
                                }
                            }
                        }
                    }

                    row++;
                }

                pcount = pcount + passexel.Count;
            }

            Sheet.Cells["A" + pcount].Value = "Prepared by";
            Sheet.Cells["D" + pcount].Value = "Reviewed by";
            Sheet.Cells["H" + pcount].Value = "Approved ";
            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =Attendances.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }
    }
}