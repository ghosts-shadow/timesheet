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
    [Authorize]
    public class weekendlistsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();
        SelectListItem[] weekends = {
            new SelectListItem {Text = "Friday", Value = "5"},
            new SelectListItem {Text = "Saturday", Value = "6"},
            new SelectListItem {Text = "Sunday", Value = "0"},
        };
        // GET: weekendlists
        public ActionResult Index()
        {
            var weekendlists = new List<weekendlist>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = db.ProjectLists.ToList();
            }

            foreach (var list in t)
            {
                weekendlists.AddRange(db.weekendlists.ToList().FindAll(x=>x.project_id == list.ID));
            }
            
            return View(weekendlists);
        }

        // GET: weekendlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            weekendlist weekendlist = db.weekendlists.Find(id);
            if (weekendlist == null)
            {
                return HttpNotFound();
            }
            return View(weekendlist);
        }

        // GET: weekendlists/Create
        public ActionResult Create()
        {
            var weekendlists = new List<weekendlist>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = db.ProjectLists.ToList();
            }

            ViewBag.project_id = new SelectList(t, "ID", "PROJECT_NAME");
            ViewBag.weekend = weekends;
            return View();
        }

        // POST: weekendlists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,weekend,project_id")] weekendlist weekendlist)
        {
            if (ModelState.IsValid)
            {
                db.weekendlists.Add(weekendlist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var weekendlists = new List<weekendlist>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = db.ProjectLists.ToList();
            }

            ViewBag.project_id = new SelectList(t, "ID", "PROJECT_NAME", weekendlist.project_id);
            int.TryParse(weekendlist.weekend, out var weekday);
            ViewBag.weekend = new SelectList(weekends, weekday);
            return View(weekendlist);
        }

        // GET: weekendlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            weekendlist weekendlist = db.weekendlists.Find(id);
            if (weekendlist == null)
            {
                return HttpNotFound();
            }

            var weekendlists = new List<weekendlist>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = db.ProjectLists.ToList();
            }

            ViewBag.project_id = new SelectList(t, "ID", "PROJECT_NAME", weekendlist.project_id);
            ViewBag.weekend = weekends;
            return View(weekendlist);
        }

        // POST: weekendlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,weekend,project_id")] weekendlist weekendlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weekendlist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var weekendlists = new List<weekendlist>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = db.ProjectLists.ToList();
            }

            ViewBag.project_id = new SelectList(t, "ID", "PROJECT_NAME", weekendlist.project_id);
            int.TryParse(weekendlist.weekend, out var weekday);
            ViewBag.weekend = new SelectList(weekends,weekday);
            return View(weekendlist);
        }

        // GET: weekendlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            weekendlist weekendlist = db.weekendlists.Find(id);
            if (weekendlist == null)
            {
                return HttpNotFound();
            }
            return View(weekendlist);
        }

        // POST: weekendlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            weekendlist weekendlist = db.weekendlists.Find(id);
            db.weekendlists.Remove(weekendlist);
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
