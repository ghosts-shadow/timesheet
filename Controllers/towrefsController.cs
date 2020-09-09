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
    using PagedList;

    public class towrefsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: towrefs
        public ActionResult Index()
        {
            var towrefs = db.towrefs.Include(t => t.ProjectList).Include(t => t.ProjectList1).ToList();
            return View(towrefs.ToPagedList(1,100));
        }

        // GET: towrefs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towref towref = db.towrefs.Find(id);
            if (towref == null)
            {
                return HttpNotFound();
            }
            return View(towref);
        }

        // GET: towrefs/Create
        public ActionResult Create()
        {
            ViewBag.mp_to = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            ViewBag.mp_from = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            return View();
        }

        // POST: towrefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,refe1,mp_to,mp_from,R_no,mpcdate")] towref towref)
        {
            if (ModelState.IsValid)
            {
                db.towrefs.Add(towref);
                db.SaveChanges();
                var towf = this.db.towrefs.ToList().Last();
                return RedirectToAction("Create","towemps",towf);
            }

            ViewBag.mp_to = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_to);
            ViewBag.mp_from = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_from);
            return View(towref);
        }

        // GET: towrefs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towref towref = db.towrefs.Find(id);
            if (towref == null)
            {
                return HttpNotFound();
            }
            ViewBag.mp_to = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_to);
            ViewBag.mp_from = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_from);
            return View(towref);
        }

        // POST: towrefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,refe1,mp_to,mp_from,R_no,mpcdate")] towref towref)
        {
            if (ModelState.IsValid)
            {
                db.Entry(towref).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.mp_to = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_to);
            ViewBag.mp_from = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", towref.mp_from);
            return View(towref);
        }

        // GET: towrefs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towref towref = db.towrefs.Find(id);
            if (towref == null)
            {
                return HttpNotFound();
            }
            return View(towref);
        }

        // POST: towrefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            towref towref = db.towrefs.Find(id);
            db.towrefs.Remove(towref);
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
