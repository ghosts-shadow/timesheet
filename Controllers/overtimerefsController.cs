using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    public class overtimerefsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: overtimerefs
        [Authorize(Roles = "Admin,Head_of_projects(citiscape),Head_of_projects(grove),HR_manager,Project_manager,logistics_officer,Admin_View")]
        public ActionResult Index()
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            var towrlist = db.towrefs.ToList();
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") ||
                                    this.User.IsInRole("Head_of_projects(citiscape)") ||
                                    this.User.IsInRole("Head_of_projects(grove)")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                var projectlist = new List<ProjectList>();
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole("Head_of_projects(grove)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by != null && x.excute_by == "grove").ToList();
                    }
                    else if (User.IsInRole("Head_of_projects(citiscape)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by == null || x.excute_by == "citiscape").ToList();
                    }

                }

                t = projectlist;
            }

            var overtimerefs = new List<overtimeref>();
            foreach (var list in t)
            {
                var ov = db.overtimerefs.Where(x => x.overtimepro == list.ID).ToList();
                var reap = ov.FindAll(x =>x.overtimeemployeelists.Count > 0 && x.overtimeemployeelists.First().status == null);
                if (reap != null && reap.Any())
                {
                    overtimerefs.AddRange(reap);
                }
            }
            return View(overtimerefs.OrderByDescending(x=>x.overtimedate).ToList());
        }

        // GET: overtimerefs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeref overtimeref = db.overtimerefs.Find(id);
            if (overtimeref == null)
            {
                return HttpNotFound();
            }

            return View(overtimeref);
        }

        [Authorize(Roles = "Project_manager,Head_of_projects(citiscape),Head_of_projects(grove),Admin")]
        // GET: overtimerefs/Create
        public ActionResult Create()
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            var towrlist = db.towrefs.ToList();
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") || this.User.IsInRole("Head_of_projects(citiscape)") || this.User.IsInRole("Head_of_projects(grove)")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                var projectlist = new List<ProjectList>();
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole("Head_of_projects(grove)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by != null && x.excute_by == "grove").ToList();
                    }
                    else if (User.IsInRole("Head_of_projects(citiscape)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by == null || x.excute_by == "citiscape").ToList();
                    }

                }

                t = projectlist;
            }

            ViewBag.overtimepro = new SelectList(t.OrderBy(x=>x.PROJECT_NAME), "ID", "PROJECT_NAME");
            return View();
        }

        // POST: overtimerefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project_manager,Head_of_projects(citiscape),Head_of_projects(grove),Admin")]
        public ActionResult Create(overtimeref overtimeref)
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            var towrlist = db.towrefs.ToList();
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager") || this.User.IsInRole("Head_of_projects(citiscape)") || this.User.IsInRole("Head_of_projects(grove)")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                var projectlist = new List<ProjectList>();
                if (Request.IsAuthenticated)
                {
                    if (User.IsInRole("Head_of_projects(grove)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by != null && x.excute_by == "grove").ToList();
                    }
                    else if (User.IsInRole("Head_of_projects(citiscape)"))
                    {
                        projectlist = db.ProjectLists.Where(x => x.excute_by == null || x.excute_by == "citiscape").ToList();
                    }

                }

                t = projectlist;
            }

            if (ModelState.IsValid)
            {
                var otcheck = db.overtimerefs.ToList();
                if (!otcheck.Exists(x => x.overtimepro == overtimeref.overtimepro &&
                    x.overtimeref1 == overtimeref.overtimeref1))
                {
                    overtimeref.overtimedate = DateTime.Now;
                    db.overtimerefs.Add(overtimeref);
                    db.SaveChanges();
                    return RedirectToAction("Create", "overtimeemployeelists", overtimeref);
                }
                else
                {
                    ModelState.AddModelError("overtimeref1", "combination already exist");
                }
            }

            ViewBag.overtimepro = new SelectList(t, "ID", "PROJECT_NAME", overtimeref.overtimepro);
            return View(overtimeref);
        }

        // GET: overtimerefs/Edit/5
        [Authorize(Roles = "Project_manager,Head_of_projects(citiscape),Head_of_projects(grove)")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeref overtimeref = db.overtimerefs.Find(id);
            if (overtimeref == null)
            {
                return HttpNotFound();
            }

            ViewBag.overtimepro = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", overtimeref.overtimepro);
            return View(overtimeref);
        }

        // POST: overtimerefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project_manager,Head_of_projects(citiscape),Head_of_projects(grove)")]
        public ActionResult Edit([Bind(Include = "Id,overtimedate,overtimeref1,overtimepro")]
            overtimeref overtimeref)
        {
            if (ModelState.IsValid)
            {
                db.Entry(overtimeref).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.overtimepro = new SelectList(db.ProjectLists, "ID", "PROJECT_NAME", overtimeref.overtimepro);
            return View(overtimeref);
        }

        // GET: overtimerefs/Delete/5
        [Authorize(Roles = "Project_manager,Head_of_projects(citiscape),Head_of_projects(grove)")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeref overtimeref = db.overtimerefs.Find(id);
            if (overtimeref == null)
            {
                return HttpNotFound();
            }

            return View(overtimeref);
        }

        // POST: overtimerefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            overtimeref overtimeref = db.overtimerefs.Find(id);
            if (this.User.IsInRole("Project_manager") || this.User.IsInRole("Head_of_projects(citiscape)") || this.User.IsInRole("Head_of_projects(grove)"))
            {
                db.overtimerefs.Remove(overtimeref);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(overtimeref);
            }
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