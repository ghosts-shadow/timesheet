// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjmanController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProjmanController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

    using onlygodknows.Models;

    public class ProjmanController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        private int i = 0;
        public ActionResult PMapproval()
        {
            var t = new List<ProjectList>();
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList(); 
                t = new List<ProjectList>();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }
            var pl=new List<ProjectList>();
            var t1 = t.First();
            long f1 = t1.ID;
            foreach (var v in t)
            {
                
                if (v.ID == f1)
                {
                    pl.Add(v);
                }
            }
            var pl1 = pl.First();
            var ap1 = new List<approval>();
            ap1 = this.db.approvals.Where(x => x.P_id==pl1.ID && x.status== "submitted").ToList();
            var ap2 = ap1.Where(x => x.adate == ap1.First().adate && x.MPS_id== ap1.First().MPS_id);
            if (ap1.Count>0)
            {
                 ViewBag.csdate = ap2.First().adate.Value.ToLongDateString();
                ViewBag.csmsp = this.db.ManPowerSuppliers.Find(ap2.First().MPS_id).Supplier;
                ViewBag.csp = this.db.ProjectLists.Find(ap2.First().P_id).PROJECT_NAME;
                ViewBag.suser = ap2.First().Susername;
            }
            return View(ap2);
        }

        public ActionResult approved()
        {
            var t = new List<ProjectList>();
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                t = new List<ProjectList>();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var pl = new List<ProjectList>();
            var t1 = t.First();
            long f1 = t1.ID;
            foreach (var v in t)
            {

                if (v.ID == f1)
                {
                    pl.Add(v);
                }
            }

            var pl1 = pl.First();
            var ap1 = new List<approval>();
            ap1 = this.db.approvals.Where(x => x.P_id == pl1.ID && x.status == "submitted").ToList();
            var ap2 = ap1.Where(x => x.adate == ap1.First().adate && x.MPS_id == ap1.First().MPS_id);
            foreach (var approval in ap2)
            {
                approval.status = "approved";
                approval.Ausername = User.Identity.Name;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            return RedirectToAction("PMapproval");
        }

        public ActionResult rejected(string  why)
        {
            var t = new List<ProjectList>();
            var uid = User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                t = new List<ProjectList>();
                foreach (var i in scid)
                {
                    t.Add(this.db.ProjectLists.Find(i.Project));
                }
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var pl = new List<ProjectList>();
            var t1 = t.First();
            long f1 = t1.ID;
            foreach (var v in t)
            {

                if (v.ID == f1)
                {
                    pl.Add(v);
                }
            }

            var pl1 = pl.First();
            var ap1 = new List<approval>();
            ap1 = this.db.approvals.Where(x => x.P_id == pl1.ID && x.status == "submitted").ToList();
            var ap2 = ap1.Where(x => x.adate == ap1.First().adate && x.MPS_id == ap1.First().MPS_id);
            foreach (var approval in ap2)
            {
                approval.status = "rejected for " + why;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            return RedirectToAction("PMapproval");
        }
    }
}
//10pods