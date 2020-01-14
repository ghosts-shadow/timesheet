using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    using System.Diagnostics.CodeAnalysis;

    public class testController : Controller
    {
        // GET: test
        public LogisticsSoftEntities db = new LogisticsSoftEntities();
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
            this.ViewBag.mdate = aa.TMonth.ToString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.empno = new SelectList(d.OrderBy(m => m.EMPNO), "ID", "EMPNO");

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
                               new SelectListItem { Text = "S", Value = "0" },
                               new SelectListItem { Text = "A", Value = "0" },
                               new SelectListItem { Text = "T", Value = "0" },
                               new SelectListItem { Text = "V", Value = "0" }
                           };
            ViewBag.hours = data;
            return this.View();
        }
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            this.ViewBag.mdate = aa.TMonth.ToString();
            var d = from LabourMaster in this.db.LabourMasters
                    where LabourMaster.ManPowerSupply == b.ID
                    select LabourMaster;
            this.ViewBag.empno = new SelectList(d.OrderBy(m => m.EMPNO), "ID", "EMPNO");

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
                               new SelectListItem { Text = "S", Value = "0" },
                               new SelectListItem { Text = "A", Value = "0" },
                               new SelectListItem { Text = "T", Value = "0" },
                               new SelectListItem { Text = "V", Value = "0" }
                           };
            ViewBag.hours = data;
            if (this.ModelState.IsValid)
            {
                for (var i = 0; i < test.Tests.Count; i++)
                {
                    at.EmpID = test.Tests[i].empno;
                    at.SubMain = aa.ID;
                    if (test.Tests[i].date.Day == 1) at.C1 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 2) at.C2 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 3) at.C3 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 4) at.C4 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 5) at.C5 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 6) at.C6 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 7) at.C7 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 8) at.C8 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 9) at.C9 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 10) at.C10 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 11) at.C11 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 12) at.C12 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 13) at.C13 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 14) at.C14 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 15) at.C15 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 16) at.C16 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 17) at.C17 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 18) at.C18 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 19) at.C19 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 20) at.C20 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 21) at.C21 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 22) at.C22 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 23) at.C23 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 24) at.C24 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 25) at.C25 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 26) at.C26 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 27) at.C27 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 28) at.C28 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 29) at.C29 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 30) at.C30 = test.Tests[i].hours;
                    if (test.Tests[i].date.Day == 31) at.C31 = test.Tests[i].hours;
                    this.db.Attendances.Add(at);
                    this.db.SaveChanges(); return this.RedirectToAction("MCreate","Home");
                }
            }

            return this.View(test);
        }
    }
}