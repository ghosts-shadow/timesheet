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
    [Authorize(Roles = "admin")]
    public class promanlistsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: promanlists
        public ActionResult Index()
        {
            var promanlists = db.promanlists.Include(p => p.ManPowerSupplier1).Include(p => p.ProjectList);
            return View(promanlists.ToList());
        }

        // GET: promanlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promanlist promanlist = db.promanlists.Find(id);
            if (promanlist == null)
            {
                return HttpNotFound();
            }
            return View(promanlist);
        }

        // GET: promanlists/Create
        public ActionResult Create()
        {
            ViewBag.ManPowerSupplier = new SelectList(db.ManPowerSuppliers, "ID", "Supplier");
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            return View();
        }

        // POST: promanlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ManPowerSupplier,Project")] promanlist promanlist)
        {
            if (ModelState.IsValid)
            {
                db.promanlists.Add(promanlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManPowerSupplier = new SelectList(db.ManPowerSuppliers, "ID", "Supplier", promanlist.ManPowerSupplier);
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", promanlist.Project);
            return View(promanlist);
        }

        // GET: promanlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promanlist promanlist = db.promanlists.Find(id);
            if (promanlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManPowerSupplier = new SelectList(db.ManPowerSuppliers, "ID", "Supplier", promanlist.ManPowerSupplier);
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", promanlist.Project);
            return View(promanlist);
        }

        // POST: promanlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ManPowerSupplier,Project")] promanlist promanlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promanlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManPowerSupplier = new SelectList(db.ManPowerSuppliers, "ID", "Supplier", promanlist.ManPowerSupplier);
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", promanlist.Project);
            return View(promanlist);
        }

        // GET: promanlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            promanlist promanlist = db.promanlists.Find(id);
            if (promanlist == null)
            {
                return HttpNotFound();
            }
            return View(promanlist);
        }

        // POST: promanlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            promanlist promanlist = db.promanlists.Find(id);
            db.promanlists.Remove(promanlist);
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
