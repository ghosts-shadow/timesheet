﻿using System;
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
    [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer,Admin_View")]
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
        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer")]
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
        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer")]
        public ActionResult Create( Manpowerinoutform manpowerinoutform)
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
            ViewBag.EmpID = new SelectList(db.LabourMasters.Where(x => x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x => x.EMPNO), "ID", "EMPNO");
            ViewBag.Empname = new SelectList(db.LabourMasters.Where(x => x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x => x.EMPNO), "ID", "Person_Name");
            ViewBag.Position = new SelectList(db.LabourMasters.Where(x => x.ManPowerSupply == 1 || x.ManPowerSupply == 8).OrderBy(x => x.EMPNO), "ID", "Position");
            ViewBag.Project = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME");
            if (ModelState.IsValid)
            {
                if (manpowerinoutform.check_out != null)
                {
                    var minoutlist = db.Manpowerinoutforms.Where(x => x.EmpID == manpowerinoutform.EmpID && x.Project == manpowerinoutform.Project && x.check_out == null).OrderByDescending(x => x.date_).ToList();
                    if (minoutlist.Count != 0)
                    {
                        var minoutcheck = minoutlist.First();
                        minoutcheck.check_out = manpowerinoutform.check_out;
                        db.Entry(minoutcheck).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("check_out", "add check_in first");
                        return View(manpowerinoutform);
                    }
                }
                else
                {
                    db.Manpowerinoutforms.Add(manpowerinoutform);
                    db.SaveChanges();
                }
                return RedirectToAction("Create");
            }
            return View(manpowerinoutform);
        }

        // GET: Manpowerinoutforms/Edit/5
        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager")]
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
        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager")]
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

        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager")]
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
        [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager")]
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
