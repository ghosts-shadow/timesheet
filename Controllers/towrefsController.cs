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
    using System.IO;
    using PagedList;

    [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer,Admin_View")]
    public class towrefsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: towrefs
        public ActionResult Index()
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            var towrlist = db.towrefs.ToList();
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("logistics_officer") || this.User.IsInRole("Admin_View")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var towrefs = new List<towref>();
            var towrefs1 = new List<towref>();
            foreach (var list in t)
            {
                var trlist = towrlist.FindAll(x => x.mp_to == list.ID);
                var trlist1 = towrlist.FindAll(x => x.mp_from == list.ID);
                foreach (var towref in trlist)
                {
                    towref.AR = true;
                    towrefs.Add(towref);
                }

                towrefs.AddRange(trlist1);
            }

            foreach (var tw in towrefs)
            {
                // if (tw.towemps.Count == 0)
                // {
                //     var torfind = db.towrefs.ToList().Find(x=>x.Id == tw.Id);
                //     if (torfind != null)
                //     {
                //         db.towrefs.Remove(torfind);
                //         db.SaveChanges();
                //     }
                // }
                // else
                {
                    if (!towrefs1.Exists(x=>x.Id == tw.Id) && tw.towemps.Count != 0)
                    {
                        towrefs1.Add(tw);
                    }
                    
                }
            }
            //
            // if (Request.IsAjaxRequest())
            // {
            //     return PartialView(towrefs1.FindAll(x => x.towemps.First().app_by != "default").OrderByDescending(x=>x.mpcdate).ToPagedList(1, 100));
            // }

            return View(towrefs1.FindAll(x=>x.towemps.First().app_by != "default").OrderByDescending(x => x.mpcdate).ToPagedList(1, 100));
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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult Create()
        {
            var prolist = db.ProjectLists.ToList();
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
                t = this.db.ProjectLists.ToList();
            }

            ViewBag.mp_from = new SelectList(t.OrderBy(x=>x.PROJECT_NAME).ToList(), "ID", "PROJECT_NAME");
            if (t.Count == 1)
            {
                foreach (var list in t)
                {
                    prolist.Remove(list);
                }
            }

            ViewBag.mp_to = new SelectList(prolist.OrderBy(x => x.PROJECT_NAME).ToList(), "ID", "PROJECT_NAME");
            return View();
        }

        // POST: towrefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult Create([Bind(Include = "Id,refe1,mp_to,mp_from,R_no,mpcdate")]
            towref towref, HttpPostedFileBase postedFile)
        {
            var checktow = this.db.towrefs.ToList();
            if (checktow.Exists(x => x.R_no == towref.R_no || x.refe1 == towref.refe1))
            {
                var emptrext = checktow.Find(x => x.R_no == towref.R_no || x.refe1 == towref.refe1);
                if (emptrext.towemps.Count == 0)
                {
                    db.towrefs.Remove(emptrext);
                    db.SaveChanges();
                    goto renew;
                }
                ModelState.AddModelError("refe1", " already exists");
                ModelState.AddModelError("R_no", " already exists");
                if (towref.mp_from == towref.mp_to)
                {
                    ModelState.AddModelError("mp_to", "please select different projects");
                }

                
                goto ass;
            }

            if (towref.mp_from == towref.mp_to)
            {
                ModelState.AddModelError("mp_to", "please select different projects");
                goto ass;
            }
            renew: ;
            if (ModelState.IsValid)
            {
                towref.mpcdate = DateTime.Now;
                db.towrefs.Add(towref);
                db.SaveChanges();
                if (postedFile != null)
                {
                    var postedFileExtension = Path.GetExtension(postedFile.FileName);
                    if (string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(
                            postedFileExtension,
                            ".pdf",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] bytes;
                        using (var br = new BinaryReader(postedFile.InputStream))
                        {
                            bytes = br.ReadBytes(postedFile.ContentLength);
                        }

                        var idlast = this.db.towrefs.OrderBy(x => x.Id).ToList().Last();
                        var tq = new trattfile();
                        tq.towref = idlast;
                        tq.content_type = postedFile.ContentType;
                        tq.data = bytes;
                        tq.name = Path.GetFileName(postedFile.FileName);
                        tq.rowref = idlast.Id + 1;
                        this.db.trattfiles.Add(tq);
                        this.db.SaveChanges();
                    }
                }

                var towf = this.db.towrefs.ToList().Last();
                return RedirectToAction("Create", "towemps", towf);
            }

            ass: ;

            var prolist = db.ProjectLists.ToList();
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
                t = this.db.ProjectLists.ToList();
            }

            ViewBag.mp_from = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Value);
            if (t.Count == 1)
            {
                foreach (var list in t)
                {
                    prolist.Remove(list);
                }
            }

            ViewBag.mp_to = new SelectList(prolist, "ID", "PROJECT_NAME").OrderBy(x => x.Value);
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

            var prolist = db.ProjectLists.ToList();
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
                t = this.db.ProjectLists.ToList();
            }

            ViewBag.mp_from = new SelectList(t, "ID", "PROJECT_NAME");
            foreach (var list in t)
            {
                prolist.Remove(list);
            }

            ViewBag.mp_to = new SelectList(prolist, "ID", "PROJECT_NAME");
            return View(towref);
        }

        // POST: towrefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,refe1,mp_to,mp_from,R_no,mpcdate")]
            towref towref)
        {
            if (ModelState.IsValid)
            {
                db.Entry(towref).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var prolist = db.ProjectLists.ToList();
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
                t = this.db.ProjectLists.ToList();
            }

            ViewBag.mp_from = new SelectList(t, "ID", "PROJECT_NAME");
            foreach (var list in t)
            {
                prolist.Remove(list);
            }

            ViewBag.mp_to = new SelectList(prolist, "ID", "PROJECT_NAME");
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