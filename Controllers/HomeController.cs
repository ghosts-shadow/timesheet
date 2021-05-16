namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.Services;
    using EASendMail;
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;
    using Microsoft.Exchange.WebServices.Data;
    using OfficeOpenXml;
    using onlygodknows.Models;
    using Owin;
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Web;
    using System.Web.Mvc;
    using MimeKit;
    using SmtpClient = MailKit.Net.Smtp.SmtpClient;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }

        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", this.Name, this.Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[this.Name] = this.Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }

    public class HomeController : Controller
    {
        private string errr = string.Empty;

        public long ID;

        public long manid;

        public long pid;

        public DateTime Tmonth;

        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        private string errorm = string.Empty;

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult apnote1()
        {
            var aplistf = this.db.approvals.ToList();
            var aplists = new List<approval>();
            foreach (var ap in aplistf)
                if (!aplists.Exists(
                    x => x.adate == ap.adate && x.MPS_id == ap.MPS_id && x.P_id == ap.P_id
                         && x.status == "submitted"))
                    if (ap.status == "submitted")
                        aplists.Add(ap);

            return this.PartialView(aplists);
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

        [Authorize(Roles = "Employee,Admin")]
        public ActionResult AIndex(MainTimeSheet ids)
        {
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID).ToList();
            this.TempData["mcreateid"] = ids;
            ViewBag.ids = ids;
            var aa = a.Find(x => x.ID == ids.ID);
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToLongDateString();
            this.ViewBag.mdate1 = aa.TMonth.ToString("MM/dd/yyyy");

            var d = from LabourMaster in this.db.LabourMasters
                where LabourMaster.ManPowerSupply == b.ID
                select LabourMaster;
            this.ViewBag.EmpID = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.pos = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Position");
            this.ViewBag.name = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Person_name");
            var atlist = this.db.Attendances.ToList();
            var dateat = aa.TMonth;
            if (dateat.Day == 1) this.fillformpremon(aa.ID);
            else
            {
                filldate(aa.ID);
            }
            {
                var data = new[]
                {
                    new SelectListItem {Text = "0", Value = "0"},
                    new SelectListItem {Text = "1", Value = "1"},
                    new SelectListItem {Text = "2", Value = "2"},
                    new SelectListItem {Text = "3", Value = "3"},
                    new SelectListItem {Text = "4", Value = "4"},
                    new SelectListItem {Text = "5", Value = "5"},
                    new SelectListItem {Text = "6", Value = "6"},
                    new SelectListItem {Text = "7", Value = "7"},
                    new SelectListItem {Text = "8", Value = "8"},
                    new SelectListItem {Text = "9", Value = "9"},
                    new SelectListItem {Text = "10", Value = "10"},
                    new SelectListItem {Text = "11", Value = "11"},
                    new SelectListItem {Text = "12", Value = "12"},
                    new SelectListItem {Text = "13", Value = "13"},
                    new SelectListItem {Text = "14", Value = "14"},
                    new SelectListItem {Text = "15", Value = "15"},
                    new SelectListItem {Text = "16", Value = "16"},
                    new SelectListItem {Text = "17", Value = "17"},
                    new SelectListItem {Text = "18", Value = "18"},
                    new SelectListItem {Text = "19", Value = "19"},
                    new SelectListItem {Text = "20", Value = "20"},
                    new SelectListItem {Text = "21", Value = "21"},
                    new SelectListItem {Text = "22", Value = "22"},
                    new SelectListItem {Text = "23", Value = "23"},
                    new SelectListItem {Text = "24", Value = "24"},
                    new SelectListItem {Text = "S", Value = "S"},
                    new SelectListItem {Text = "A", Value = "A"},
                    new SelectListItem {Text = "T", Value = "T"},
                    new SelectListItem {Text = "V", Value = "V"}
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
            }

            var atlist1 = this.db.Attendances.Where(x => x.SubMain.Equals(aa.ID)).Include(x => x.LabourMaster)
                .OrderByDescending(m => m.ID).ToList();
            var as1 = this.db.Attendances.Where(x => x.SubMain.Equals(aa.ID)).Include(x => x.LabourMaster)
                .OrderByDescending(m => m.EmpID);
            var listat = new List<Attendance>();
            foreach (var VA in as1.OrderBy(x => x.ID))
            {
                if (!listat.Exists(
                    x => x.MainTimeSheet.ProjectList.PROJECT_NAME == VA.MainTimeSheet.ProjectList.PROJECT_NAME
                         && x.EmpID == VA.EmpID))
                {
                    listat.Add(VA);
                }
            }

            var model1 = new timesheetViewModel
            {
                Attendancecollection = listat
            };

            return this.View(model1);
        }

        [HttpPost]
        [Authorize(Roles = "Employee,Admin")]
        [MultipleButton(Name = "action", Argument = "AIndex1")]
        public ActionResult AIndex1(long? empno)
        {
            var ids = this.TempData["mcreateid"] as MainTimeSheet;
            ViewBag.ids = ids;
            var a = this.db.MainTimeSheets.Where(x => x.ID == ids.ID).OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToLongDateString();
            this.ViewBag.mdate1 = aa.TMonth;

            var d = from LabourMaster in this.db.LabourMasters
                where LabourMaster.ManPowerSupply == b.ID
                select LabourMaster;
            this.ViewBag.EmpID = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO", empno);
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO", empno);
            this.ViewBag.pos = new SelectList(
                d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO),
                "ID",
                "Position",
                empno);
            this.ViewBag.name = new SelectList(
                d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO),
                "ID",
                "Person_name",
                empno);

            var data = new[]
            {
                new SelectListItem {Text = "0", Value = "0"},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"},
                new SelectListItem {Text = "17", Value = "17"},
                new SelectListItem {Text = "18", Value = "18"},
                new SelectListItem {Text = "19", Value = "19"},
                new SelectListItem {Text = "20", Value = "20"},
                new SelectListItem {Text = "21", Value = "21"},
                new SelectListItem {Text = "22", Value = "22"},
                new SelectListItem {Text = "23", Value = "23"},
                new SelectListItem {Text = "24", Value = "24"},
                new SelectListItem {Text = "S", Value = "S"},
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "T", Value = "T"},
                new SelectListItem {Text = "V", Value = "V"}
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

            var model1 = new timesheetViewModel();
            if (empno != null)
            {
                var testi = this.db.Attendances.Where(x => x.SubMain.Equals(aa.ID) && x.EmpID == empno)
                    .Include(x => x.LabourMaster).OrderByDescending(m => m.ID);
                if (testi != null)
                    model1 = new timesheetViewModel {Attendancecollection = testi.ToList()};
            }
            else
            {
                model1 = new timesheetViewModel
                {
                    Attendancecollection = this.db.Attendances.Where(x => x.SubMain.Equals(aa.ID))
                        .Include(x => x.LabourMaster).OrderByDescending(m => m.ID)
                };
            }

            return this.View(model1);
        }

        public List<int> GetAll(DateTime date)
        {
            var month = date.Month;
            var lastDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var array = new List<int>(); // dd/mm/yy
            var count = -1;
            for (var i = 1; i <= lastDay; i++)
            {
                var temp = new DateTime(date.Year, month, i);
                var day = temp.DayOfWeek;
                if (day == DayOfWeek.Friday)
                {
                    count++;
                    var dd = temp.Day;
                    array.Add(dd);
                }
            }

            return array;
        }

        public List<int> GetAllholi(DateTime date)
        {
            var holilist = this.db.Holidays
                .Where(x => x.Date.Value.Month == date.Month && x.Date.Value.Year == date.Year).ToList();
            var array = new List<int>();
            foreach (var ho in holilist) array.Add(ho.Date.Value.Day);

            return array;
        }

        public void filldate(long mid)
        {
            var mainlit = this.db.MainTimeSheets.OrderByDescending(x => x.ID).ToList();
            var qw = mainlit.Find(x => x.ID == mid);
            var at = this.db.Attendances.ToList();
            var atp = at.FindAll(x => x.SubMain == qw.ID);
            var b = this.db.ManPowerSuppliers.Find(qw.ManPowerSupplier);
            var fday = this.GetAll(qw.TMonth);
            {
                foreach (var sd in atp)
                {
                    var aq = GetAll(sd.MainTimeSheet.TMonth);
                    var sy = sd.MainTimeSheet.TMonth.Day;
                    if (sy != 1)
                    {
                        sy -= 1;
                        if (aq.Exists(x => x.Equals(sy)))
                        {
                            sy -= 1;
                            if (sy == 1)
                            {
                                if (sd.C3 == sd.C1 || sd.C3 == "0")
                                {
                                    sd.C3 = sd.C1;
                                }
                                
                            }
                            if (sy == 2)
                            {
                                if (sd.C4 == sd.C2 || sd.C4 == "0")
                                {
                                    sd.C4 = sd.C2;
                                }
                                
                            }
                            if (sy == 3)
                            {
                                if (sd.C5 == sd.C3 || sd.C5 == "0")
                                {
                                    sd.C5 = sd.C3;
                                }
                                
                            }
    
                            if (sy == 4)
                            {
                                if (sd.C6 == sd.C4 || sd.C6 == "0")
                                {
                                    sd.C6 = sd.C4;
                                }
                                
                            }
    
                            if (sy == 5)
                            {
                                if (sd.C7 == sd.C5 || sd.C5 == "0")
                                {
                                    sd.C7 = sd.C5;
                                }
                                
                            }
    
                            if (sy == 6)
                            {
                                if (sd.C31 == sd.C29 || sd.C6 == "0")
                                {
                                    sd.C6 = sd.C6;
                                }
                                
                            }
    
                            if (sy == 7)
                            {
                                if (sd.C9 == sd.C7 || sd.C9 == "0")
                                {
                                    sd.C9 = sd.C7;
                                }
                            }
    
                            if (sy == 8)
                            {
                                if (sd.C10 == sd.C8 || sd.C10 == "0")
                                {
                                    sd.C10 = sd.C8;
                                }
                            }
    
                            if (sy == 9)
                            {
                                if (sd.C11 == sd.C9 || sd.C11 == "0")
                                {
                                    sd.C11 = sd.C9;
                                }
                                
                            }
    
                            if (sy == 10)
                            {
                                if (sd.C12 == sd.C10 || sd.C12 == "0")
                                {
                                    sd.C12 = sd.C10;
                                }
                                
                            }
    
                            if (sy == 11)
                            {
                                if (sd.C13 == sd.C11 || sd.C31 == "0")
                                {
                                    sd.C13 = sd.C11;
                                }
                                
                            }
    
                            if (sy == 12)
                            {
                                if (sd.C14 == sd.C12 || sd.C14 == "0")
                                {
                                    sd.C14 = sd.C12;
                                }
                            }
    
                            if (sy == 13)
                            {
                                if (sd.C15 == sd.C13 || sd.C15 == "0")
                                {
                                    sd.C15 = sd.C13;
                                }
                            }
    
                            if (sy == 14)
                            {
                                if (sd.C16 == sd.C14 || sd.C16 == "0")
                                {
                                    sd.C16 = sd.C14;
                                }
                                
                            }
    
                            if (sy == 15)
                            {
                                if (sd.C17 == sd.C15 || sd.C17 == "0")
                                {
                                    sd.C17 = sd.C15;
                                }
                                ;
                            }
    
                            if (sy == 16)
                            {
                                if (sd.C18 == sd.C16 || sd.C18 == "0")
                                {
                                    sd.C18 = sd.C16;
                                }
                                
                            }
    
                            if (sy == 17)
                            {
                                if (sd.C19 == sd.C17 || sd.C19 == "0")
                                {
                                    sd.C19 = sd.C17;
                                }
                                
                            }
    
                            if (sy == 18)
                            {
                                if (sd.C20 == sd.C18 || sd.C20 == "0")
                                {
                                    sd.C20 = sd.C18;
                                }
                                ;
                            }
    
                            if (sy == 19)
                            {
                                if (sd.C21 == sd.C19 || sd.C21 == "0")
                                {
                                    sd.C21 = sd.C19;
                                }
                                
                            }
    
                            if (sy == 20)
                            {
                                if (sd.C22 == sd.C20 || sd.C22 == "0")
                                {
                                    sd.C22 = sd.C20;
                                }
                            }
    
                            if (sy == 21)
                            {
                                if (sd.C23 == sd.C21 || sd.C23 == "0")
                                {
                                    sd.C23 = sd.C21;
                                }
                                ;
                            }
    
                            if (sy == 22)
                            {
                                if (sd.C24 == sd.C22 || sd.C24 == "0")
                                {
                                    sd.C24 = sd.C22;
                                }
                                ;
                            }
    
                            if (sy == 23)
                            {
                                if (sd.C25 == sd.C23 || sd.C25 == "0")
                                {
                                    sd.C25 = sd.C23;
                                }
                                ;
                            }
    
                            if (sy == 24)
                            {
                                if (sd.C26 == sd.C24 || sd.C26 == "0")
                                {
                                    sd.C26 = sd.C24;
                                }
                                ;
                            }
    
                            if (sy == 25)
                            {
                                if (sd.C27 == sd.C25 || sd.C27 == "0")
                                {
                                    sd.C27 = sd.C25;
                                }
                            }
    
                            if (sy == 26)
                            {
                                if (sd.C28 == sd.C26 || sd.C28 == "0")
                                {
                                    sd.C28 = sd.C26;
                                }
                            }
    
                            if (sy == 27)
                            {
                                if (sd.C29 == sd.C27 || sd.C29 == "0")
                                {
                                    sd.C29 = sd.C27;
                                }
                            }
    
                            if (sy == 28)
                            {
                                if (sd.C31 == sd.C29 || sd.C30 == "0")
                                {
                                    sd.C30 = sd.C28;
                                }
                            }
    
                            if (sy == 29)
                            {
                                if (sd.C31 == sd.C29 || sd.C31 == "0")
                                {
                                    sd.C31 = sd.C29;
                                }
                                
                            }
                            this.db.Entry(sd).State = EntityState.Modified;
                            this.db.SaveChanges();
                            sd.TotalHours = 0;
                            long.TryParse(sd.C1, out var tal);
                            long.TryParse(sd.C2, out var tal1);
                            long.TryParse(sd.C3, out var tal2);
                            long.TryParse(sd.C4, out var tal3);
                            long.TryParse(sd.C5, out var tal4);
                            long.TryParse(sd.C6, out var tal5);
                            long.TryParse(sd.C7, out var tal6);
                            long.TryParse(sd.C8, out var tal7);
                            long.TryParse(sd.C9, out var tal8);
                            long.TryParse(sd.C10, out var tal9);
                            long.TryParse(sd.C11, out var tal10);
                            long.TryParse(sd.C12, out var tal11);
                            long.TryParse(sd.C13, out var tal12);
                            long.TryParse(sd.C14, out var tal13);
                            long.TryParse(sd.C15, out var tal14);
                            long.TryParse(sd.C16, out var tal15);
                            long.TryParse(sd.C17, out var tal16);
                            long.TryParse(sd.C18, out var tal17);
                            long.TryParse(sd.C19, out var tal18);
                            long.TryParse(sd.C20, out var tal19);
                            long.TryParse(sd.C21, out var tal20);
                            long.TryParse(sd.C22, out var tal21);
                            long.TryParse(sd.C23, out var tal22);
                            long.TryParse(sd.C24, out var tal23);
                            long.TryParse(sd.C25, out var tal24);
                            long.TryParse(sd.C26, out var tal25);
                            long.TryParse(sd.C27, out var tal26);
                            long.TryParse(sd.C28, out var tal27);
                            long.TryParse(sd.C29, out var tal28);
                            long.TryParse(sd.C30, out var tal29);
                            long.TryParse(sd.C31, out var tal30);
                            sd.TotalHours = tal + tal1 + tal2 + tal3 + tal4 + tal5 + tal6 + tal7 + tal8 + tal9 + tal10 + tal11 + tal12
                                            + tal13 + tal14 + tal15 + tal16 + tal17 + tal18 + tal19 + tal20 + tal21 + tal22 + tal23
                                            + tal24 + tal25 + tal26 + tal27 + tal28 + tal29 + tal30;
                            double.TryParse(b.NormalTimeUpto.ToString(), out var tho);
                            {
                                var t = new List<long>();
                                sd.TotalOverTime = 0;
                                t.Add(tal);
                                t.Add(tal1);
                                t.Add(tal2);
                                t.Add(tal3);
                                t.Add(tal4);
                                t.Add(tal5);
                                t.Add(tal6);
                                t.Add(tal7);
                                t.Add(tal8);
                                t.Add(tal9);
                                t.Add(tal10);
                                t.Add(tal11);
                                t.Add(tal12);
                                t.Add(tal13);
                                t.Add(tal14);
                                t.Add(tal15);
                                t.Add(tal16);
                                t.Add(tal17);
                                t.Add(tal18);
                                t.Add(tal19);
                                t.Add(tal20);
                                t.Add(tal21);
                                t.Add(tal22);
                                t.Add(tal23);
                                t.Add(tal24);
                                t.Add(tal25);
                                t.Add(tal26);
                                t.Add(tal27);
                                t.Add(tal28);
                                t.Add(tal29);
                                t.Add(tal30);
                                long tho1 = 0;
                                    int i = 0;
                                    foreach (var l in t)
                                    {
                                        i++;
                                        if (!fday.Exists(x=>x.Equals(i)))
                                        {
                                            if (l > tho)
                                            {
                                                tho1 += l - (long)tho;
                                                sd.TotalOverTime = tho1;
                                            }
                                        }
                                        
                                    }
                            }
                        {
                            sd.TotalSickLeave = 0;
                            long ts = 0;
                            if (!sd.C1.IsNullOrWhiteSpace())
                                if (sd.C1.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C2.IsNullOrWhiteSpace())
                                if (sd.C2.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C3.IsNullOrWhiteSpace())
                                if (sd.C3.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C4.IsNullOrWhiteSpace())
                                if (sd.C4.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C5.IsNullOrWhiteSpace())
                                if (sd.C5.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C6.IsNullOrWhiteSpace())
                                if (sd.C6.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C7.IsNullOrWhiteSpace())
                                if (sd.C7.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C8.IsNullOrWhiteSpace())
                                if (sd.C8.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C9.IsNullOrWhiteSpace())
                                if (sd.C9.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C10.IsNullOrWhiteSpace())
                                if (sd.C10.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C11.IsNullOrWhiteSpace())
                                if (sd.C11.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C12.IsNullOrWhiteSpace())
                                if (sd.C12.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C13.IsNullOrWhiteSpace())
                                if (sd.C13.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C14.IsNullOrWhiteSpace())
                                if (sd.C14.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C15.IsNullOrWhiteSpace())
                                if (sd.C15.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C16.IsNullOrWhiteSpace())
                                if (sd.C16.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C17.IsNullOrWhiteSpace())
                                if (sd.C17.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C18.IsNullOrWhiteSpace())
                                if (sd.C18.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C19.IsNullOrWhiteSpace())
                                if (sd.C19.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C20.IsNullOrWhiteSpace())
                                if (sd.C20.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C21.IsNullOrWhiteSpace())
                                if (sd.C21.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C22.IsNullOrWhiteSpace())
                                if (sd.C22.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C23.IsNullOrWhiteSpace())
                                if (sd.C23.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C24.IsNullOrWhiteSpace())
                                if (sd.C24.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C25.IsNullOrWhiteSpace())
                                if (sd.C25.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C26.IsNullOrWhiteSpace())
                                if (sd.C26.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C27.IsNullOrWhiteSpace())
                                if (sd.C27.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C28.IsNullOrWhiteSpace())
                                if (sd.C28.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C29.IsNullOrWhiteSpace())
                                if (sd.C29.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C30.IsNullOrWhiteSpace())
                                if (sd.C30.Equals("S"))
                                    ts = ts + 1;
                            if (!sd.C31.IsNullOrWhiteSpace())
                                if (sd.C31.Equals("S"))
                                    ts = ts + 1;
    
                            sd.TotalSickLeave = ts;
                        }
                        {
                            sd.TotalVL = 0;
                            long tv = 0;
                            if (!sd.C1.IsNullOrWhiteSpace())
                                if (sd.C1.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C2.IsNullOrWhiteSpace())
                                if (sd.C2.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C3.IsNullOrWhiteSpace())
                                if (sd.C3.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C4.IsNullOrWhiteSpace())
                                if (sd.C4.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C5.IsNullOrWhiteSpace())
                                if (sd.C5.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C6.IsNullOrWhiteSpace())
                                if (sd.C6.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C7.IsNullOrWhiteSpace())
                                if (sd.C7.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C8.IsNullOrWhiteSpace())
                                if (sd.C8.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C9.IsNullOrWhiteSpace())
                                if (sd.C9.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C10.IsNullOrWhiteSpace())
                                if (sd.C10.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C11.IsNullOrWhiteSpace())
                                if (sd.C11.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C12.IsNullOrWhiteSpace())
                                if (sd.C12.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C13.IsNullOrWhiteSpace())
                                if (sd.C13.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C14.IsNullOrWhiteSpace())
                                if (sd.C14.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C15.IsNullOrWhiteSpace())
                                if (sd.C15.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C16.IsNullOrWhiteSpace())
                                if (sd.C16.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C17.IsNullOrWhiteSpace())
                                if (sd.C17.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C18.IsNullOrWhiteSpace())
                                if (sd.C18.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C19.IsNullOrWhiteSpace())
                                if (sd.C19.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C20.IsNullOrWhiteSpace())
                                if (sd.C20.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C21.IsNullOrWhiteSpace())
                                if (sd.C21.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C22.IsNullOrWhiteSpace())
                                if (sd.C22.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C23.IsNullOrWhiteSpace())
                                if (sd.C23.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C24.IsNullOrWhiteSpace())
                                if (sd.C24.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C25.IsNullOrWhiteSpace())
                                if (sd.C25.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C26.IsNullOrWhiteSpace())
                                if (sd.C26.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C27.IsNullOrWhiteSpace())
                                if (sd.C27.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C28.IsNullOrWhiteSpace())
                                if (sd.C28.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C29.IsNullOrWhiteSpace())
                                if (sd.C29.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C30.IsNullOrWhiteSpace())
                                if (sd.C30.Equals("V"))
                                    tv = tv + 1;
                            if (!sd.C31.IsNullOrWhiteSpace())
                                if (sd.C31.Equals("V"))
                                    tv = tv + 1;
    
                            sd.TotalVL = tv;
                        }
                        {
                            sd.TotalAbsent = 0;
                            long tv = 0;
                            if (!sd.C1.IsNullOrWhiteSpace())
                                if (sd.C1.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C2.IsNullOrWhiteSpace())
                                if (sd.C2.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C3.IsNullOrWhiteSpace())
                                if (sd.C3.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C4.IsNullOrWhiteSpace())
                                if (sd.C4.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C5.IsNullOrWhiteSpace())
                                if (sd.C5.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C6.IsNullOrWhiteSpace())
                                if (sd.C6.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C7.IsNullOrWhiteSpace())
                                if (sd.C7.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C8.IsNullOrWhiteSpace())
                                if (sd.C8.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C9.IsNullOrWhiteSpace())
                                if (sd.C9.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C10.IsNullOrWhiteSpace())
                                if (sd.C10.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C11.IsNullOrWhiteSpace())
                                if (sd.C11.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C12.IsNullOrWhiteSpace())
                                if (sd.C12.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C13.IsNullOrWhiteSpace())
                                if (sd.C13.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C14.IsNullOrWhiteSpace())
                                if (sd.C14.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C15.IsNullOrWhiteSpace())
                                if (sd.C15.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C16.IsNullOrWhiteSpace())
                                if (sd.C16.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C17.IsNullOrWhiteSpace())
                                if (sd.C17.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C18.IsNullOrWhiteSpace())
                                if (sd.C18.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C19.IsNullOrWhiteSpace())
                                if (sd.C19.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C20.IsNullOrWhiteSpace())
                                if (sd.C20.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C21.IsNullOrWhiteSpace())
                                if (sd.C21.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C22.IsNullOrWhiteSpace())
                                if (sd.C22.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C23.IsNullOrWhiteSpace())
                                if (sd.C23.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C24.IsNullOrWhiteSpace())
                                if (sd.C24.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C25.IsNullOrWhiteSpace())
                                if (sd.C25.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C26.IsNullOrWhiteSpace())
                                if (sd.C26.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C27.IsNullOrWhiteSpace())
                                if (sd.C27.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C28.IsNullOrWhiteSpace())
                                if (sd.C28.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C29.IsNullOrWhiteSpace())
                                if (sd.C29.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C30.IsNullOrWhiteSpace())
                                if (sd.C30.Equals("A"))
                                    tv = tv + 1;
                            if (!sd.C31.IsNullOrWhiteSpace())
                                if (sd.C31.Equals("A"))
                                    tv = tv + 1;
    
                            sd.TotalAbsent = tv;
                        }
                        {
                            sd.TotalTransefer = 0;
                            long tv = 0;
                            if (!sd.C1.IsNullOrWhiteSpace())
                                if (sd.C1.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C2.IsNullOrWhiteSpace())
                                if (sd.C2.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C3.IsNullOrWhiteSpace())
                                if (sd.C3.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C4.IsNullOrWhiteSpace())
                                if (sd.C4.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C5.IsNullOrWhiteSpace())
                                if (sd.C5.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C6.IsNullOrWhiteSpace())
                                if (sd.C6.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C7.IsNullOrWhiteSpace())
                                if (sd.C7.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C8.IsNullOrWhiteSpace())
                                if (sd.C8.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C9.IsNullOrWhiteSpace())
                                if (sd.C9.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C10.IsNullOrWhiteSpace())
                                if (sd.C10.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C11.IsNullOrWhiteSpace())
                                if (sd.C11.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C12.IsNullOrWhiteSpace())
                                if (sd.C12.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C13.IsNullOrWhiteSpace())
                                if (sd.C13.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C14.IsNullOrWhiteSpace())
                                if (sd.C14.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C15.IsNullOrWhiteSpace())
                                if (sd.C15.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C16.IsNullOrWhiteSpace())
                                if (sd.C16.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C17.IsNullOrWhiteSpace())
                                if (sd.C17.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C18.IsNullOrWhiteSpace())
                                if (sd.C18.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C19.IsNullOrWhiteSpace())
                                if (sd.C19.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C20.IsNullOrWhiteSpace())
                                if (sd.C20.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C21.IsNullOrWhiteSpace())
                                if (sd.C21.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C22.IsNullOrWhiteSpace())
                                if (sd.C22.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C23.IsNullOrWhiteSpace())
                                if (sd.C23.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C24.IsNullOrWhiteSpace())
                                if (sd.C24.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C25.IsNullOrWhiteSpace())
                                if (sd.C25.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C26.IsNullOrWhiteSpace())
                                if (sd.C26.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C27.IsNullOrWhiteSpace())
                                if (sd.C27.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C28.IsNullOrWhiteSpace())
                                if (sd.C28.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C29.IsNullOrWhiteSpace())
                                if (sd.C29.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C30.IsNullOrWhiteSpace())
                                if (sd.C30.Equals("T"))
                                    tv = tv + 1;
                            if (!sd.C31.IsNullOrWhiteSpace())
                                if (sd.C31.Equals("T"))
                                    tv = tv + 1;
    
                            sd.TotalTransefer = tv;
                        }
                        sd.status = "panding";
                        this.db.Entry(sd).State = EntityState.Modified;
                            this.db.SaveChanges();
                            goto fi;
                        }
                        if (aq.Exists(x => x.Equals(sy + 1)))
                        {
                            goto fi;
                        }
                        if (sy == 1)
                        {
                            if (sd.C2 == sd.C1 || sd.C2 == "0" )
                            {
                                sd.C2 = sd.C1;
                            }
    
                        }
    
                        if (sy == 2)
                        {
                            if (sd.C3 == sd.C2 || sd.C3 == "0")
                            {
                                sd.C3 = sd.C2;
                            }
    
                        }
    
                        if (sy == 3)
                        {
                            if (sd.C4 == sd.C3 || sd.C4 == "0")
                            {
                                sd.C4 = sd.C3;
                            }
    
                        }
    
                        if (sy == 4)
                        {
                            if (sd.C5 == sd.C4 || sd.C5 == "0")
                            {
                                sd.C5 = sd.C4;
                            }
    
                        }
    
                        if (sy == 5)
                        {
                            if (sd.C6 == sd.C5 || sd.C6 == "0")
                            {
                                sd.C6 = sd.C5;
                            }
    
                        }
    
                        if (sy == 6)
                        {
                            if (sd.C7 == sd.C6 || sd.C7 == "0")
                            {
                                sd.C7 = sd.C6;
                            }
    
                        }
    
                        if (sy == 7)
                        {
                            if (sd.C8 == sd.C7 || sd.C8 == "0")
                            {
                                sd.C8 = sd.C7;
                            }
    
                        }
    
                        if (sy == 8)
                        {
                            if (sd.C9 == sd.C8 || sd.C9 == "0")
                            {
                                sd.C9 = sd.C8;
                            }
    
                        }
    
                        if (sy == 9)
                        {
                            if (sd.C10 == sd.C9 || sd.C10 == "0")
                            {
                                sd.C10 = sd.C9;
                            }
    
                        }
    
                        if (sy == 10)
                        {
                            if (sd.C11 == sd.C10 || sd.C11 == "0")
                            {
                                sd.C11 = sd.C10;
                            }
    
                        }
    
                        if (sy == 11)
                        {
                            if (sd.C12 == sd.C11 || sd.C12 == "0")
                            {
                                sd.C12 = sd.C11;
                            }
    
                        }
    
                        if (sy == 12)
                        {
                            if (sd.C13 == sd.C12 || sd.C13 == "0")
                            {
                                sd.C13 = sd.C12;
                            }
    
                        }
    
                        if (sy == 13)
                        {
                            if (sd.C14 == sd.C13 || sd.C14 == "0")
                            {
                                sd.C14 = sd.C13;
                            }
    
                        }
    
                        if (sy == 14)
                        {
                            if (sd.C15 == sd.C14 || sd.C15 == "0")
                            {
                                sd.C15 = sd.C14;
                            }
    
                        }
    
                        if (sy == 15)
                        {
                            if (sd.C16 == sd.C15 || sd.C16 == "0")
                            {
                                sd.C16 = sd.C15;
                            }
    
                        }
    
                        if (sy == 16)
                        {
                            if (sd.C17 == sd.C16 || sd.C17 == "0")
                            {
                                sd.C17 = sd.C16;
                            }
    
                        }
    
                        if (sy == 17)
                        {
                            if (sd.C18 == sd.C17 || sd.C18 == "0")
                            {
                                sd.C18 = sd.C17;
                            }
    
                        }
    
                        if (sy == 18)
                        {
                            if (sd.C19 == sd.C18 || sd.C19 == "0")
                            {
                                sd.C19 = sd.C18;
                            }
    
                        }
    
                        if (sy == 19)
                        {
                            if (sd.C20 == sd.C19 || sd.C20 == "0")
                            {
                                sd.C20 = sd.C19;
                            }
                        }
    
                        if (sy == 20)
                        {
                            if (sd.C21 == sd.C20 || sd.C21 == "0")
                            {
                                sd.C21 = sd.C20;
                            }
    
                        }
    
                        if (sy == 21)
                        {
                            if (sd.C22 == sd.C21 || sd.C22 == "0")
                            {
                                sd.C22 = sd.C21;
                            }
    
                        }
    
                        if (sy == 22)
                        {
                            if (sd.C23 == sd.C22 || sd.C23 == "0")
                            {
                                sd.C23 = sd.C22;
                            }
    
                        }
    
                        if (sy == 23)
                        {
                            if (sd.C24 == sd.C23 || sd.C24 == "0")
                            {
                                sd.C24 = sd.C23;
                            }
    
                        }
    
                        if (sy == 24)
                        {
                            if (sd.C25 == sd.C24 || sd.C25 == "0")
                            {
                                sd.C25 = sd.C24;
                            }
    
                        }
    
                        if (sy == 25)
                        {
                            if (sd.C26 == sd.C25 || sd.C26 == "0")
                            {
                                sd.C26 = sd.C25;
                            }
                        }
    
                        if (sy == 26)
                        {
                            if (sd.C27 == sd.C26 || sd.C27 == "0")
                            {
                                sd.C27 = sd.C26;
    
                            }
                        }
    
                        if (sy == 27)
                        {
                            if (sd.C28 == sd.C27 || sd.C28 == "0")
                            {
                                sd.C28 = sd.C27;
                            }
    
                        }
    
                        if (sy == 28)
                        {
                            if (sd.C29 == sd.C28 || sd.C29 == "0")
                            {
                                sd.C29 = sd.C28;
    
                            }
                        }
    
                        if (sy == 29)
                        {
                            if (sd.C30 == sd.C29 || sd.C30 == "0")
                            {
                                sd.C30 = sd.C29;
                            }
    
                        }
    
                        if (sy == 30)
                        {
                            if (sd.C31 == sd.C30 || sd.C31 == "0")
                            {
                                sd.C31 = sd.C30;
                            }
    
                        }
    
    
                        this.db.Entry(sd).State = EntityState.Modified;
                        this.db.SaveChanges();
                        sd.TotalHours = 0;
                        {
                            long.TryParse(sd.C1, out var tl);
                            long.TryParse(sd.C2, out var tl1);
                            long.TryParse(sd.C3, out var tl2);
                            long.TryParse(sd.C4, out var tl3);
                            long.TryParse(sd.C5, out var tl4);
                            long.TryParse(sd.C6, out var tl5);
                            long.TryParse(sd.C7, out var tl6);
                            long.TryParse(sd.C8, out var tl7);
                            long.TryParse(sd.C9, out var tl8);
                            long.TryParse(sd.C10, out var tl9);
                            long.TryParse(sd.C11, out var tl10);
                            long.TryParse(sd.C12, out var tl11);
                            long.TryParse(sd.C13, out var tl12);
                            long.TryParse(sd.C14, out var tl13);
                            long.TryParse(sd.C15, out var tl14);
                            long.TryParse(sd.C16, out var tl15);
                            long.TryParse(sd.C17, out var tl16);
                            long.TryParse(sd.C18, out var tl17);
                            long.TryParse(sd.C19, out var tl18);
                            long.TryParse(sd.C20, out var tl19);
                            long.TryParse(sd.C21, out var tl20);
                            long.TryParse(sd.C22, out var tl21);
                            long.TryParse(sd.C23, out var tl22);
                            long.TryParse(sd.C24, out var tl23);
                            long.TryParse(sd.C25, out var tl24);
                            long.TryParse(sd.C26, out var tl25);
                            long.TryParse(sd.C27, out var tl26);
                            long.TryParse(sd.C28, out var tl27);
                            long.TryParse(sd.C29, out var tl28);
                            long.TryParse(sd.C30, out var tl29);
                            long.TryParse(sd.C31, out var tl30);
                            long.TryParse(sd.TotalHours.ToString(), out var sdlg);
                            sd.TotalHours = tl + tl1 + tl2 + tl3 + tl4 + tl5 + tl6 + tl7 + tl8 + tl9 + tl10 + tl11 + tl12 + tl13
                                            + tl14 + tl15 + tl16 + tl17 + tl18 + tl19 + tl20 + tl21 + tl22 + tl23 + tl24 + tl25
                                            + tl26 + tl27 + tl28 + tl29 + tl30;
                            double.TryParse(b.NormalTimeUpto.ToString(), out var tho);
                            {
                                var t = new List<long>();
                                sd.TotalOverTime = 0;
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
                                        sd.TotalOverTime = tho1;
                                    }
                            }
                            {
                                sd.TotalSickLeave = 0;
                                long ts = 0;
                                if (!sd.C1.IsNullOrWhiteSpace())
                                    if (sd.C1.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C2.IsNullOrWhiteSpace())
                                    if (sd.C2.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C3.IsNullOrWhiteSpace())
                                    if (sd.C3.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C4.IsNullOrWhiteSpace())
                                    if (sd.C4.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C5.IsNullOrWhiteSpace())
                                    if (sd.C5.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C6.IsNullOrWhiteSpace())
                                    if (sd.C6.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C7.IsNullOrWhiteSpace())
                                    if (sd.C7.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C8.IsNullOrWhiteSpace())
                                    if (sd.C8.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C9.IsNullOrWhiteSpace())
                                    if (sd.C9.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C10.IsNullOrWhiteSpace())
                                    if (sd.C10.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C11.IsNullOrWhiteSpace())
                                    if (sd.C11.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C12.IsNullOrWhiteSpace())
                                    if (sd.C12.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C13.IsNullOrWhiteSpace())
                                    if (sd.C13.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C14.IsNullOrWhiteSpace())
                                    if (sd.C14.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C15.IsNullOrWhiteSpace())
                                    if (sd.C15.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C16.IsNullOrWhiteSpace())
                                    if (sd.C16.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C17.IsNullOrWhiteSpace())
                                    if (sd.C17.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C18.IsNullOrWhiteSpace())
                                    if (sd.C18.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C19.IsNullOrWhiteSpace())
                                    if (sd.C19.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C20.IsNullOrWhiteSpace())
                                    if (sd.C20.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C21.IsNullOrWhiteSpace())
                                    if (sd.C21.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C22.IsNullOrWhiteSpace())
                                    if (sd.C22.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C23.IsNullOrWhiteSpace())
                                    if (sd.C23.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C24.IsNullOrWhiteSpace())
                                    if (sd.C24.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C25.IsNullOrWhiteSpace())
                                    if (sd.C25.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C26.IsNullOrWhiteSpace())
                                    if (sd.C26.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C27.IsNullOrWhiteSpace())
                                    if (sd.C27.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C28.IsNullOrWhiteSpace())
                                    if (sd.C28.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C29.IsNullOrWhiteSpace())
                                    if (sd.C29.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C30.IsNullOrWhiteSpace())
                                    if (sd.C30.Equals("S"))
                                        ts = ts + 1;
                                if (!sd.C31.IsNullOrWhiteSpace())
                                    if (sd.C31.Equals("S"))
                                        ts = ts + 1;
    
                                sd.TotalSickLeave = ts;
                            }
                            {
                                sd.TotalVL = 0;
                                long tv = 0;
                                if (!sd.C1.IsNullOrWhiteSpace())
                                    if (sd.C1.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C2.IsNullOrWhiteSpace())
                                    if (sd.C2.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C3.IsNullOrWhiteSpace())
                                    if (sd.C3.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C4.IsNullOrWhiteSpace())
                                    if (sd.C4.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C5.IsNullOrWhiteSpace())
                                    if (sd.C5.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C6.IsNullOrWhiteSpace())
                                    if (sd.C6.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C7.IsNullOrWhiteSpace())
                                    if (sd.C7.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C8.IsNullOrWhiteSpace())
                                    if (sd.C8.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C9.IsNullOrWhiteSpace())
                                    if (sd.C9.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C10.IsNullOrWhiteSpace())
                                    if (sd.C10.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C11.IsNullOrWhiteSpace())
                                    if (sd.C11.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C12.IsNullOrWhiteSpace())
                                    if (sd.C12.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C13.IsNullOrWhiteSpace())
                                    if (sd.C13.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C14.IsNullOrWhiteSpace())
                                    if (sd.C14.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C15.IsNullOrWhiteSpace())
                                    if (sd.C15.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C16.IsNullOrWhiteSpace())
                                    if (sd.C16.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C17.IsNullOrWhiteSpace())
                                    if (sd.C17.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C18.IsNullOrWhiteSpace())
                                    if (sd.C18.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C19.IsNullOrWhiteSpace())
                                    if (sd.C19.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C20.IsNullOrWhiteSpace())
                                    if (sd.C20.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C21.IsNullOrWhiteSpace())
                                    if (sd.C21.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C22.IsNullOrWhiteSpace())
                                    if (sd.C22.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C23.IsNullOrWhiteSpace())
                                    if (sd.C23.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C24.IsNullOrWhiteSpace())
                                    if (sd.C24.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C25.IsNullOrWhiteSpace())
                                    if (sd.C25.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C26.IsNullOrWhiteSpace())
                                    if (sd.C26.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C27.IsNullOrWhiteSpace())
                                    if (sd.C27.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C28.IsNullOrWhiteSpace())
                                    if (sd.C28.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C29.IsNullOrWhiteSpace())
                                    if (sd.C29.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C30.IsNullOrWhiteSpace())
                                    if (sd.C30.Equals("V"))
                                        tv = tv + 1;
                                if (!sd.C31.IsNullOrWhiteSpace())
                                    if (sd.C31.Equals("V"))
                                        tv = tv + 1;
    
                                sd.TotalVL = tv;
                            }
                            {
                                sd.TotalAbsent = 0;
                                long tv = 0;
                                if (!sd.C1.IsNullOrWhiteSpace())
                                    if (sd.C1.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C2.IsNullOrWhiteSpace())
                                    if (sd.C2.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C3.IsNullOrWhiteSpace())
                                    if (sd.C3.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C4.IsNullOrWhiteSpace())
                                    if (sd.C4.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C5.IsNullOrWhiteSpace())
                                    if (sd.C5.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C6.IsNullOrWhiteSpace())
                                    if (sd.C6.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C7.IsNullOrWhiteSpace())
                                    if (sd.C7.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C8.IsNullOrWhiteSpace())
                                    if (sd.C8.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C9.IsNullOrWhiteSpace())
                                    if (sd.C9.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C10.IsNullOrWhiteSpace())
                                    if (sd.C10.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C11.IsNullOrWhiteSpace())
                                    if (sd.C11.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C12.IsNullOrWhiteSpace())
                                    if (sd.C12.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C13.IsNullOrWhiteSpace())
                                    if (sd.C13.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C14.IsNullOrWhiteSpace())
                                    if (sd.C14.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C15.IsNullOrWhiteSpace())
                                    if (sd.C15.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C16.IsNullOrWhiteSpace())
                                    if (sd.C16.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C17.IsNullOrWhiteSpace())
                                    if (sd.C17.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C18.IsNullOrWhiteSpace())
                                    if (sd.C18.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C19.IsNullOrWhiteSpace())
                                    if (sd.C19.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C20.IsNullOrWhiteSpace())
                                    if (sd.C20.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C21.IsNullOrWhiteSpace())
                                    if (sd.C21.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C22.IsNullOrWhiteSpace())
                                    if (sd.C22.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C23.IsNullOrWhiteSpace())
                                    if (sd.C23.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C24.IsNullOrWhiteSpace())
                                    if (sd.C24.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C25.IsNullOrWhiteSpace())
                                    if (sd.C25.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C26.IsNullOrWhiteSpace())
                                    if (sd.C26.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C27.IsNullOrWhiteSpace())
                                    if (sd.C27.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C28.IsNullOrWhiteSpace())
                                    if (sd.C28.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C29.IsNullOrWhiteSpace())
                                    if (sd.C29.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C30.IsNullOrWhiteSpace())
                                    if (sd.C30.Equals("A"))
                                        tv = tv + 1;
                                if (!sd.C31.IsNullOrWhiteSpace())
                                    if (sd.C31.Equals("A"))
                                        tv = tv + 1;
    
                                sd.TotalAbsent = tv;
                            }
                            {
                                sd.TotalTransefer = 0;
                                long tv = 0;
                                if (!sd.C1.IsNullOrWhiteSpace())
                                    if (sd.C1.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C2.IsNullOrWhiteSpace())
                                    if (sd.C2.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C3.IsNullOrWhiteSpace())
                                    if (sd.C3.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C4.IsNullOrWhiteSpace())
                                    if (sd.C4.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C5.IsNullOrWhiteSpace())
                                    if (sd.C5.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C6.IsNullOrWhiteSpace())
                                    if (sd.C6.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C7.IsNullOrWhiteSpace())
                                    if (sd.C7.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C8.IsNullOrWhiteSpace())
                                    if (sd.C8.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C9.IsNullOrWhiteSpace())
                                    if (sd.C9.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C10.IsNullOrWhiteSpace())
                                    if (sd.C10.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C11.IsNullOrWhiteSpace())
                                    if (sd.C11.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C12.IsNullOrWhiteSpace())
                                    if (sd.C12.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C13.IsNullOrWhiteSpace())
                                    if (sd.C13.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C14.IsNullOrWhiteSpace())
                                    if (sd.C14.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C15.IsNullOrWhiteSpace())
                                    if (sd.C15.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C16.IsNullOrWhiteSpace())
                                    if (sd.C16.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C17.IsNullOrWhiteSpace())
                                    if (sd.C17.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C18.IsNullOrWhiteSpace())
                                    if (sd.C18.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C19.IsNullOrWhiteSpace())
                                    if (sd.C19.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C20.IsNullOrWhiteSpace())
                                    if (sd.C20.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C21.IsNullOrWhiteSpace())
                                    if (sd.C21.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C22.IsNullOrWhiteSpace())
                                    if (sd.C22.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C23.IsNullOrWhiteSpace())
                                    if (sd.C23.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C24.IsNullOrWhiteSpace())
                                    if (sd.C24.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C25.IsNullOrWhiteSpace())
                                    if (sd.C25.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C26.IsNullOrWhiteSpace())
                                    if (sd.C26.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C27.IsNullOrWhiteSpace())
                                    if (sd.C27.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C28.IsNullOrWhiteSpace())
                                    if (sd.C28.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C29.IsNullOrWhiteSpace())
                                    if (sd.C29.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C30.IsNullOrWhiteSpace())
                                    if (sd.C30.Equals("T"))
                                        tv = tv + 1;
                                if (!sd.C31.IsNullOrWhiteSpace())
                                    if (sd.C31.Equals("T"))
                                        tv = tv + 1;
    
                                sd.TotalTransefer = tv;
                            }
                            sd.status = "panding";
                        }
                        this.db.Entry(sd).State = EntityState.Modified;
                        this.db.SaveChanges();
                    }
    
                    
                    fi:;
    
                }
            }
        }

        public void fillformpremon(long mid)
        {
            var mainlit = this.db.MainTimeSheets.OrderByDescending(x => x.ID).ToList();
            var qw = mainlit.Find(x => x.ID == mid);
            var lm = mainlit.Find(
                x => x.TMonth.Month == qw.TMonth.Month - 1 && x.TMonth.Year == qw.TMonth.Year
                                                           && x.ManPowerSupplier == qw.ManPowerSupplier
                                                           && x.Project == qw.Project);
            var lastmainid = lm;
            var at = this.db.Attendances.ToList();
            if (lm == null) goto q1;
            if (lastmainid.TMonth.Month + 1 != 13)
            {
                var fillmonth = lastmainid.TMonth.Month + 1;
            }
            var atp = at.FindAll(x => x.SubMain == qw.ID);
            var atflist = at.FindAll(x => x.SubMain == lastmainid.ID);
            var dms = DateTime.DaysInMonth(lm.TMonth.Year, lm.TMonth.Month);
            var fday = new DateTime(lm.TMonth.Year, lm.TMonth.Month + 1, 1);
            var fdaylist = this.GetAll(fday);
            if (atp.Count != 0) goto q1;
            foreach (var at1 in atflist)
            {
                var at2 = new Attendance();
                at2.SubMain = qw.ID;
                at2.EmpID = at1.EmpID;
                if (dms == 30)
                {
                    if (fdaylist.Exists(x => x == 1))
                    {
                        at2.C2 = at1.C30;
                        long.TryParse(at2.C2, out var q1);
                        at2.TotalHours = q1;
                    }
                    else
                    {
                        at2.C1 = at1.C30;
                        long.TryParse(at2.C1, out var q1);
                        at2.TotalHours = q1;
                    }
                }

                if (dms == 31)
                {
                    if (fdaylist.Exists(x => x == 1))
                    {
                        at2.C2 = at1.C31;
                        long.TryParse(at2.C2, out var q1);
                        at2.TotalHours = q1;
                    }
                    else
                    {
                        at2.C1 = at1.C31;
                        long.TryParse(at2.C1, out var q1);
                        at2.TotalHours = q1;
                    }
                }

                if (dms == 29)
                {
                    if (fdaylist.Exists(x => x == 1))
                    {
                        at2.C2 = at1.C29;
                        long.TryParse(at2.C2, out var q1);
                        at2.TotalHours = q1;
                    }
                    else
                    {
                        at2.C1 = at1.C29;
                        long.TryParse(at2.C1, out var q1);
                        at2.TotalHours = q1;
                    }
                }

                if (dms == 28)
                {
                    if (fdaylist.Exists(x => x == 1))
                    {
                        at2.C2 = at1.C28;
                        long.TryParse(at2.C2, out var q1);
                        at2.TotalHours = q1;
                    }
                    else
                    {
                        at2.C1 = at1.C28;
                        long.TryParse(at2.C1, out var q1);
                        at2.TotalHours = q1;
                    }
                }

                this.db.Attendances.Add(at2);
                this.db.SaveChanges();
            }

            q1: ;
            var attplist = at.FindAll(x =>
                x.MainTimeSheet.Project == qw.Project && x.MainTimeSheet.ManPowerSupplier == qw.ManPowerSupplier);
            var attpfinallist = new List<Attendance>();
            var fday2 = new DateTime(qw.TMonth.Year, qw.TMonth.Month, 1);
            var fdaylist2 = this.GetAll(fday2);
            var cou = 1;
            foreach (var ap in attplist)
            {
                var attp = new Attendance();
                attp.SubMain = qw.ID;
                attp.MainTimeSheet = qw;
                attp.EmpID = ap.EmpID;
                if (fdaylist2.Exists(x => x == 1))
                {
                    attp.C2 = "8";
                    long.TryParse("8", out var q1);
                    ap.TotalHours = q1;
                }
                else
                {
                    attp.C1 = "8";
                    long.TryParse(attp.C1, out var q1);
                    attp.TotalHours = q1;
                }

                if (!attpfinallist.Exists(x=>x.EmpID == ap.EmpID))
                {
                    attpfinallist.Add(attp);
                    this.db.Attendances.Add(attp);
                    this.db.SaveChanges();
                    cou++;
                }
            }

            var co = cou;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize(Roles = "Employee,Admin")]
        [MultipleButton(Name = "action", Argument = "AIndex")]
        public ActionResult AIndex(
            [Bind(
                Include =
                    "ID,EmpID,SubMain,C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30,C31,TotalHours,TotalOverTime,TotalAbsent,AccommodationDeduction,FoodDeduction,TotalWorkingDays,TotalVL,TotalTransefer,TotalSickLeave,FridayHours,Holidays,ManPowerSupply,CompID,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,xABST,nABST,xOT,nnOT")]
            Attendance attendance)
        {
            var ap1 = this.db.approvals.ToList();
            var model1 = new timesheetViewModel {attendance = attendance};
            var check = new List<Attendance>();
            var ids = this.TempData["mcreateid"] as MainTimeSheet;
            var a = this.db.MainTimeSheets.Where(x => x.ID == ids.ID).OrderByDescending(m => m.ID);
            var aa = a.First();
            this.ViewBag.mid = aa.ID;
            var b = this.db.ManPowerSuppliers.Find(aa.ManPowerSupplier);
            var c = this.db.ProjectLists.Find(aa.Project);
            var ap = ap1.FindAll(x =>
                x.MPS_id == aa.ManPowerSupplier && x.P_id == aa.Project);
            this.ViewBag.pid = c.PROJECT_NAME;
            this.ViewBag.mps = b.Supplier;
            this.ViewBag.mpssh = b.ShortName;
            this.ViewBag.mdate = aa.TMonth.ToLongDateString();
            this.ViewBag.mdate1 = aa.TMonth;
            this.ViewBag.exist = string.Empty;

            var d = from LabourMaster in this.db.LabourMasters
                where LabourMaster.ManPowerSupply == b.ID
                select LabourMaster;
            this.ViewBag.EmpID = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.pos = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Position");
            this.ViewBag.name = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Person_name");

            // oldmts = this.db.MainTimeSheets
            // .Where(
            // x => x.TMonth.Month.Equals(list.date.Month) && x.TMonth.Year.Equals(list.date.Year)
            // && x.ManPowerSupplier.Equals(aa.ManPowerSupplier)
            // && x.Project.Equals(aa.Project))
            // .OrderByDescending(x => x.ID).ToList();
            var data = new[]
            {
                new SelectListItem {Text = "0", Value = "0"},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"},
                new SelectListItem {Text = "17", Value = "17"},
                new SelectListItem {Text = "18", Value = "18"},
                new SelectListItem {Text = "19", Value = "19"},
                new SelectListItem {Text = "20", Value = "20"},
                new SelectListItem {Text = "21", Value = "21"},
                new SelectListItem {Text = "22", Value = "22"},
                new SelectListItem {Text = "23", Value = "23"},
                new SelectListItem {Text = "24", Value = "24"},
                new SelectListItem {Text = "S", Value = "S"},
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "T", Value = "T"},
                new SelectListItem {Text = "V", Value = "V"}
            };
            {
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
            }

            if (this.ModelState.IsValid)
            {
                var oldmts = this.db.MainTimeSheets.Where(x => x.ID == attendance.SubMain).OrderByDescending(x => x.ID)
                    .ToList();
                if (oldmts.Count != 0)
                {
                    var oldmts1 = oldmts.Last();

                    // First();
                    check = this.db.Attendances
                        .Where(z => z.EmpID.Equals(attendance.EmpID) && z.SubMain.Equals(oldmts1.ID)).ToList();
                }
                else
                {
                    check = this.db.Attendances.Where(z => z.EmpID.Equals(attendance.EmpID) && z.SubMain.Equals(aa.ID))
                        .ToList();
                }

                if (check.Count != 0)
                {
                    var at = this.db.Attendances.Find(check.First().ID);
                    long fri1 = 0;
                    long holi = 0;
                    var date = new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1);
                    if (at != null)
                    {
                        if (attendance.C1 != "0" && attendance.C1 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1) 
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C1 = attendance.C1;

                        if (attendance.C2 != "0" && attendance.C2 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 2)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C2 = attendance.C2;

                        if (attendance.C3 != "0" && attendance.C3 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 3)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C3 = attendance.C3;

                        if (attendance.C4 != "0" && attendance.C4 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 4)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C4 = attendance.C4;

                        if (attendance.C5 != "0" && attendance.C5 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 5)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C5 = attendance.C5;

                        if (attendance.C6 != "0" && attendance.C6 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 6)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C6 = attendance.C6;

                        if (attendance.C7 != "0" && attendance.C7 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 7)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C7 = attendance.C7;

                        if (attendance.C8 != "0" && attendance.C8 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 8)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C8 = attendance.C8;

                        if (attendance.C9 != "0" && attendance.C9 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 9)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C9 = attendance.C9;

                        if (attendance.C10 != "0" && attendance.C10 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 10)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C10 = attendance.C10;

                        if (attendance.C11 != "0" && attendance.C11 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 11)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C11 = attendance.C11;

                        if (attendance.C12 != "0" && attendance.C12 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 12)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C12 = attendance.C12;

                        if (attendance.C13 != "0" && attendance.C13 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 13)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C13 = attendance.C13;

                        if (attendance.C14 != "0" && attendance.C14 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 14)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C14 = attendance.C14;

                        if (attendance.C15 != "0" && attendance.C15 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 15)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C15 = attendance.C15;

                        if (attendance.C16 != "0" && attendance.C16 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 16)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C16 = attendance.C16;

                        if (attendance.C17 != "0" && attendance.C17 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 17)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C17 = attendance.C17;

                        if (attendance.C18 != "0" && attendance.C18 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 18)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C18 = attendance.C18;

                        if (attendance.C19 != "0" && attendance.C19 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 19)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C19 = attendance.C19;

                        if (attendance.C20 != "0" && attendance.C20 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 20)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C20 = attendance.C20;

                        if (attendance.C21 != "0" && attendance.C21 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 21)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C21 = attendance.C21;

                        if (attendance.C22 != "0" && attendance.C22 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 22)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C22 = attendance.C22;

                        if (attendance.C23 != "0" && attendance.C23 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 23)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C23 = attendance.C23;

                        if (attendance.C24 != "0" && attendance.C24 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 24)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C24 = attendance.C24;

                        if (attendance.C25 != "0" && attendance.C25 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 25)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C25 = attendance.C25;

                        if (attendance.C26 != "0" && attendance.C26 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 26)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C26 = attendance.C26;

                        if (attendance.C27 != "0" && attendance.C27 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 27)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C27 = attendance.C27;

                        if (attendance.C28 != "0" && attendance.C28 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 28)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C28 = attendance.C28;

                        if (attendance.C29 != "0" && attendance.C29 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 29)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C29 = attendance.C29;

                        if (attendance.C30 != "0" && attendance.C30 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 30)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C30 = attendance.C30;

                        if (attendance.C31 != "0" && attendance.C31 != null)
                            if (!ap.Exists(
                                x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 31)
                                     && !(x.status == null || x.status.Contains("rejected"))))
                                at.C31 = attendance.C31;

                        this.db.Entry(at).State = EntityState.Modified;
                        this.db.SaveChanges();
                        var hday = this.db.Holidays.ToList();
                        date = new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1);
                        for (var i = 0; i < DateTime.DaysInMonth(aa.TMonth.Year, aa.TMonth.Month); i++)
                        {
                            if (!hday.Exists(x => x.Date == date))
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

                                    at.FridayHours = fri1;
                                }
                            }

                            date = date.AddDays(1);
                        }

                        this.db.Entry(at).State = EntityState.Modified;
                        this.db.SaveChanges();
                        date = new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1);
                        for (var i = 0; i < DateTime.DaysInMonth(aa.TMonth.Year, aa.TMonth.Month); i++)
                        {
                            if (hday.Exists(x => x.Date == date))
                            {
                                if (date.Day == 1)
                                {
                                    long.TryParse(at.C1, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 2)
                                {
                                    long.TryParse(at.C2, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 3)
                                {
                                    long.TryParse(at.C3, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 4)
                                {
                                    long.TryParse(at.C4, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 5)
                                {
                                    long.TryParse(at.C5, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 6)
                                {
                                    long.TryParse(at.C6, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 7)
                                {
                                    long.TryParse(at.C7, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 8)
                                {
                                    long.TryParse(at.C8, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 9)
                                {
                                    long.TryParse(at.C9, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 10)
                                {
                                    long.TryParse(at.C10, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 11)
                                {
                                    long.TryParse(at.C11, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 12)
                                {
                                    long.TryParse(at.C11, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 13)
                                {
                                    long.TryParse(at.C13, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 14)
                                {
                                    long.TryParse(at.C14, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 15)
                                {
                                    long.TryParse(at.C15, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 16)
                                {
                                    long.TryParse(at.C16, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 17)
                                {
                                    long.TryParse(at.C17, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 18)
                                {
                                    long.TryParse(at.C18, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 19)
                                {
                                    long.TryParse(at.C19, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 20)
                                {
                                    long.TryParse(at.C20, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 21)
                                {
                                    long.TryParse(at.C21, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 22)
                                {
                                    long.TryParse(at.C22, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 23)
                                {
                                    long.TryParse(at.C23, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 24)
                                {
                                    long.TryParse(at.C24, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 25)
                                {
                                    long.TryParse(at.C25, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 26)
                                {
                                    long.TryParse(at.C26, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 27)
                                {
                                    long.TryParse(at.C27, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 28)
                                {
                                    long.TryParse(at.C28, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 29)
                                {
                                    long.TryParse(at.C29, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 30)
                                {
                                    long.TryParse(at.C30, out var tl);
                                    holi = holi + tl;
                                }

                                if (date.Day == 31)
                                {
                                    long.TryParse(at.C31, out var tl);
                                    holi = holi + tl;
                                }
                            }

                            at.Holidays = holi;

                            date = date.AddDays(1);
                        }

                        this.db.Entry(at).State = EntityState.Modified;
                        this.db.SaveChanges();
                        {
                            at = this.db.Attendances.Find(check.First().ID);
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
                            at.TotalHours = tl + tl1 + tl2 + tl3 + tl4 + tl5 + tl6 + tl7 + tl8 + tl9 + tl10 + tl11
                                            + tl12 + tl13 + tl14 + tl15 + tl16 + tl17 + tl18 + tl19 + tl20 + tl21 + tl22
                                            + tl23 + tl24 + tl25 + tl26 + tl27 + tl28 + tl29 + tl30;
                            this.db.Entry(at).State = EntityState.Modified;
                            this.db.SaveChanges();
                            var fday = this.GetAll(aa.TMonth);
                            var hlistday = this.GetAllholi(aa.TMonth);
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
                                var i = 0;
                                foreach (var l in t)
                                {
                                    i++;
                                    if (!(fday.Exists(x => x.Equals(i)) || hlistday.Exists(x => x.Equals(i))))
                                        if (l > tho)
                                        {
                                            tho1 += l - (long) tho;
                                            at.TotalOverTime = tho1;
                                        }
                                }

                                this.db.Entry(at).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }
                            {
                                var i = 0;
                                at.TotalSickLeave = 0;
                                long ts = 0;
                                if (!at.C1.IsNullOrWhiteSpace())
                                {
                                    if (at.C1.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C2.IsNullOrWhiteSpace())
                                {
                                    if (at.C2.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C3.IsNullOrWhiteSpace())
                                {
                                    if (at.C3.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C4.IsNullOrWhiteSpace())
                                {
                                    if (at.C4.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C5.IsNullOrWhiteSpace())
                                {
                                    if (at.C5.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C6.IsNullOrWhiteSpace())
                                {
                                    if (at.C6.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C7.IsNullOrWhiteSpace())
                                {
                                    if (at.C7.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C8.IsNullOrWhiteSpace())
                                {
                                    if (at.C8.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C9.IsNullOrWhiteSpace())
                                {
                                    if (at.C9.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C10.IsNullOrWhiteSpace())
                                {
                                    if (at.C10.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C11.IsNullOrWhiteSpace())
                                {
                                    if (at.C11.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C12.IsNullOrWhiteSpace())
                                {
                                    if (at.C12.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C13.IsNullOrWhiteSpace())
                                {
                                    if (at.C13.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C14.IsNullOrWhiteSpace())
                                {
                                    if (at.C14.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C15.IsNullOrWhiteSpace())
                                {
                                    if (at.C15.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C16.IsNullOrWhiteSpace())
                                {
                                    if (at.C16.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C17.IsNullOrWhiteSpace())
                                {
                                    if (at.C17.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C18.IsNullOrWhiteSpace())
                                {
                                    if (at.C18.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C19.IsNullOrWhiteSpace())
                                {
                                    if (at.C19.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C20.IsNullOrWhiteSpace())
                                {
                                    if (at.C20.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C21.IsNullOrWhiteSpace())
                                {
                                    if (at.C21.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C22.IsNullOrWhiteSpace())
                                {
                                    if (at.C22.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C23.IsNullOrWhiteSpace())
                                {
                                    if (at.C23.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C24.IsNullOrWhiteSpace())
                                {
                                    if (at.C24.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C25.IsNullOrWhiteSpace())
                                {
                                    if (at.C25.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C26.IsNullOrWhiteSpace())
                                {
                                    if (at.C26.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C27.IsNullOrWhiteSpace())
                                {
                                    if (at.C27.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C28.IsNullOrWhiteSpace())
                                {
                                    if (at.C28.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(29))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C29.IsNullOrWhiteSpace())
                                {
                                    if (at.C29.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C30.IsNullOrWhiteSpace())
                                {
                                    if (at.C30.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                            ts = ts + 1;
                                    i++;
                                }

                                if (!at.C31.IsNullOrWhiteSpace())
                                {
                                    if (at.C31.Equals("S"))
                                        if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                            ts = ts + 1;
                                    i++;
                                }

                                at.TotalSickLeave = ts;
                                this.db.Entry(at).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }
                            {
                                var i = 0;
                                at.TotalVL = 0;
                                long tv = 0;
                                if (!at.C1.IsNullOrWhiteSpace())
                                {
                                    if (at.C1.Equals("V"))

                                        if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(11))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C2.IsNullOrWhiteSpace())
                                {
                                    if (at.C2.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C3.IsNullOrWhiteSpace())
                                {
                                    if (at.C3.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C4.IsNullOrWhiteSpace())
                                {
                                    if (at.C4.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C5.IsNullOrWhiteSpace())
                                {
                                    if (at.C5.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C6.IsNullOrWhiteSpace())
                                {
                                    if (at.C6.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C7.IsNullOrWhiteSpace())
                                {
                                    if (at.C7.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C8.IsNullOrWhiteSpace())
                                {
                                    if (at.C8.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C9.IsNullOrWhiteSpace())
                                {
                                    if (at.C9.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C10.IsNullOrWhiteSpace())
                                {
                                    if (at.C10.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C11.IsNullOrWhiteSpace())
                                {
                                    if (at.C11.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C12.IsNullOrWhiteSpace())
                                {
                                    if (at.C12.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C13.IsNullOrWhiteSpace())
                                {
                                    if (at.C13.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C14.IsNullOrWhiteSpace())
                                {
                                    if (at.C14.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C15.IsNullOrWhiteSpace())
                                {
                                    if (at.C15.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C16.IsNullOrWhiteSpace())
                                {
                                    if (at.C16.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C17.IsNullOrWhiteSpace())
                                {
                                    if (at.C17.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C18.IsNullOrWhiteSpace())
                                {
                                    if (at.C18.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C19.IsNullOrWhiteSpace())
                                {
                                    if (at.C19.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C20.IsNullOrWhiteSpace())
                                {
                                    if (at.C20.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C21.IsNullOrWhiteSpace())
                                {
                                    if (at.C21.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C22.IsNullOrWhiteSpace())
                                {
                                    if (at.C22.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C23.IsNullOrWhiteSpace())
                                {
                                    if (at.C23.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C24.IsNullOrWhiteSpace())
                                {
                                    if (at.C24.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C25.IsNullOrWhiteSpace())
                                {
                                    if (at.C25.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C26.IsNullOrWhiteSpace())
                                {
                                    if (at.C26.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C27.IsNullOrWhiteSpace())
                                {
                                    if (at.C27.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C28.IsNullOrWhiteSpace())
                                {
                                    if (at.C28.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C29.IsNullOrWhiteSpace())
                                {
                                    if (at.C29.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C30.IsNullOrWhiteSpace())
                                {
                                    if (at.C30.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C31.IsNullOrWhiteSpace())
                                {
                                    if (at.C31.Equals("V"))
                                        if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                            tv = tv + 1;

                                    i++;
                                }

                                at.TotalVL = tv;
                                this.db.Entry(at).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }
                            {
                                var i = 0;
                                at.TotalAbsent = 0;
                                long tv = 0;
                                if (!at.C1.IsNullOrWhiteSpace())
                                {
                                    if (at.C1.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C2.IsNullOrWhiteSpace())
                                {
                                    if (at.C2.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C3.IsNullOrWhiteSpace())
                                {
                                    if (at.C3.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C4.IsNullOrWhiteSpace())
                                {
                                    if (at.C4.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                            tv = tv + 1;
                                    i++;
                                }

                                if (!at.C5.IsNullOrWhiteSpace())
                                {
                                    if (at.C5.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C6.IsNullOrWhiteSpace())
                                {
                                    if (at.C6.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C7.IsNullOrWhiteSpace())
                                {
                                    if (at.C7.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C8.IsNullOrWhiteSpace())
                                {
                                    if (at.C8.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C9.IsNullOrWhiteSpace())
                                {
                                    if (at.C9.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C10.IsNullOrWhiteSpace())
                                {
                                    if (at.C10.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C11.IsNullOrWhiteSpace())
                                {
                                    if (at.C11.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C12.IsNullOrWhiteSpace())
                                {
                                    if (at.C12.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C13.IsNullOrWhiteSpace())
                                {
                                    if (at.C13.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C14.IsNullOrWhiteSpace())
                                {
                                    if (at.C14.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C15.IsNullOrWhiteSpace())
                                {
                                    if (at.C15.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C16.IsNullOrWhiteSpace())
                                {
                                    if (at.C16.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C17.IsNullOrWhiteSpace())
                                {
                                    if (at.C17.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C18.IsNullOrWhiteSpace())
                                {
                                    if (at.C18.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C19.IsNullOrWhiteSpace())
                                {
                                    if (at.C19.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C20.IsNullOrWhiteSpace())
                                {
                                    if (at.C20.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C21.IsNullOrWhiteSpace())
                                {
                                    if (at.C21.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C22.IsNullOrWhiteSpace())
                                {
                                    if (at.C22.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C23.IsNullOrWhiteSpace())
                                {
                                    if (at.C23.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C24.IsNullOrWhiteSpace())
                                {
                                    if (at.C24.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C25.IsNullOrWhiteSpace())
                                {
                                    if (at.C25.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C26.IsNullOrWhiteSpace())
                                {
                                    if (at.C26.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C27.IsNullOrWhiteSpace())
                                {
                                    if (at.C27.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C28.IsNullOrWhiteSpace())
                                {
                                    if (at.C28.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C29.IsNullOrWhiteSpace())
                                {
                                    if (at.C29.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C30.IsNullOrWhiteSpace())
                                {
                                    if (at.C30.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C31.IsNullOrWhiteSpace())
                                {
                                    if (at.C31.Equals("A"))
                                        if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                            tv = tv + 1;

                                    i++;
                                }

                                at.TotalAbsent = tv;
                                this.db.Entry(at).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }
                            {
                                var i = 0;
                                at.TotalTransefer = 0;
                                long tv = 0;
                                if (!at.C1.IsNullOrWhiteSpace())
                                {
                                    if (at.C1.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C2.IsNullOrWhiteSpace())
                                {
                                    if (at.C2.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C3.IsNullOrWhiteSpace())
                                {
                                    if (at.C3.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C4.IsNullOrWhiteSpace())
                                {
                                    if (at.C4.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C5.IsNullOrWhiteSpace())
                                {
                                    if (at.C5.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C6.IsNullOrWhiteSpace())
                                {
                                    if (at.C6.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C7.IsNullOrWhiteSpace())
                                {
                                    if (at.C7.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C8.IsNullOrWhiteSpace())
                                {
                                    if (at.C8.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C9.IsNullOrWhiteSpace())
                                {
                                    if (at.C9.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C10.IsNullOrWhiteSpace())
                                {
                                    if (at.C10.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C11.IsNullOrWhiteSpace())
                                {
                                    if (at.C11.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C12.IsNullOrWhiteSpace())
                                {
                                    if (at.C12.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C13.IsNullOrWhiteSpace())
                                {
                                    if (at.C13.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C14.IsNullOrWhiteSpace())
                                {
                                    if (at.C14.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C15.IsNullOrWhiteSpace())
                                {
                                    if (at.C15.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C16.IsNullOrWhiteSpace())
                                {
                                    if (at.C16.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C17.IsNullOrWhiteSpace())
                                {
                                    if (at.C17.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C18.IsNullOrWhiteSpace())
                                {
                                    if (at.C18.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C19.IsNullOrWhiteSpace())
                                {
                                    if (at.C19.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C20.IsNullOrWhiteSpace())
                                {
                                    if (at.C20.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C21.IsNullOrWhiteSpace())
                                {
                                    if (at.C21.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C22.IsNullOrWhiteSpace())
                                {
                                    if (at.C22.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C23.IsNullOrWhiteSpace())
                                {
                                    if (at.C23.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C24.IsNullOrWhiteSpace())
                                {
                                    if (at.C24.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C25.IsNullOrWhiteSpace())
                                {
                                    if (at.C25.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C26.IsNullOrWhiteSpace())
                                {
                                    if (at.C26.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C27.IsNullOrWhiteSpace())
                                {
                                    if (at.C27.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C28.IsNullOrWhiteSpace())
                                {
                                    if (at.C28.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C29.IsNullOrWhiteSpace())
                                {
                                    if (at.C29.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C30.IsNullOrWhiteSpace())
                                {
                                    if (at.C30.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                            tv = tv + 1;

                                    i++;
                                }

                                if (!at.C31.IsNullOrWhiteSpace())
                                {
                                    if (at.C31.Equals("T"))
                                        if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                            tv = tv + 1;

                                    i++;
                                }

                                at.TotalTransefer = tv;
                                this.db.Entry(at).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }

                            at.status = "panding";
                        }

                        this.db.Entry(at).State = EntityState.Modified;
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    var at = attendance;
                    at.EmpID = attendance.EmpID;
                    at.SubMain = attendance.SubMain;
                    long fri1 = 0;
                    long holi = 0;
                    var date = new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1);
                    for (var i = 0; i < DateTime.DaysInMonth(aa.TMonth.Year, aa.TMonth.Month); i++)
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

                            at.FridayHours = fri1;
                        }

                        date = date.AddDays(1);
                    }

                    var hday = this.db.Holidays.ToList();
                    date = new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1);
                    for (var i = 0; i < DateTime.DaysInMonth(aa.TMonth.Year, aa.TMonth.Month); i++)
                    {
                        if (hday.Exists(x => x.Date == date))
                        {
                            if (date.Day == 1)
                            {
                                long.TryParse(at.C1, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 2)
                            {
                                long.TryParse(at.C2, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 3)
                            {
                                long.TryParse(at.C3, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 4)
                            {
                                long.TryParse(at.C4, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 5)
                            {
                                long.TryParse(at.C5, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 6)
                            {
                                long.TryParse(at.C6, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 7)
                            {
                                long.TryParse(at.C7, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 8)
                            {
                                long.TryParse(at.C8, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 9)
                            {
                                long.TryParse(at.C9, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 10)
                            {
                                long.TryParse(at.C10, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 11)
                            {
                                long.TryParse(at.C11, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 12)
                            {
                                long.TryParse(at.C11, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 13)
                            {
                                long.TryParse(at.C13, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 14)
                            {
                                long.TryParse(at.C14, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 15)
                            {
                                long.TryParse(at.C15, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 16)
                            {
                                long.TryParse(at.C16, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 17)
                            {
                                long.TryParse(at.C17, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 18)
                            {
                                long.TryParse(at.C18, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 19)
                            {
                                long.TryParse(at.C19, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 20)
                            {
                                long.TryParse(at.C20, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 21)
                            {
                                long.TryParse(at.C21, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 22)
                            {
                                long.TryParse(at.C22, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 23)
                            {
                                long.TryParse(at.C23, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 24)
                            {
                                long.TryParse(at.C24, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 25)
                            {
                                long.TryParse(at.C25, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 26)
                            {
                                long.TryParse(at.C26, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 27)
                            {
                                long.TryParse(at.C27, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 28)
                            {
                                long.TryParse(at.C28, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 29)
                            {
                                long.TryParse(at.C29, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 30)
                            {
                                long.TryParse(at.C30, out var tl);
                                holi = holi + tl;
                            }

                            if (date.Day == 31)
                            {
                                long.TryParse(at.C31, out var tl);
                                holi = holi + tl;
                            }

                            at.Holidays = holi;
                        }

                        date = date.AddDays(1);
                    }

                    if (attendance.C1 != "0" && attendance.C1 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 1)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C1 = attendance.C1;

                    if (attendance.C2 != "0" && attendance.C2 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 2)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C2 = attendance.C2;

                    if (attendance.C3 != "0" && attendance.C3 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 3)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C3 = attendance.C3;

                    if (attendance.C4 != "0" && attendance.C4 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 4)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C4 = attendance.C4;

                    if (attendance.C5 != "0" && attendance.C5 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 5)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C5 = attendance.C5;

                    if (attendance.C6 != "0" && attendance.C6 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 6)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C6 = attendance.C6;

                    if (attendance.C7 != "0" && attendance.C7 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 7)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C7 = attendance.C7;

                    if (attendance.C8 != "0" && attendance.C8 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 8)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C8 = attendance.C8;

                    if (attendance.C9 != "0" && attendance.C9 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 9)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C9 = attendance.C9;

                    if (attendance.C10 != "0" && attendance.C10 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 10)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C10 = attendance.C10;

                    if (attendance.C11 != "0" && attendance.C11 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 11)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C11 = attendance.C11;

                    if (attendance.C12 != "0" && attendance.C12 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 12)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C12 = attendance.C12;

                    if (attendance.C13 != "0" && attendance.C13 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 13)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C13 = attendance.C13;

                    if (attendance.C14 != "0" && attendance.C14 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 14)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C14 = attendance.C14;

                    if (attendance.C15 != "0" && attendance.C15 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 15)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C15 = attendance.C15;

                    if (attendance.C16 != "0" && attendance.C16 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 16)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C16 = attendance.C16;

                    if (attendance.C17 != "0" && attendance.C17 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 17)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C17 = attendance.C17;

                    if (attendance.C18 != "0" && attendance.C18 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 18)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C18 = attendance.C18;

                    if (attendance.C19 != "0" && attendance.C19 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 19)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C19 = attendance.C19;

                    if (attendance.C20 != "0" && attendance.C20 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 20)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C20 = attendance.C20;

                    if (attendance.C21 != "0" && attendance.C21 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 21)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C21 = attendance.C21;

                    if (attendance.C22 != "0" && attendance.C22 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 22)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C22 = attendance.C22;

                    if (attendance.C23 != "0" && attendance.C23 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 23)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C23 = attendance.C23;

                    if (attendance.C24 != "0" && attendance.C24 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 24)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C24 = attendance.C24;

                    if (attendance.C25 != "0" && attendance.C25 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 25)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C25 = attendance.C25;

                    if (attendance.C26 != "0" && attendance.C26 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 26)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C26 = attendance.C26;

                    if (attendance.C27 != "0" && attendance.C27 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 27)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C27 = attendance.C27;

                    if (attendance.C28 != "0" && attendance.C28 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 28)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C28 = attendance.C28;

                    if (attendance.C29 != "0" && attendance.C29 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 29)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C29 = attendance.C29;

                    if (attendance.C30 != "0" && attendance.C30 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 30)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C30 = attendance.C30;

                    if (attendance.C31 != "0" && attendance.C31 != null)
                        if (!ap.Exists(
                            x => x.adate == new DateTime(aa.TMonth.Year, aa.TMonth.Month, 31)
                                 && !(x.status == null || x.status.Contains("rejected"))))
                            at.C31 = attendance.C31;

                    if (attendance.TotalHours != 0)
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
                        var i = 0;
                        var fday = this.GetAll(aa.TMonth);
                        var hlistday = this.GetAllholi(aa.TMonth);
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
                            {
                                i++;
                                if (!(fday.Exists(x => x.Equals(i)) || hlistday.Exists(x => x.Equals(i))))
                                    if (l > tho)
                                    {
                                        tho1 += l - (long) tho;
                                        at.TotalOverTime = tho1;
                                    }
                            }
                        }

                        i = 0;
                        {
                            at.TotalSickLeave = 0;
                            long ts = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                            {
                                if (at.C1.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C2.IsNullOrWhiteSpace())
                            {
                                if (at.C2.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C3.IsNullOrWhiteSpace())
                            {
                                if (at.C3.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C4.IsNullOrWhiteSpace())
                            {
                                if (at.C4.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C5.IsNullOrWhiteSpace())
                            {
                                if (at.C5.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C6.IsNullOrWhiteSpace())
                            {
                                if (at.C6.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C7.IsNullOrWhiteSpace())
                            {
                                if (at.C7.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C8.IsNullOrWhiteSpace())
                            {
                                if (at.C8.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C9.IsNullOrWhiteSpace())
                            {
                                if (at.C9.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C10.IsNullOrWhiteSpace())
                            {
                                if (at.C10.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C11.IsNullOrWhiteSpace())
                            {
                                if (at.C11.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C12.IsNullOrWhiteSpace())
                            {
                                if (at.C12.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C13.IsNullOrWhiteSpace())
                            {
                                if (at.C13.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C14.IsNullOrWhiteSpace())
                            {
                                if (at.C14.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C15.IsNullOrWhiteSpace())
                            {
                                if (at.C15.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C16.IsNullOrWhiteSpace())
                            {
                                if (at.C16.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C17.IsNullOrWhiteSpace())
                            {
                                if (at.C17.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C18.IsNullOrWhiteSpace())
                            {
                                if (at.C18.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C19.IsNullOrWhiteSpace())
                            {
                                if (at.C19.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C20.IsNullOrWhiteSpace())
                            {
                                if (at.C20.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C21.IsNullOrWhiteSpace())
                            {
                                if (at.C21.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C22.IsNullOrWhiteSpace())
                            {
                                if (at.C22.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C23.IsNullOrWhiteSpace())
                            {
                                if (at.C23.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C24.IsNullOrWhiteSpace())
                            {
                                if (at.C24.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C25.IsNullOrWhiteSpace())
                            {
                                if (at.C25.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C26.IsNullOrWhiteSpace())
                            {
                                if (at.C26.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C27.IsNullOrWhiteSpace())
                            {
                                if (at.C27.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C28.IsNullOrWhiteSpace())
                            {
                                if (at.C28.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C29.IsNullOrWhiteSpace())
                            {
                                if (at.C29.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C30.IsNullOrWhiteSpace())
                            {
                                if (at.C30.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                        ts = ts + 1;
                                i++;
                            }

                            if (!at.C31.IsNullOrWhiteSpace())
                            {
                                if (at.C31.Equals("S"))
                                    if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                        ts = ts + 1;
                                i++;
                            }

                            at.TotalSickLeave = ts;
                        }
                        {
                            i = 0;
                            at.TotalVL = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                            {
                                if (at.C1.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C2.IsNullOrWhiteSpace())
                            {
                                if (at.C2.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C3.IsNullOrWhiteSpace())
                            {
                                if (at.C3.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C4.IsNullOrWhiteSpace())
                            {
                                if (at.C4.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C5.IsNullOrWhiteSpace())
                            {
                                if (at.C5.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C6.IsNullOrWhiteSpace())
                            {
                                if (at.C6.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C7.IsNullOrWhiteSpace())
                            {
                                if (at.C7.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C8.IsNullOrWhiteSpace())
                            {
                                if (at.C8.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C9.IsNullOrWhiteSpace())
                            {
                                if (at.C9.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C10.IsNullOrWhiteSpace())
                            {
                                if (at.C10.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C11.IsNullOrWhiteSpace())
                            {
                                if (at.C11.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C12.IsNullOrWhiteSpace())
                            {
                                if (at.C12.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C13.IsNullOrWhiteSpace())
                            {
                                if (at.C13.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C14.IsNullOrWhiteSpace())
                            {
                                if (at.C14.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C15.IsNullOrWhiteSpace())
                            {
                                if (at.C15.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C16.IsNullOrWhiteSpace())
                            {
                                if (at.C16.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C17.IsNullOrWhiteSpace())
                            {
                                if (at.C17.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C18.IsNullOrWhiteSpace())
                            {
                                if (at.C18.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C19.IsNullOrWhiteSpace())
                            {
                                if (at.C19.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C20.IsNullOrWhiteSpace())
                            {
                                if (at.C20.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C21.IsNullOrWhiteSpace())
                            {
                                if (at.C21.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C22.IsNullOrWhiteSpace())
                            {
                                if (at.C22.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C23.IsNullOrWhiteSpace())
                            {
                                if (at.C23.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C24.IsNullOrWhiteSpace())
                            {
                                if (at.C24.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C25.IsNullOrWhiteSpace())
                            {
                                if (at.C25.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C26.IsNullOrWhiteSpace())
                            {
                                if (at.C26.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C27.IsNullOrWhiteSpace())
                            {
                                if (at.C27.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C28.IsNullOrWhiteSpace())
                            {
                                if (at.C28.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C29.IsNullOrWhiteSpace())
                            {
                                if (at.C29.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C30.IsNullOrWhiteSpace())
                            {
                                if (at.C30.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C31.IsNullOrWhiteSpace())
                            {
                                if (at.C31.Equals("V"))
                                    if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                        tv = tv + 1;

                                i++;
                            }

                            at.TotalVL = tv;
                        }
                        {
                            i = 0;
                            at.TotalAbsent = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                            {
                                if (at.C1.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C2.IsNullOrWhiteSpace())
                            {
                                if (at.C2.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C3.IsNullOrWhiteSpace())
                            {
                                if (at.C3.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C4.IsNullOrWhiteSpace())
                            {
                                if (at.C4.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C5.IsNullOrWhiteSpace())
                            {
                                if (at.C5.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C6.IsNullOrWhiteSpace())
                            {
                                if (at.C6.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C7.IsNullOrWhiteSpace())
                            {
                                if (at.C7.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C8.IsNullOrWhiteSpace())
                            {
                                if (at.C8.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C9.IsNullOrWhiteSpace())
                            {
                                if (at.C9.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C10.IsNullOrWhiteSpace())
                            {
                                if (at.C10.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C11.IsNullOrWhiteSpace())
                            {
                                if (at.C11.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C12.IsNullOrWhiteSpace())
                            {
                                if (at.C12.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C13.IsNullOrWhiteSpace())
                            {
                                if (at.C13.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C14.IsNullOrWhiteSpace())
                            {
                                if (at.C14.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C15.IsNullOrWhiteSpace())
                            {
                                if (at.C15.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C16.IsNullOrWhiteSpace())
                            {
                                if (at.C16.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C17.IsNullOrWhiteSpace())
                            {
                                if (at.C17.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C18.IsNullOrWhiteSpace())
                            {
                                if (at.C18.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C19.IsNullOrWhiteSpace())
                            {
                                if (at.C19.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C20.IsNullOrWhiteSpace())
                            {
                                if (at.C20.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C21.IsNullOrWhiteSpace())
                            {
                                if (at.C21.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C22.IsNullOrWhiteSpace())
                            {
                                if (at.C22.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C23.IsNullOrWhiteSpace())
                            {
                                if (at.C23.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C24.IsNullOrWhiteSpace())
                            {
                                if (at.C24.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C25.IsNullOrWhiteSpace())
                            {
                                if (at.C25.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C26.IsNullOrWhiteSpace())
                            {
                                if (at.C26.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C27.IsNullOrWhiteSpace())
                            {
                                if (at.C27.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C28.IsNullOrWhiteSpace())
                            {
                                if (at.C28.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C29.IsNullOrWhiteSpace())
                            {
                                if (at.C29.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C30.IsNullOrWhiteSpace())
                            {
                                if (at.C30.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C31.IsNullOrWhiteSpace())
                            {
                                if (at.C31.Equals("A"))
                                    if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                        tv = tv + 1;

                                i++;
                            }

                            at.TotalAbsent = tv;
                        }
                        {
                            i = 0;
                            at.TotalTransefer = 0;
                            long tv = 0;
                            if (!at.C1.IsNullOrWhiteSpace())
                            {
                                if (at.C1.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C2.IsNullOrWhiteSpace())
                            {
                                if (at.C2.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C3.IsNullOrWhiteSpace())
                            {
                                if (at.C3.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C4.IsNullOrWhiteSpace())
                            {
                                if (at.C4.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C5.IsNullOrWhiteSpace())
                            {
                                if (at.C5.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C6.IsNullOrWhiteSpace())
                            {
                                if (at.C6.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C7.IsNullOrWhiteSpace())
                            {
                                if (at.C7.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C8.IsNullOrWhiteSpace())
                            {
                                if (at.C8.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C9.IsNullOrWhiteSpace())
                            {
                                if (at.C9.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C10.IsNullOrWhiteSpace())
                            {
                                if (at.C10.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C11.IsNullOrWhiteSpace())
                            {
                                if (at.C11.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C12.IsNullOrWhiteSpace())
                            {
                                if (at.C12.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C13.IsNullOrWhiteSpace())
                            {
                                if (at.C13.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C14.IsNullOrWhiteSpace())
                            {
                                if (at.C14.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C15.IsNullOrWhiteSpace())
                            {
                                if (at.C15.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C16.IsNullOrWhiteSpace())
                            {
                                if (at.C16.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C17.IsNullOrWhiteSpace())
                            {
                                if (at.C17.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C18.IsNullOrWhiteSpace())
                            {
                                if (at.C18.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C19.IsNullOrWhiteSpace())
                            {
                                if (at.C19.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C20.IsNullOrWhiteSpace())
                            {
                                if (at.C20.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C21.IsNullOrWhiteSpace())
                            {
                                if (at.C21.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C22.IsNullOrWhiteSpace())
                            {
                                if (at.C22.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C23.IsNullOrWhiteSpace())
                            {
                                if (at.C23.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C24.IsNullOrWhiteSpace())
                            {
                                if (at.C24.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C25.IsNullOrWhiteSpace())
                            {
                                if (at.C25.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C26.IsNullOrWhiteSpace())
                            {
                                if (at.C26.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C27.IsNullOrWhiteSpace())
                            {
                                if (at.C27.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C28.IsNullOrWhiteSpace())
                            {
                                if (at.C28.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C29.IsNullOrWhiteSpace())
                            {
                                if (at.C29.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C30.IsNullOrWhiteSpace())
                            {
                                if (at.C30.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                        tv = tv + 1;

                                i++;
                            }

                            if (!at.C31.IsNullOrWhiteSpace())
                            {
                                if (at.C31.Equals("T"))
                                    if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                        tv = tv + 1;

                                i++;
                            }

                            at.TotalTransefer = tv;
                        }

                        at.status = "panding";
                    }

                    this.db.Attendances.Add(at);
                    this.db.SaveChanges();
                }

                return this.RedirectToAction("AIndex");
            }

            model1 = new timesheetViewModel {attendance = attendance};
            return this.View(model1);
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Asearch(int? page, int? pagesize, string search)
        {
            var a = this.db.MainTimeSheets.OrderByDescending(m => m.ID);

            // var aa = a.First();
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
        public ActionResult csearch(DateTime? mtsmonth, long? csmps, string MM)
        {
            var ll = new List<Attendance>();
            DateTime date;
            if (mtsmonth.HasValue)
            {
                DateTime.TryParse(mtsmonth.ToString(), out date);
                this.ViewBag.dateee = date.ToShortDateString();
            }
            else
            {
                date = DateTime.Now;
                this.ViewBag.dateee = date;
            }

            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));

                this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }
            else
            {
                this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }

            this.ViewBag.csmps1 = csmps;
            this.ViewBag.mtsmonth1 = date;
            this.db.Database.CommandTimeout = 300;
            this.ViewBag.MM = MM;
            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            var list = this.db.Attendances.Include(x => x.LabourMaster).ToList();
            var atlist = new List<Attendance>();
            if (csmps.HasValue && mtsmonth.HasValue)
            {
                DateTime.TryParse(mtsmonth.Value.ToString(), out var dm);
                long.TryParse(csmps.ToString(), out var lcsmps);
                var ab = this.db.MainTimeSheets
                    .Where(
                        x => x.TMonth.Month.Equals(dm.Month) && x.TMonth.Year.Equals(dm.Year)
                                                             && x.ManPowerSupplier.Equals(lcsmps)).OrderBy(x => x.ID)
                    .ToPagedList(1, 1000);
                if (ab.Count != 0)
                {
                    foreach (var abis in ab)
                        atlist.AddRange(
                            this.db.Attendances.Where(x => x.SubMain.Equals(abis.ID)).Include(x => x.LabourMaster));
                    var listat = new List<Attendance>();
                    foreach (var VA in atlist.OrderBy(x => x.ID))
                    {
                        if (!listat.Exists(x =>
                            x.MainTimeSheet.ProjectList.PROJECT_NAME == VA.MainTimeSheet.ProjectList.PROJECT_NAME &&
                            x.EmpID == VA.EmpID))
                        {
                            listat.Add(VA);
                        }
                    }

                    return this.View(
                        listat.OrderBy(x => x.MainTimeSheet.Project).ThenBy(x => x.ID).ToPagedList(1, 1000));
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "the combination does not exist");
                    return this.View(ll.OrderByDescending(x => x.ID).ToPagedList(1, 1000));
                }
            }

            return this.View(ll.OrderBy(x => x.LabourMaster.EMPNO).ToPagedList(1, 1000));
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult tomcreate(DateTime TMonth, long? ManPowerSupplier, long? Project)
        {
            var mts = new MainTimeSheet();
            mts.TMonth = TMonth;
            mts.Project = Project.Value;
            mts.ManPowerSupplier = ManPowerSupplier.Value;
            return this.RedirectToAction("MCreate", new {mts});
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult MCreate()
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                var t = new List<ProjectList>();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
                this.ViewBag.Project = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }
            else
            {
                this.ViewBag.Project = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
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
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                var t = new List<ProjectList>();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));

                this.ViewBag.Project = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }
            else
            {
                this.ViewBag.Project = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }

            this.ViewBag.ManPowerSupplier = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            this.Tmonth = mainTimeSheet.TMonth.Date;
            this.ID = mainTimeSheet.ID;
            this.manid = mainTimeSheet.ManPowerSupplier;
            this.pid = mainTimeSheet.Project;
            var ids = new MainTimeSheet();
            if (this.ModelState.IsValid)
            {
                var apall = this.db.approvals.ToList();
                if (!apall.Exists(
                    x => x.MPS_id == mainTimeSheet.ManPowerSupplier && x.P_id == mainTimeSheet.Project
                                                                    && x.adate == mainTimeSheet.TMonth
                                                                    && (x.status.Equals("submitted")
                                                                        || x.status.Equals("approved"))))
                {
                    var te = this.db.MainTimeSheets.OrderByDescending(x => x.ID).ToList();
                    if (te.Exists(x =>
                        x.TMonth.Month == mainTimeSheet.TMonth.Month && x.TMonth.Year == mainTimeSheet.TMonth.Year &&
                        x.Project == mainTimeSheet.Project && x.ManPowerSupplier == mainTimeSheet.ManPowerSupplier))
                    {
                        var te1 = te.Find(x =>
                            x.TMonth.Month == mainTimeSheet.TMonth.Month && x.TMonth.Year == mainTimeSheet.TMonth.Year
                                                                         && x.Project == mainTimeSheet.Project
                                                                         && x.ManPowerSupplier
                                                                         == mainTimeSheet.ManPowerSupplier);
                        if (mainTimeSheet.TMonth.Month == te[0].TMonth.Month
                            && mainTimeSheet.TMonth.Year == te[0].TMonth.Year
                            && mainTimeSheet.ManPowerSupplier == te[0].ManPowerSupplier
                            && te[0].Project == mainTimeSheet.Project) goto qw;
                        te1.TMonth = mainTimeSheet.TMonth;
                        this.db.MainTimeSheets.Add(mainTimeSheet);
                        this.db.SaveChanges();
                        te = this.db.MainTimeSheets.OrderByDescending(x => x.ID).ToList();
                        var teall = te.FindAll(
                            x => x.TMonth.Month == mainTimeSheet.TMonth.Month
                                 && x.TMonth.Year == mainTimeSheet.TMonth.Year
                                 && x.ManPowerSupplier == mainTimeSheet.ManPowerSupplier
                                 && x.Project == mainTimeSheet.Project).OrderBy(x => x.ID);
                        var atalllist = this.db.Attendances.ToList();
                        if (teall != null || teall.Count() != 0)
                        {
                            var atallfind = new List<Attendance>();
                            foreach (var sheet in teall)
                            {
                                var zz = atalllist.FindAll(x => x.SubMain == sheet.ID);
                                if (zz.Count != 0) atallfind = atalllist.FindAll(x => x.SubMain == sheet.ID);
                            }

                            foreach (var ql in atallfind)
                            {
                                ql.SubMain = teall.Last().ID;
                                this.db.Entry(ql).State = EntityState.Modified;
                                this.db.SaveChanges();
                            }
                        }

                        ids = teall.Last();
                        return this.RedirectToAction("AIndex", "Home", ids);
                    }
                    else
                    {
                        this.db.MainTimeSheets.Add(mainTimeSheet);
                        this.db.SaveChanges();
                        var te2 = this.db.MainTimeSheets.ToList().OrderBy(x => x.ID).Last();
                        ids = te2;
                        return this.RedirectToAction("AIndex", "Home", ids);
                    }
                }
                else
                {
                    var errr1 = "timesheet already " + apall.Find(
                                        x => x.MPS_id == mainTimeSheet.ManPowerSupplier
                                             && x.P_id == mainTimeSheet.Project
                                             && x.adate == mainTimeSheet.TMonth)
                                    .status;
                    this.ModelState.AddModelError(string.Empty, errr1);
                }
            }

            return this.View(mainTimeSheet);
            qw:
            var te3 = this.db.MainTimeSheets.ToList().OrderBy(x => x.ID).Last();
            ids = te3;
            ids.TMonth = mainTimeSheet.TMonth;
            this.db.Entry(ids).State = EntityState.Modified;
            this.db.SaveChanges();
            return this.RedirectToAction("AIndex", "Home", ids);
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
                new SelectListItem {Text = string.Empty, Value = string.Empty},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"},
                new SelectListItem {Text = "17", Value = "17"},
                new SelectListItem {Text = "18", Value = "18"},
                new SelectListItem {Text = "19", Value = "19"},
                new SelectListItem {Text = "20", Value = "20"},
                new SelectListItem {Text = "21", Value = "21"},
                new SelectListItem {Text = "22", Value = "22"},
                new SelectListItem {Text = "23", Value = "23"},
                new SelectListItem {Text = "24", Value = "24"},
                new SelectListItem {Text = "S", Value = "S"},
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "T", Value = "T"},
                new SelectListItem {Text = "V", Value = "V"}
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
            this.ViewBag.emplist = d.Select(x => x.EMPNO).ToList();
            this.ViewBag.empno = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "EMPNO");
            this.ViewBag.name = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Person_Name");
            this.ViewBag.position = new SelectList(d.Where(x => x.EMPNO >= 4).OrderBy(m => m.EMPNO), "ID", "Position");

            var data = new[]
            {
                new SelectListItem {Text = string.Empty, Value = string.Empty},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"},
                new SelectListItem {Text = "17", Value = "17"},
                new SelectListItem {Text = "18", Value = "18"},
                new SelectListItem {Text = "19", Value = "19"},
                new SelectListItem {Text = "20", Value = "20"},
                new SelectListItem {Text = "21", Value = "21"},
                new SelectListItem {Text = "22", Value = "22"},
                new SelectListItem {Text = "23", Value = "23"},
                new SelectListItem {Text = "24", Value = "24"},
                new SelectListItem {Text = "S", Value = "S"},
                new SelectListItem {Text = "A", Value = "A"},
                new SelectListItem {Text = "T", Value = "T"},
                new SelectListItem {Text = "V", Value = "V"}
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
                                                                        && x.Project.Equals(aa.Project))
                        .OrderByDescending(x => x.ID).ToList();

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
                        var lo1 = new List<Attendance>();
                        foreach (var same in lo)
                        {
                            var at1 = this.db.Attendances.Where(e => e.SubMain.Equals(same.ID)).ToList();
                            if (at1 != null)
                                foreach (var at2 in at1)
                                    lo1.Add(at2);
                        }

                        var lo3 = new List<Attendance>();
                        foreach (var lo2 in lo1)
                            if (list.empno == lo2.EmpID)
                                lo3.Add(lo2);

                        long com = 0;
                        foreach (var at1 in lo3)
                        {
                            if (check.Count != 0) at = check.First();
                            if (!at1.SubMain.Equals(at.SubMain))
                            {
                                if (list.date.Day == 1)
                                {
                                    long.TryParse(at1.C1, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 2)
                                {
                                    long.TryParse(at1.C2, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 3)
                                {
                                    long.TryParse(at1.C3, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 4)
                                {
                                    long.TryParse(at1.C4, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 5)
                                {
                                    long.TryParse(at1.C5, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 6)
                                {
                                    long.TryParse(at1.C6, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 7)
                                {
                                    long.TryParse(at1.C7, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 8)
                                {
                                    long.TryParse(at1.C8, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 9)
                                {
                                    long.TryParse(at1.C9, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 10)
                                {
                                    long.TryParse(at1.C10, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 11)
                                {
                                    long.TryParse(at1.C11, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 12)
                                {
                                    long.TryParse(at1.C12, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 13)
                                {
                                    long.TryParse(at1.C13, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 14)
                                {
                                    long.TryParse(at1.C14, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 15)
                                {
                                    long.TryParse(at1.C15, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 16)
                                {
                                    long.TryParse(at1.C16, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 17)
                                {
                                    long.TryParse(at1.C17, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 18)
                                {
                                    long.TryParse(at1.C18, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 19)
                                {
                                    long.TryParse(at1.C19, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 20)
                                {
                                    long.TryParse(at1.C20, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 21)
                                {
                                    long.TryParse(at1.C21, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 22)
                                {
                                    long.TryParse(at1.C22, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 23)
                                {
                                    long.TryParse(at1.C23, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 24)
                                {
                                    long.TryParse(at1.C24, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 25)
                                {
                                    long.TryParse(at1.C25, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 26)
                                {
                                    long.TryParse(at1.C26, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 27)
                                {
                                    long.TryParse(at1.C27, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 28)
                                {
                                    long.TryParse(at1.C28, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 29)
                                {
                                    long.TryParse(at1.C29, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 30)
                                {
                                    long.TryParse(at1.C30, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
                                    }
                                }

                                if (list.date.Day == 31)
                                {
                                    long.TryParse(at1.C31, out var k);
                                    long.TryParse(list.hours, out var l);
                                    com = k + l;
                                    if (com > 24)
                                    {
                                        var dd = this.db.LabourMasters.Find(list.empno);
                                        var errorm = "total time of the employee no:" + dd.EMPNO.ToString()
                                                                                      + " for the day is "
                                                                                      + com.ToString()
                                                                                      + "hrs which is greater then 24hrs";
                                        this.ModelState.AddModelError(string.Empty, errorm);
                                        return this.View(test);
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

                    this.db.Entry(at).State = EntityState.Modified;
                    this.db.SaveChanges();
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
                                    tho1 += l - (long) tho;
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

                return this.RedirectToAction("tests");
            }

            return this.View(test);
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult download(DateTime? mtsmonth2, long? csp2, long? csmps2)
        {
            this.errorm = this.TempData["mydata"] as string;
            this.ModelState.AddModelError(string.Empty, this.errorm);
            DateTime date1 = new DateTime();
            var final1 = new List<test>();
            if (mtsmonth2.HasValue)
            {
                //date1 = new DateTime(mtsmonth2.Value.Year, mtsmonth2.Value.Month, mtsmonth2.Value.Day);
                date1 = mtsmonth2.Value;
                this.ViewBag.dateee = date1.ToString("D");
            }
            else
            {
                date1 = DateTime.Now;
                this.ViewBag.dateee = date1.ToString("D");
            }

            var apall = this.db.approvals.ToList();
            this.ViewBag.csp1 = csp2;
            this.ViewBag.csmps1 = csmps2;
            this.ViewBag.mtsmonth1 = date1;
            this.db.Database.CommandTimeout = 300;
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));

                this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }
            else
            {
                this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            }

            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            var list = this.db.Attendances.Include(x => x.LabourMaster).ToList();
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

                foreach (var abis in ab)
                {
                    var ass = this.db.Attendances.Where(x => x.SubMain.Equals(abis.ID)).Include(x => x.LabourMaster)
                        .ToList();
                    foreach (var attendance in ass)
                    {
                        var et = new test();
                        var epno = this.db.LabourMasters.Find(attendance.EmpID);
                        et.empno = epno.EMPNO;
                        if (apall.Exists(x => x.A_id == attendance.ID && x.adate == dm))
                            if (apall.Exists(x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm))
                            {
                                et.approved_by = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).Ausername;
                                et.status = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).status;
                                et.submitted_by = apall.Find(
                                    x => x.status != "submitted" && x.A_id == attendance.ID && x.adate == dm).Susername;
                            }

                        if (date1.Day == 1) et.hours = attendance.C1;

                        if (date1.Day == 2) et.hours = attendance.C2;

                        if (date1.Day == 3) et.hours = attendance.C3;

                        if (date1.Day == 4) et.hours = attendance.C4;

                        if (date1.Day == 5) et.hours = attendance.C5;

                        if (date1.Day == 6) et.hours = attendance.C6;

                        if (date1.Day == 7) et.hours = attendance.C7;

                        if (date1.Day == 8) et.hours = attendance.C8;

                        if (date1.Day == 9) et.hours = attendance.C9;

                        if (date1.Day == 10) et.hours = attendance.C10;

                        if (date1.Day == 11) et.hours = attendance.C11;

                        if (date1.Day == 12) et.hours = attendance.C12;

                        if (date1.Day == 13) et.hours = attendance.C13;

                        if (date1.Day == 14) et.hours = attendance.C14;

                        if (date1.Day == 15) et.hours = attendance.C15;

                        if (date1.Day == 16) et.hours = attendance.C16;

                        if (date1.Day == 17) et.hours = attendance.C17;

                        if (date1.Day == 18) et.hours = attendance.C18;

                        if (date1.Day == 19) et.hours = attendance.C19;

                        if (date1.Day == 20) et.hours = attendance.C20;

                        if (date1.Day == 21) et.hours = attendance.C21;

                        if (date1.Day == 22) et.hours = attendance.C22;

                        if (date1.Day == 23) et.hours = attendance.C23;

                        if (date1.Day == 24) et.hours = attendance.C24;

                        if (date1.Day == 25) et.hours = attendance.C25;
                        
                        if (date1.Day == 26) et.hours = attendance.C26;

                        if (date1.Day == 27) et.hours = attendance.C27;

                        if (date1.Day == 28) et.hours = attendance.C28;

                        if (date1.Day == 29) et.hours = attendance.C29;

                        if (date1.Day == 30) et.hours = attendance.C30;

                        if (date1.Day == 31) et.hours = attendance.C31;

                        if (!final1.Exists(x => x.empno.Equals(et.empno)))
                            if (et.hours != null)
                                final1.Add(et);
                    }
                }

                return this.View(final1.OrderBy(x => x.empno).ToPagedList(1, 100));
            }

            return this.View(final1.OrderBy(x => x.empno).ToPagedList(1, 100));
        }

        public void SendMail(string sup, string prop, DateTime da, string na)
        {
            var man = this.db.AspNetUsers.ToList();
            var context = new ApplicationDbContext();
            var users = context.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains("6023f0a5-8d24-45d3-9641-b3c2e39aa763")).ToList();
            var asa = new List<AspNetUser>();
            var cper = this.db.CsPermissions.ToList();
            foreach (var user1 in users) asa.Add(man.Find(x => x.Id == user1.Id));

            var pname = this.db.ProjectLists.ToList();
            var pname1 = pname.Find(x => x.PROJECT_NAME == prop);
            var fuser1 = new List<CsPermission>();
            foreach (var netUser in asa)
                if (cper.Exists(x => x.CsUser == netUser.csid && x.Project == pname1.ID))
                    fuser1.Add(cper.Find(x => x.CsUser == netUser.csid && x.Project == pname1.ID));
            var pno = fuser1.FindAll(x => x.Project == pname1.ID);
            var pasa = new List<AspNetUser>();
            foreach (var permission in pno) pasa.Add(asa.Find(x => x.csid == permission.CsUser));
            /*var oMail = new SmtpMail("TryIt");
            var ccstring =
                "mkhairy@citiscapegroup.com,efathy@citiscapegroup.com,zNader@citiscapegroup.com,amohamed@itiscapegroup.com";
            oMail.From = "timekeeper@citiscapegroup.com";
            oMail.To = pasa.First().Email;
            pasa.Remove(pasa.First());
            foreach (var ccpasa in pasa) ccstring += "," + ccpasa.Email;
            oMail.Cc = ccstring;
            oMail.Subject = "A NEW TIMESHEET SUBMITTED";
            oMail.TextBody = "Dear Sir,\n\n Please note that I have sent a new Time-Sheet for the date "
                             + da.ToShortDateString() + ", ManPowerSupplier: " + sup + " and Project name: " + prop
                             + " for you to  approve / reject\n\nBest regards\n" + na + "\n\n\n\n";
            var oServer = new SmtpServer("outlook.office365.com");
            oServer.Protocol = ServerProtocol.ExchangeEWS;
            oServer.User = "timekeeper@citiscapegroup.com";
            oServer.Password = "Vam15380";
            oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
            var oSmtp = new SmtpClient();
            oSmtp.SendMail(oServer, oMail);*/
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("timekeeper", "timekeeper@citiscapegroup.com"));
                message.To.Add(new MailboxAddress(pasa.First().UserName, pasa.First().Email));
                string[] ccstring =
                {
                    "mkhairy@citiscapegroup.com", "efathy@citiscapegroup.com", "zNader@citiscapegroup.com",
                    "amohamed@itiscapegroup.com"
                };
                foreach (var VARIABLE in ccstring)
                {
                    message.Cc.Add(new MailboxAddress(VARIABLE));
                }

                pasa.Remove(pasa.First());
                foreach (var ccpasa in pasa) message.Cc.Add(new MailboxAddress(ccpasa.Email));
                message.Subject = "A NEW TIMESHEET SUBMITTED";

                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir,

Please note that I have sent a new Time-Sheet for the date " + da.ToShortDateString() + ", ManPowerSupplier: " + sup + " and Project name: " + prop + " for you to  approve / reject\n\nBest regards\n" + na + "\n\n\n\n"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("outlook.office365.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("timekeeper@citiscapegroup.com", "Vam15380");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }

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
                    var i = 0;
                    foreach (var attendance in ass)
                        if (!apall.Exists(x => x.A_id == attendance.ID && x.adate == dm))
                        {
                            if (this.User.IsInRole("Employee"))
                            {
                                i++;
                                attendance.status = "submitted for " + dm.Day;
                                this.db.Entry(attendance).State = EntityState.Modified;
                                this.db.SaveChanges();
                                var ap = new approval();
                                ap.MPS_id = csmps2;
                                ap.P_id = csp2;
                                ap.adate = dm;
                                ap.status = "submitted";
                                ap.A_id = attendance.ID;
                                ap.Susername = this.User.Identity.Name;
                                ap.Empno = attendance.LabourMaster.EMPNO;
                                this.db.approvals.Add(ap);
                                if (i == 1)
                                    this.SendMail(
                                        attendance.MainTimeSheet.ManPowerSupplier1.Supplier,
                                        attendance.MainTimeSheet.ProjectList.PROJECT_NAME,
                                        dm,
                                        this.User.Identity.Name);
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
                                    this.errorm = "\n already submitted;";
                                    this.ModelState.AddModelError(string.Empty, this.errorm);
                                    this.TempData["mydata"] = this.errorm;
                                }

                                if (aa1.status.Contains("rejected") && aa1.adate == dm)
                                    if (this.User.IsInRole("Employee"))
                                    {
                                        attendance.status = "submitted for " + dm.Day;
                                        this.db.Entry(attendance).State = EntityState.Modified;
                                        this.db.SaveChanges();
                                        aa1.status = "submitted";
                                        this.db.Entry(aa1).State = EntityState.Modified;
                                        if (i == 1)
                                            this.SendMail(
                                                attendance.MainTimeSheet.ManPowerSupplier1.Supplier,
                                                attendance.MainTimeSheet.ProjectList.PROJECT_NAME,
                                                dm,
                                                this.User.Identity.Name);
                                        this.db.SaveChanges();
                                    }

                                if (aa1.status == "approved" && aa1.adate == dm)
                                {
                                    this.errorm = "\n already approved;";
                                    this.ModelState.AddModelError(string.Empty, this.errorm);
                                    this.TempData["mydata"] = this.errorm;
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

        public void DownloadExcelfull(DateTime? mtsmonth1, long? csmps1)
        {
            List<Attendance> listat;
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("Attendances");
            var row = 4;
            var pcount = 5;
            DateTime.TryParse(mtsmonth1.ToString(), out var date);
            var m = this.db.ManPowerSuppliers.Find(csmps1);
            Sheet.Cells["C1"].Value = m.Supplier;
            Sheet.Cells["E1"].Value = date.ToLongDateString();

            Sheet.Cells["A3"].Value = "Emp ID";
            Sheet.Cells["B3"].Value = "1";
            Sheet.Cells["C3"].Value = "2";
            Sheet.Cells["D3"].Value = "3";
            Sheet.Cells["E3"].Value = "4";
            Sheet.Cells["F3"].Value = "5";
            Sheet.Cells["G3"].Value = "6";
            Sheet.Cells["H3"].Value = "7";
            Sheet.Cells["I3"].Value = "8";
            Sheet.Cells["J3"].Value = "9";
            Sheet.Cells["K3"].Value = "10";
            Sheet.Cells["L3"].Value = "11";
            Sheet.Cells["M3"].Value = "12";
            Sheet.Cells["N3"].Value = "13";
            Sheet.Cells["O3"].Value = "14";
            Sheet.Cells["P3"].Value = "15";
            Sheet.Cells["Q3"].Value = "16";
            Sheet.Cells["R3"].Value = "17";
            Sheet.Cells["S3"].Value = "18";
            Sheet.Cells["T3"].Value = "19";
            Sheet.Cells["U3"].Value = "20";
            Sheet.Cells["V3"].Value = "21";
            Sheet.Cells["W3"].Value = "22";
            Sheet.Cells["X3"].Value = "23";
            Sheet.Cells["Y3"].Value = "24";
            Sheet.Cells["Z3"].Value = "25";
            Sheet.Cells["AA3"].Value = "26";
            Sheet.Cells["AB3"].Value = "27";
            Sheet.Cells["AC3"].Value = "28";
            Sheet.Cells["AD3"].Value = "29";
            Sheet.Cells["AE3"].Value = "30";
            Sheet.Cells["AF3"].Value = "31";
            Sheet.Cells["AG3"].Value = "TotalHours";
            Sheet.Cells["AH3"].Value = "total normalTime";
            Sheet.Cells["AI3"].Value = "TotalOverTime";
            Sheet.Cells["AJ3"].Value = "TotalAbsent";
            Sheet.Cells["AK3"].Value = "TotalVL";
            Sheet.Cells["AL3"].Value = "TotalTransefer";
            Sheet.Cells["AM3"].Value = "TotalSickLeave";
            Sheet.Cells["AN3"].Value = "FridayHours";
            Sheet.Cells["AO3"].Value = "HolidayHours";
            Sheet.Cells["AP3"].Value = "PROJECT NAME";
            long.TryParse(csmps1.ToString(), out var mcs);
            var Msum = this.db.MainTimeSheets.Where(
                y => y.ManPowerSupplier == mcs && y.TMonth.Month == date.Month && y.TMonth.Year == date.Year).ToList();
            var cony = 0;
            var passexel = new List<Attendance>();
            foreach (var sum in Msum)
            {
                listat = this.db.Attendances.Where(x => x.SubMain.Equals(sum.ID)).OrderByDescending(x => x.ID)
                    .ToList();

                foreach (var VA in listat.OrderBy(x => x.ID))
                {
                    if (!passexel.Exists(
                        x => x.MainTimeSheet.ProjectList.ID == VA.MainTimeSheet.ProjectList.ID
                             && x.EmpID == VA.EmpID))
                    {
                        passexel.Add(VA);
                    }
                    else
                    {
                        cony++;
                    }
                }

                pcount = pcount + passexel.Count;
            }

            for (var i = 0; i < passexel.Count; i++)
            {
                var days = new DateTime(date.Year, date.Month, 1);
                for (var j = 1; j < 32; j++)
                {
                    date = days;
                    Sheet.Cells[string.Format("A{0}", row)].Value = passexel[i].LabourMaster.EMPNO;
                    if (date.Day == 1) Sheet.Cells[string.Format("B{0}", row)].Value = passexel[i].C1;

                    if (date.Day == 2) Sheet.Cells[string.Format("C{0}", row)].Value = passexel[i].C2;

                    if (date.Day == 3) Sheet.Cells[string.Format("D{0}", row)].Value = passexel[i].C3;

                    if (date.Day == 4) Sheet.Cells[string.Format("E{0}", row)].Value = passexel[i].C4;

                    if (date.Day == 5) Sheet.Cells[string.Format("F{0}", row)].Value = passexel[i].C5;

                    if (date.Day == 6) Sheet.Cells[string.Format("G{0}", row)].Value = passexel[i].C6;

                    if (date.Day == 7) Sheet.Cells[string.Format("H{0}", row)].Value = passexel[i].C7;

                    if (date.Day == 8) Sheet.Cells[string.Format("I{0}", row)].Value = passexel[i].C8;

                    if (date.Day == 9) Sheet.Cells[string.Format("J{0}", row)].Value = passexel[i].C9;

                    if (date.Day == 10) Sheet.Cells[string.Format("K{0}", row)].Value = passexel[i].C10;

                    if (date.Day == 11) Sheet.Cells[string.Format("L{0}", row)].Value = passexel[i].C11;

                    if (date.Day == 12) Sheet.Cells[string.Format("M{0}", row)].Value = passexel[i].C12;

                    if (date.Day == 13) Sheet.Cells[string.Format("N{0}", row)].Value = passexel[i].C13;

                    if (date.Day == 14) Sheet.Cells[string.Format("O{0}", row)].Value = passexel[i].C14;

                    if (date.Day == 15) Sheet.Cells[string.Format("P{0}", row)].Value = passexel[i].C15;

                    if (date.Day == 16) Sheet.Cells[string.Format("Q{0}", row)].Value = passexel[i].C16;

                    if (date.Day == 17) Sheet.Cells[string.Format("R{0}", row)].Value = passexel[i].C17;

                    if (date.Day == 18) Sheet.Cells[string.Format("S{0}", row)].Value = passexel[i].C18;

                    if (date.Day == 19) Sheet.Cells[string.Format("T{0}", row)].Value = passexel[i].C19;

                    if (date.Day == 20) Sheet.Cells[string.Format("U{0}", row)].Value = passexel[i].C20;

                    if (date.Day == 21) Sheet.Cells[string.Format("V{0}", row)].Value = passexel[i].C21;

                    if (date.Day == 22) Sheet.Cells[string.Format("W{0}", row)].Value = passexel[i].C22;

                    if (date.Day == 23) Sheet.Cells[string.Format("X{0}", row)].Value = passexel[i].C23;

                    if (date.Day == 24) Sheet.Cells[string.Format("Y{0}", row)].Value = passexel[i].C24;

                    if (date.Day == 25) Sheet.Cells[string.Format("Z{0}", row)].Value = passexel[i].C25;

                    if (date.Day == 26) Sheet.Cells[string.Format("AA{0}", row)].Value = passexel[i].C26;

                    if (date.Day == 27) Sheet.Cells[string.Format("AB{0}", row)].Value = passexel[i].C27;

                    if (date.Day == 28) Sheet.Cells[string.Format("AC{0}", row)].Value = passexel[i].C28;

                    if (date.Day == 29) Sheet.Cells[string.Format("AD{0}", row)].Value = passexel[i].C29;

                    if (date.Day == 30) Sheet.Cells[string.Format("AE{0}", row)].Value = passexel[i].C30;

                    if (date.Day == 31) Sheet.Cells[string.Format("AF{0}", row)].Value = passexel[i].C31;

                    Sheet.Cells[string.Format("AG{0}", row)].Value = passexel[i].TotalHours;

                    var a = passexel[i].TotalHours;
                    var b = passexel[i].TotalOverTime;
                    var c = passexel[i].FridayHours;
                    var d = passexel[i].Holidays;
                    if (c == null)
                    {
                        c = 0;
                    }

                    if (d == null)
                    {
                        d = 0;
                    }

                    var a_b = new long?();
                    if (a != null || a != 0)
                    {
                        if (b != null || b != 0) a_b = a - b - c - d;
                        else a_b = a;
                    }
                    else
                    {
                        a_b = 0;
                    }

                    if (passexel[i].TotalOverTime == null)
                    {
                        passexel[i].TotalOverTime = 0;
                    }

                    if (passexel[i].TotalAbsent == null)
                    {
                        passexel[i].TotalAbsent = 0;
                    }

                    if (passexel[i].TotalVL == null)
                    {
                        passexel[i].TotalVL = 0;
                    }

                    if (passexel[i].TotalTransefer == null)
                    {
                        passexel[i].TotalTransefer = 0;
                    }

                    if (passexel[i].TotalSickLeave == null)
                    {
                        passexel[i].TotalSickLeave = 0;
                    }

                    if (passexel[i].FridayHours == null)
                    {
                        passexel[i].FridayHours = 0;
                    }

                    if (passexel[i].Holidays == null)
                    {
                        passexel[i].Holidays = 0;
                    }

                    Sheet.Cells[string.Format("AH{0}", row)].Value = a_b;
                    Sheet.Cells[string.Format("AI{0}", row)].Value = passexel[i].TotalOverTime;
                    Sheet.Cells[string.Format("AJ{0}", row)].Value = passexel[i].TotalAbsent;
                    Sheet.Cells[string.Format("AK{0}", row)].Value = passexel[i].TotalVL;
                    Sheet.Cells[string.Format("AL{0}", row)].Value = passexel[i].TotalTransefer;
                    Sheet.Cells[string.Format("AM{0}", row)].Value = passexel[i].TotalSickLeave;
                    Sheet.Cells[string.Format("AN{0}", row)].Value = passexel[i].FridayHours;
                    Sheet.Cells[string.Format("AO{0}", row)].Value = passexel[i].Holidays;
                    Sheet.Cells[string.Format("AP{0}", row)].Value =
                        passexel[i].MainTimeSheet.ProjectList.PROJECT_NAME;
                    days = days.AddDays(1);
                }

                row++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =Attendances.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }
    }
}