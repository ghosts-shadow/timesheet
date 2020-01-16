using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlygodknows.Controllers
{
    using onlygodknows.Models;
    [Authorize(Users = "sdiniz")]
    public class CSPermissionsController : Controller
    { 
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        public ActionResult project_permissions()
        {
            this.ViewBag.csp = new SelectList(this.db.ProjectLists, "ID", "PROJECT_NAME");
            this.ViewBag.permi = new SelectList(this.db.AspNetUsers, "id", "UserName");

            return View();
        }
    }
}