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
    using OfficeOpenXml;

    public class towempsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();
        // GET: towemps
        public ActionResult Index(int? id)
        {
            var towemps = db.towemps.Include(t => t.LabourMaster).Include(t => t.towref).ToList();
            var tr = this.db.towrefs.Find(id);
            ViewBag.tw = tr.Id;
            ViewBag.form = tr.ProjectList1.PROJECT_NAME;
            ViewBag.to = tr.ProjectList.PROJECT_NAME;
            ViewBag.R_no = tr.R_no;
            ViewBag.refe1 = tr.refe1;
            ViewBag.mpcdate = tr.mpcdate;
            return View(towemps.FindAll(x => x.rowref == id));
        }

        // GET: towemps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            return View(towemp);
        }
        
        // GET: towemps/Create
        public ActionResult Create(towref tw1)
        {
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x=>x.EMPNO >3).OrderBy(x=>x.EMPNO), "ID", "EMPNO");
            ViewBag.lab_name = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x=>x.EMPNO), "ID", "Person_Name");
            ViewBag.lab_position = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x=>x.EMPNO), "ID", "Position");
            ViewBag.lab_mps = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x=>x.EMPNO), "ID", "ManPowerSupply");
            var tr = this.db.towrefs.Find(tw1.Id);
            ViewBag.tw = tr.Id;
            ViewBag.form = tr.ProjectList1.PROJECT_NAME;
            ViewBag.to = tr.ProjectList.PROJECT_NAME;
            ViewBag.R_no = tr.R_no;
            ViewBag.refe1 = tr.refe1;
            ViewBag.mpcdate = tr.mpcdate;
            TempData["mydata"] = tw1;
            return View();
        }

        // POST: towemps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,lab_no,effectivedate,rowref")] towemp[] towemp)
        {
            towref tw = TempData["mydata"] as towref;
            if (ModelState.IsValid)
            {
                var i = 0;
                foreach (var towemp1 in towemp)
                {
                    if (i==0)
                    {
                    towemp1.rowref = tw.Id;
                        db.towemps.Add(towemp1);
                        db.SaveChanges();
                    }
                    if (towemp[i].lab_no != towemp1.lab_no)
                    {
                    db.towemps.Add(towemp1);
                    db.SaveChanges();
                    }
                    i++;
                }  
                return RedirectToAction("Index","towrefs");
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x => x.EMPNO), "ID", "EMPNO", towemp[0].lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp[0].rowref);
            ViewBag.lab_name = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x => x.EMPNO), "ID", "Person_Name");
            ViewBag.lab_position = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x => x.EMPNO), "ID", "Position");
            ViewBag.lab_mps = new SelectList(db.LabourMasters.Where(x => x.EMPNO > 3).OrderBy(x => x.EMPNO), "ID", "ManPowerSupply");
            return View(towemp[1]);
        }

        // GET: towemps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", towemp.lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp.rowref);
            return View(towemp);
        }

        // POST: towemps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,lab_no,effectivedate,rowref")] towemp towemp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(towemp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", towemp.lab_no);
            ViewBag.rowref = new SelectList(db.towrefs, "Id", "Id", towemp.rowref);
            return View(towemp);
        }

        // GET: towemps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            towemp towemp = db.towemps.Find(id);
            if (towemp == null)
            {
                return HttpNotFound();
            }
            return View(towemp);
        }

        // POST: towemps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            towemp towemp = db.towemps.Find(id);
            db.towemps.Remove(towemp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public void DownloadExcel(int tr)
        {
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("transferred_workers");
            var towemps = db.towemps.Include(t => t.LabourMaster).Include(t => t.towref).ToList();
            var tw = this.db.towrefs.Find(tr);
            Sheet.Cells["A1"].Value = "from";
            Sheet.Cells["B1"].Value = "to";
            Sheet.Cells["C1"].Value = "R no";
            Sheet.Cells["D1"].Value = "ref";
            Sheet.Cells["E1"].Value = "date";
            Sheet.Cells["A2"].Value = tw.ProjectList.PROJECT_NAME;
            Sheet.Cells["B2"].Value = tw.ProjectList1.PROJECT_NAME;
            Sheet.Cells["C2"].Value = tw.R_no;
            Sheet.Cells["D2"].Value = tw.refe1;
            var as2 = "";
            if (tw.mpcdate != null)
            {
                var as1 = tw.mpcdate.ToString();
                as2 = as1.Remove(tw.mpcdate.ToString().IndexOf(" "), 12);
            }

            Sheet.Cells["E2"].Value = as2;
            Sheet.Cells["A4"].Value = "effectivedate";
            Sheet.Cells["B4"].Value = "EMPNO";
            Sheet.Cells["C4"].Value = "name";
            Sheet.Cells["D4"].Value = "Position";
            Sheet.Cells["E4"].Value = "ManPowerSupply";
            var i = 5;
            foreach (var tw1 in towemps.FindAll(x => x.rowref == tw.Id))
            {
                as2 = "";
                if (tw1.effectivedate != null)
                {
                    var as1 = tw1.effectivedate.ToString();
                    as2 = as1.Remove(tw1.effectivedate.ToString().IndexOf(" "), 12);
                }
                Sheet.Cells[string.Format("A{0}", i)].Value = as2;
                Sheet.Cells[string.Format("B{0}", i)].Value = tw1.LabourMaster.EMPNO;
                Sheet.Cells[string.Format("C{0}", i)].Value = tw1.LabourMaster.Person_Name;
                Sheet.Cells[string.Format("D{0}", i)].Value = tw1.LabourMaster.Position;
                switch (tw1.LabourMaster.ManPowerSupply)
                {
                    case 1:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "CITISCAPE"; 
                        break;
                    case 2:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "CALIBERS";
                        break;
                    case 3:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "SAWAEED ";
                        break;
                    case 4:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "TAKATOF";
                        break;
                    case 5:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "Shorook ";
                        break;
                    case 6:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "ARABTEC ";
                        break;
                    case 7:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "Fibrex ";
                        break;
                    case 8:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "GROVE";
                        break;
                }
                i++;
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =transfer.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
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
