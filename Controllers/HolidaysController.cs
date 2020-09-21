namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using onlygodknows.Models;
    [Authorize(Roles = "Admin")]
    public class HolidaysController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: Holidays/Create
        public ActionResult Create()
        {
            return this.View();
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                Include =
                    "ID,year,Comment,Date,Hejri,Holiday1,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type")]
            Holiday holiday,
            DateTime from,
            DateTime to)
        {
            if (this.ModelState.IsValid)
            {
                var li = new List<DateTime>();
                while (from <= to)
                {
                    holiday.Date = from;
                    holiday.year = from.Year;
                    this.db.Holidays.Add(holiday);
                    this.db.SaveChanges();
                    li.Add(from);
                    from = from.AddDays(1);
                }

                return this.RedirectToAction("Index");
            }

            return this.View(holiday);
        }

        // GET: Holidays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var holiday = this.db.Holidays.Find(id);
            if (holiday == null) return this.HttpNotFound();
            return this.View(holiday);
        }

        // POST: Holidays/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var holiday = this.db.Holidays.Find(id);
            this.db.Holidays.Remove(holiday);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        // GET: Holidays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var holiday = this.db.Holidays.Find(id);
            if (holiday == null) return this.HttpNotFound();
            return this.View(holiday);
        }

        // GET: Holidays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var holiday = this.db.Holidays.Find(id);
            if (holiday == null) return this.HttpNotFound();
            return this.View(holiday);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                Include =
                    "ID,year,Comment,Date,Hejri,Holiday1,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type")]
            Holiday holiday)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(holiday).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            return this.View(holiday);
        }

        // GET: Holidays
        public ActionResult Index()
        {
            return this.View(this.db.Holidays.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) this.db.Dispose();
            base.Dispose(disposing);
        }
    }
}