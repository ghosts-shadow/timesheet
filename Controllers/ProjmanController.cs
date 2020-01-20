using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlygodknows.Controllers
{
    using Microsoft.AspNet.Identity;

    using onlygodknows.Models;

    public class ProjmanController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        private int i = 0;
        // GET: Projman
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

            foreach (var v in t)
            {
                
            }
            return View();
        }
    }
}