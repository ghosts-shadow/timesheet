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

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;

    using onlygodknows.Models;

    public class ProjmanController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        private int i = 0;
        public ActionResult PMapproval( long? manPower,long? pro, DateTime? mtsmonth2)
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

            if (manPower.HasValue)
            {
                TempData["mydata1"] = manPower;
            }

            if (mtsmonth2.HasValue)
            {
                TempData["mydata"] = mtsmonth2;
            }

            if (pro.HasValue)
            {
                TempData["mydata2"] = pro;
            }
            this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME");
            ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers,"ID", "Supplier");
            if (mtsmonth2.HasValue)
            {
                ViewBag.csdate = mtsmonth2.Value.ToLongDateString();
            }
            var ap2 = new List<approval>();
            ap2= this.db.approvals.Where( x => x.P_id == pro && x.adate == mtsmonth2 && x.MPS_id == manPower && x.status == "submitted").ToList();
            {
                /*
            var pl=new List<ProjectList>();
            var apall = this.db.approvals.ToList();
            foreach (var v in t)
            {
                pl.Add(v);
            }
            if (pl.Count>0)
            {
                var pl1 = pl.First();
                var ap1 = new List<approval>();
                ap1 = this.db.approvals.Where(x => x.P_id==pl1.ID && x.status== "submitted").ToList();
                foreach (var mp in ap1)
                {
                    ap2 = ap1.Where(x => x.adate == mp.adate && x.MPS_id== mp.MPS_id).ToList();
                    if (ap1.Count>0)
                    {
                        ViewBag.csdate = mp.adate.Value.ToLongDateString();
                        ViewBag.csmsp = this.db.ManPowerSuppliers.Find(ap2.First().MPS_id).Supplier;
                        ViewBag.csp = this.db.ProjectLists.Find(ap2.First().P_id).PROJECT_NAME;
                        ViewBag.suser = ap2.First().Susername;
                    }
                }
                
                return View(ap2);
            }*/
            }
            if (ap2.Count>0)
            {

            ViewBag.suser = ap2.First().Susername;
                
            }
            return View(ap2);
        }
        [Authorize(Roles = "Project_manager")]
        public ActionResult approved()
        {
            /*var t = new List<ProjectList>();
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
            ap1 = this.db.approvals.Where(x => x.P_id == pl1.ID && x.status == "submitted").ToList();*/
            DateTime da=new DateTime();
            long p=0, mp = 0;
            
            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);
            
            long.TryParse(this.TempData["mydata2"].ToString(), out p);
            
            long.TryParse(this.TempData["mydata1"].ToString(), out mp);
            
            var ap2 = this.db.approvals.Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status=="submitted").ToList();
            foreach (var approval in ap2)
            {
                approval.status = "approved";
                approval.Ausername = User.Identity.Name;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            return RedirectToAction("PMapproval");
        }

        [Authorize(Roles = "Project_manager")]
        public ActionResult rejected(string  why)
        {
            DateTime da = new DateTime();
            long p=0, mp = 0;

            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);

            long.TryParse(this.TempData["mydata2"].ToString(), out p);

            long.TryParse(this.TempData["mydata1"].ToString(), out mp);

            var ap2 = this.db.approvals.Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
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