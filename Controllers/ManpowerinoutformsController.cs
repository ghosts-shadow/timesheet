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
    public class ManpowerinoutformsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: Manpowerinoutforms
        public ActionResult Index()
        {
            var manpowerinoutforms = db.Manpowerinoutforms.Include(m => m.LabourMaster).Include(m => m.ProjectList);
            return View(manpowerinoutforms.ToList());
        }

        // GET: Manpowerinoutforms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manpowerinoutform manpowerinoutform = db.Manpowerinoutforms.Find(id);
            if (manpowerinoutform == null)
            {
                return HttpNotFound();
            }
            return View(manpowerinoutform);
        }

        // GET: Manpowerinoutforms/Create
        public ActionResult Create()
        {
            ViewBag.camp = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Al Ain",Text = "Al Ain"},
                new SelectListItem(){Value = "Al Mafraq",Text = "Al Mafraq"},
                new SelectListItem(){Value = "Camp Musaffah",Text = "Camp Musaffah"},
                new SelectListItem(){Value = "Al Bahia",Text = "Al Bahia"},
                new SelectListItem(){Value = "Dubai",Text = "Dubai"}
            };
            ViewBag.companyName = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Citiscape",Text = "Citiscape"},
                new SelectListItem(){Value = "Grove",Text = "Grove"},
            };
            ViewBag.EmpID = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "EMPNO");
            ViewBag.Empname = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "Person_Name");
            ViewBag.Position = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "Position");
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            return View();
        }

        // POST: Manpowerinoutforms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Manpowerinoutform manpowerinoutform)
        {
            if (ModelState.IsValid)
            {
                db.Manpowerinoutforms.Add(manpowerinoutform);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.camp = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Al Ain",Text = "Al Ain"},
                new SelectListItem(){Value = "Al Mafraq",Text = "Al Mafraq"},
                new SelectListItem(){Value = "Camp Musaffah",Text = "Camp Musaffah"},
                new SelectListItem(){Value = "Al Bahia",Text = "Al Bahia"},
                new SelectListItem(){Value = "Dubai",Text = "Dubai"}
            };
            ViewBag.companyName = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Citiscape",Text = "Citiscape"},
                new SelectListItem(){Value = "Grove",Text = "Grove"},
            };
            ViewBag.EmpID = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "EMPNO");
            ViewBag.Empname = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "Person_Name");
            ViewBag.Position = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "Position");
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            return View(manpowerinoutform);
        }

        // GET: Manpowerinoutforms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manpowerinoutform manpowerinoutform = db.Manpowerinoutforms.Find(id);
            if (manpowerinoutform == null)
            {
                return HttpNotFound();
            }
            ViewBag.camp = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Al Ain",Text = "Al Ain"},
                new SelectListItem(){Value = "Al Mafraq",Text = "Al Mafraq"},
                new SelectListItem(){Value = "Camp Musaffah",Text = "Camp Musaffah"},
                new SelectListItem(){Value = "Al Bahia",Text = "Al Bahia"},
                new SelectListItem(){Value = "Dubai",Text = "Dubai"}
            };
            ViewBag.EmpID = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "EMPNO", manpowerinoutform.EmpID);
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", manpowerinoutform.Project);
            return View(manpowerinoutform);
        }

        // POST: Manpowerinoutforms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Manpowerinoutform manpowerinoutform)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manpowerinoutform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.camp = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "Al Ain",Text = "Al Ain"},
                new SelectListItem(){Value = "Al Mafraq",Text = "Al Mafraq"},
                new SelectListItem(){Value = "Camp Musaffah",Text = "Camp Musaffah"},
                new SelectListItem(){Value = "Al Bahia",Text = "Al Bahia"},
                new SelectListItem(){Value = "Dubai",Text = "Dubai"}
            };
            ViewBag.EmpID = new SelectList(db.LabourMasters.Where(x=>x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x=>x.EMPNO), "ID", "EMPNO", manpowerinoutform.EmpID);
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", manpowerinoutform.Project);
            return View(manpowerinoutform);
        }

        // GET: Manpowerinoutforms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manpowerinoutform manpowerinoutform = db.Manpowerinoutforms.Find(id);
            if (manpowerinoutform == null)
            {
                return HttpNotFound();
            }
            return View(manpowerinoutform);
        }

        // POST: Manpowerinoutforms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manpowerinoutform manpowerinoutform = db.Manpowerinoutforms.Find(id);
            db.Manpowerinoutforms.Remove(manpowerinoutform);
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
