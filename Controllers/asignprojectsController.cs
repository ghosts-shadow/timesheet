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
    [Authorize(Roles = "HR_assist,admin")]
    public class asignprojectsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: asignprojects
        public ActionResult Index()
        {
            var asignprojects = db.asignprojects.Include(a => a.ProjectList).Include(a => a.LabourMaster);
            return View(asignprojects.ToList());
        }

        // GET: asignprojects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asignproject asignproject = db.asignprojects.Find(id);
            if (asignproject == null)
            {
                return HttpNotFound();
            }
            return View(asignproject);
        }
        [Authorize(Roles = "HR_assist")]
        // GET: asignprojects/Create
        public ActionResult Create()
        {
            ViewBag.Project = new SelectList(db.ProjectLists.OrderBy(x=>x.PROJECT_NAME), "ID", "Project_name");
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 9), "ID", "EMPNO");
            return View();
        }

        // POST: asignprojects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR_assist")]
        public ActionResult Create([Bind(Include = "Id,asigneddate,lab_no,Project")] asignproject[] asignproject)
        {
            if (ModelState.IsValid)
            {
                foreach (var asignproject1 in asignproject)
                {
                    var assignlist = db.asignprojects.ToList();
                    if (!assignlist.Exists(x=>x.Project == asignproject1.Project && x.lab_no == asignproject1.lab_no && x.asigneddate == asignproject1.asigneddate))
                    {
                        asignproject1.asigneddate = DateTime.Now.Date;
                        db.asignprojects.Add(asignproject1);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.Project = new SelectList(db.ProjectLists.OrderBy(x => x.PROJECT_NAME), "ID", "Project_name", asignproject[0].Project);
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 9), "ID", "EMPNO", asignproject[0].lab_no);
            return View(asignproject);
        }

        // GET: asignprojects/Edit/5 [Authorize(Roles = "HR_assist,admin")]
        [Authorize(Roles = "HR_assist")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asignproject asignproject = db.asignprojects.Find(id);
            if (asignproject == null)
            {
                return HttpNotFound();
            }
            ViewBag.Project = new SelectList(db.ProjectLists.OrderBy(x => x.PROJECT_NAME), "ID", "Project_name", asignproject.Project);
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 9).OrderBy(x=>x.EMPNO), "ID", "EMPNO", asignproject.lab_no);
            return View(asignproject);
        }

        // POST: asignprojects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR_assist")]
        public ActionResult Edit([Bind(Include = "Id,asigneddate,lab_no,Project")] asignproject asignproject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asignproject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Project = new SelectList(db.ProjectLists.OrderBy(x => x.PROJECT_NAME), "ID", "Project_name", asignproject.Project);
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 9), "ID", "EMPNO", asignproject.lab_no);
            return View(asignproject);
        }

        // GET: asignprojects/Delete/5
        [Authorize(Roles = "HR_assist")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asignproject asignproject = db.asignprojects.Find(id);
            if (asignproject == null)
            {
                return HttpNotFound();
            }
            return View(asignproject);
        }

        // POST: asignprojects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR_assist")]
        public ActionResult DeleteConfirmed(int id)
        {
            asignproject asignproject = db.asignprojects.Find(id);
            db.asignprojects.Remove(asignproject);
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
