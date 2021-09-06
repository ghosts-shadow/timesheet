using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    [Authorize(Users = "sdiniz")]
    public class AttendancesController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: Attendances
        public ActionResult Index(DateTime? mtsmonth, long? csmps, long? cspro)
        {
            var attendances = new List<Attendance>();
            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            this.ViewBag.cspro = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");
            if (mtsmonth.HasValue && csmps.HasValue && cspro.HasValue)
            {
                attendances = db.Attendances.Where(x=>x.MainTimeSheet.TMonth.Month == mtsmonth.Value.Month && x.MainTimeSheet.TMonth.Year == mtsmonth.Value.Year && x.MainTimeSheet.Project == cspro && x.MainTimeSheet.ManPowerSupplier == csmps ).Include(a => a.LabourMaster).Include(a => a.MainTimeSheet).ToList();
            }
            
            return View(attendances.OrderBy(x=>x.SubMain).ThenBy(x=>x.LabourMaster.EMPNO));
        }

        // GET: Attendances/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.EmpID = new SelectList(db.LabourMasters, "ID", "VisaSponser");
            ViewBag.SubMain = new SelectList(db.MainTimeSheets, "ID", "Ref");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmpID,SubMain,C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30,C31,TotalHours,TotalOverTime,TotalAbsent,AccommodationDeduction,FoodDeduction,TotalWorkingDays,TotalVL,TotalTransefer,TotalSickLeave,FridayHours,Holidays,ManPowerSupply,CompID,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,xABST,nABST,xOT,nnOT,status,Skilled,Unskilled,Skilledhours,Unskilledhours,Skilledrothours,Unskilledrothours,Skilledfothours,Unskilledfothours,Skilledhothours,Unskilledhothours,Skilledthours,Unskilledthours,TotalWorkers,absapproved_")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Attendances.Add(attendance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpID = new SelectList(db.LabourMasters, "ID", "VisaSponser", attendance.EmpID);
            ViewBag.SubMain = new SelectList(db.MainTimeSheets, "ID", "Ref", attendance.SubMain);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpID = new SelectList(db.LabourMasters, "ID", "VisaSponser", attendance.EmpID);
            ViewBag.SubMain = new SelectList(db.MainTimeSheets, "ID", "Ref", attendance.SubMain);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmpID,SubMain,C1,C2,C3,C4,C5,C6,C7,C8,C9,C10,C11,C12,C13,C14,C15,C16,C17,C18,C19,C20,C21,C22,C23,C24,C25,C26,C27,C28,C29,C30,C31,TotalHours,TotalOverTime,TotalAbsent,AccommodationDeduction,FoodDeduction,TotalWorkingDays,TotalVL,TotalTransefer,TotalSickLeave,FridayHours,Holidays,ManPowerSupply,CompID,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,xABST,nABST,xOT,nnOT,status,Skilled,Unskilled,Skilledhours,Unskilledhours,Skilledrothours,Unskilledrothours,Skilledfothours,Unskilledfothours,Skilledhothours,Unskilledhothours,Skilledthours,Unskilledthours,TotalWorkers,absapproved_")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpID = new SelectList(db.LabourMasters, "ID", "VisaSponser", attendance.EmpID);
            ViewBag.SubMain = new SelectList(db.MainTimeSheets, "ID", "Ref", attendance.SubMain);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Attendance attendance = db.Attendances.Find(id);
            db.Attendances.Remove(attendance);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
