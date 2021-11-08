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
    [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer")]
    public class ManPowerSuppliersController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: ManPowerSuppliers
        public ActionResult Index()
        {
            return View(db.ManPowerSuppliers.ToList());
        }

        // GET: ManPowerSuppliers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManPowerSupplier manPowerSupplier = db.ManPowerSuppliers.Find(id);
            if (manPowerSupplier == null)
            {
                return HttpNotFound();
            }
            return View(manPowerSupplier);
        }
        [Authorize(Roles = "Admin,logistics_officer")]
        // GET: ManPowerSuppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManPowerSuppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,logistics_officer")]
        public ActionResult Create([Bind(Include = "ID,ShortName,Supplier,SkilledRat,NonSkilledRate,NormalTimeUpto,Crtl,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type")] ManPowerSupplier manPowerSupplier)
        {
            if (ModelState.IsValid)
            {
                db.ManPowerSuppliers.Add(manPowerSupplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manPowerSupplier);
        }

        // GET: ManPowerSuppliers/Edit/5
        [Authorize(Roles = "Admin,logistics_officer")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManPowerSupplier manPowerSupplier = db.ManPowerSuppliers.Find(id);
            if (manPowerSupplier == null)
            {
                return HttpNotFound();
            }
            return View(manPowerSupplier);
        }

        // POST: ManPowerSuppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,logistics_officer")]
        public ActionResult Edit([Bind(Include = "ID,ShortName,Supplier,SkilledRat,NonSkilledRate,NormalTimeUpto,Crtl,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type")] ManPowerSupplier manPowerSupplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manPowerSupplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manPowerSupplier);
        }

        // GET: ManPowerSuppliers/Delete/5
        [Authorize(Roles = "Admin,logistics_officer")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManPowerSupplier manPowerSupplier = db.ManPowerSuppliers.Find(id);
            if (manPowerSupplier == null)
            {
                return HttpNotFound();
            }
            return View(manPowerSupplier);
        }

        // POST: ManPowerSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,logistics_officer")]
        public ActionResult DeleteConfirmed(long id)
        {
            ManPowerSupplier manPowerSupplier = db.ManPowerSuppliers.Find(id);
            db.ManPowerSuppliers.Remove(manPowerSupplier);
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
