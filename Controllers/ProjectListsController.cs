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
    [Authorize(Roles = "Admin")]
    public class ProjectListsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: ProjectLists
        public ActionResult Index()
        {
            return View(db.ProjectLists.ToList());
        }

        // GET: ProjectLists/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectList projectList = db.ProjectLists.Find(id);
            if (projectList == null)
            {
                return HttpNotFound();
            }
            return View(projectList);
        }

        // GET: ProjectLists/Create
        public ActionResult Create()
        {
            ViewBag.excute_by = new SelectList(
                new List<SelectListItem>
                    {
                        new SelectListItem { Text = "citiscape", Value =  "citiscape"},
                        new SelectListItem { Text = "grove", Value = "grove" },
                    },
                "Value",
                "Text");
            return View();
        }

        // POST: ProjectLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectCode,STAMP_CODE,INQUIRY,PROJECT_NAME,CLIENT_MC,Notice_01,Notice_02,SCOPE_OF_WORK,START_DATE,END_DATE,STATUS,INSURANCE_STATUS,LOCATION,PM,PM_CONTACT,Completion_Certificate,REMARKS,Notice,Source,Closed,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,excute_by,project_period,equipment_budget,man_power_budget")] ProjectList projectList)
        {
            if (ModelState.IsValid)
            {
                projectList.STAMP_CODE = projectList.ProjectCode;
                db.ProjectLists.Add(projectList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectList);
        }

        // GET: ProjectLists/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.excute_by = new SelectList(
                new List<SelectListItem>
                    {
                        new SelectListItem { Text = "citiscape", Value = "citiscape" },
                        new SelectListItem { Text = "grove", Value = "grove" },
                    },
                "Value",
                "Text");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectList projectList = db.ProjectLists.Find(id);
            if (projectList == null)
            {
                return HttpNotFound();
            }
            return View(projectList);
        }

        // POST: ProjectLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectCode,STAMP_CODE,INQUIRY,PROJECT_NAME,CLIENT_MC,Notice_01,Notice_02,SCOPE_OF_WORK,START_DATE,END_DATE,STATUS,INSURANCE_STATUS,LOCATION,PM,PM_CONTACT,Completion_Certificate,REMARKS,Notice,Source,Closed,Encoded_Absolute_URL,Item_Type,Path,URL_Path,Workflow_Instance_ID,File_Type,excute_by,project_period,equipment_budget,man_power_budget")] ProjectList projectList)
        {
            if (ModelState.IsValid)
            {
                projectList.STAMP_CODE = projectList.ProjectCode;
                db.Entry(projectList).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectList);
        }

        // GET: ProjectLists/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectList projectList = db.ProjectLists.Find(id);
            if (projectList == null)
            {
                return HttpNotFound();
            }
            return View(projectList);
        }

        // POST: ProjectLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ProjectList projectList = db.ProjectLists.Find(id);
            db.ProjectLists.Remove(projectList);
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
