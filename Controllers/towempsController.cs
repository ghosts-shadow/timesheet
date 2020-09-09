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
    public class towempsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();
        // GET: towemps
        public ActionResult Index(int? id)
        {
            var towemps = db.towemps.Include(t => t.LabourMaster).Include(t => t.towref).ToList();
            var tr = this.db.towrefs.Find(id);
            ViewBag.form = tr.ProjectList.PROJECT_NAME;
            ViewBag.to = tr.ProjectList1.PROJECT_NAME;
            ViewBag.R_no = tr.R_no;
            ViewBag.refe1 = tr.refe1;
            ViewBag.mpcdate = tr.mpcdate;
            return View(towemps.FindAll(x => x.rowref == id));
        }

        // GET: towemps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            return View(towemp);
        }
        
        // GET: towemps/Create
        public ActionResult Create(towref tw1)
        {
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO");
            TempData["mydata"] = tw1;
            return View();
        }

        // POST: towemps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,lab_no,effectivedate,rowref")] towemp[] towemp)
        {
            towref tw = TempData["mydata"] as towref;if (ModelState.IsValid)
            {
                foreach (var towemp1 in towemp)
                {
                    towemp1.rowref = tw.Id;
                    db.towemps.Add(towemp1);
                    db.SaveChanges();
                }  
                return RedirectToAction("Index","towrefs");
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", towemp[0].lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp[0].rowref);
            return View(towemp[1]);
        }

        // GET: towemps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", towemp.lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp.rowref);
            return View(towemp);
        }

        // POST: towemps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,lab_no,effectivedate,rowref")] towemp towemp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(towemp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", towemp.lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp.rowref);
            return View(towemp);
        }

        // GET: towemps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            return View(towemp);
        }

        // POST: towemps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            towemp towemp = db.towemps.Find(id);
            db.towemps.Remove(towemp);
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
