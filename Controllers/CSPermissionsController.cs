using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlygodknows.Controllers
{
    using onlygodknows.Models;
    [Authorize(Roles = "Admin")]
    public class CSPermissionsController : Controller
    { 
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        public ActionResult project_permissions()
        {
            this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");
            this.ViewBag.permi = new SelectList(this.db.AspNetUsers, "csid", "UserName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult project_permissions(proplist prop)
        {
            this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");
            this.ViewBag.permi = new SelectList(this.db.AspNetUsers, "csid", "UserName");
            var csp =new CsPermission();
            foreach (var i in prop.props)
            {
                var a = this.db.CsPermissions.ToList();
                var aa = a.Last();
                csp.CsUser = i.empno;
                csp.Project = i.projectid;
                csp.CsPermission1 = false;
                this.db.CsPermissions.Add(csp);
                this.db.SaveChanges();
            }
            return this.RedirectToAction("project_permissions");
        }
    }
}