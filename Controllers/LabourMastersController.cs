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
    using Microsoft.Ajax.Utilities;

    [Authorize]
    public class LabourMastersController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: LabourMasters
        public ActionResult Index()
        {
            
            return View(db.LabourMasters.ToList());
        }

        // GET: LabourMasters/Create
        public ActionResult Create()
        {
            var po = this.db.LabourMasters.OrderBy(x=>x.ID).ToList();
            var item = new List<string>();
            foreach (var la in po)
            {
                if (!la.Position.IsNullOrWhiteSpace())
                {
                    if (!item.Exists(x => x.Contains(la.Position)))
                    {
                        item.Add(la.Position);
                    }
                }
            }
            var item2 = new List<string>();
            foreach (var la in po)
            {
                if (!la.Nationality.IsNullOrWhiteSpace())
                {
                    if (!item2.Exists(x => x.Contains(la.Nationality)))
                    {
                        item2.Add(la.Nationality);
                    }
                }
            }

            ViewBag.Nationality = new SelectList(item2);
            ViewBag.Position = new SelectList(item);
            ViewBag.ManPowerSupply = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EMPNO,ManPowerSupply,VisaSponser,Person_Name,Position,Nationality,UID_No,ResidenceVisaExpiry,LaborCardNumber,LaborCardExpiry,PassportNo,Passportexpiry,EID,EIDExpiry,Status,Photo,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type")] LabourMaster labourMaster)
        {
            var po = this.db.LabourMasters.ToList();
            var item = new List<string>();
            foreach (var la in po)
            {
                if (item.Exists(x => x.Contains(la.Position)))
                {
                    item.Add(la.Position);
                }
            }

            var item2 = new List<string>();
            foreach (var la in po)
            {
                if (!la.Nationality.IsNullOrWhiteSpace())
                {
                    if (!item2.Exists(x => x.Contains(la.Nationality)))
                    {
                        item2.Add(la.Nationality);
                    }
                }
            }

            ViewBag.Nationality = new SelectList(item2);
            ViewBag.Position = new SelectList(item);
            ViewBag.ManPowerSupply = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            if (ModelState.IsValid)
            {
                if (!po.Exists(x=>x.EMPNO == labourMaster.EMPNO && x.ManPowerSupply == labourMaster.ManPowerSupply))
                {
                    labourMaster.ID = po.Last().ID + 1;
                    db.LabourMasters.Add(labourMaster);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "enployee no "+ labourMaster.EMPNO +" all ready exists ");
                    return this.View(labourMaster);
                }
                return RedirectToAction("Create");
            }
            return View(labourMaster);
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
