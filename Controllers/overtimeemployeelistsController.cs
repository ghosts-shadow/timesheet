using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MailKit.Net.Smtp;
using Microsoft.Ajax.Utilities;
using MimeKit;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    [Authorize(Roles = "Admin,Head_of_projects,HR_manager,Project_manager,logistics_officer,Admin_View")]
    public class overtimeemployeelistsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: overtimeemployeelists
        public ActionResult Index(int? id)
        {
            var overtimeemployeelists = db.overtimeemployeelists.Include(o => o.LabourMaster)
                .Include(o => o.overtimeref).Where(x => x.otref == id).ToList();
            ViewBag.overtimepro = overtimeemployeelists[0].overtimeref.ProjectList.PROJECT_NAME;
            ViewBag.overtimedatw = overtimeemployeelists[0].overtimeref.overtimedate.Value.ToString("d");
            ViewBag.to = overtimeemployeelists[0].overtimeref.overtimeref1;
            return View(overtimeemployeelists.ToList());
        }

        // GET: overtimeemployeelists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeemployeelist overtimeemployeelist = db.overtimeemployeelists.Find(id);
            if (overtimeemployeelist == null)
            {
                return HttpNotFound();
            }

            return View(overtimeemployeelist);
        }

        // GET: overtimeemployeelists/Create
        [Authorize(Roles = "Project_manager,Head_of_projects,Admin")]
        public ActionResult Create(overtimeref otr)
        {
            var data = new[]
            {
                new SelectListItem {Text = "0", Value = "0"},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"}
            };

            var tfdbl = db.towemps.Where(x => x.towref.mp_to == otr.overtimepro && x.app_by != null)
                .OrderByDescending(x => x.effectivedate).ToList();
            var tfdbl2 = db.towemps.Where(x => x.towref.mp_from == otr.overtimepro && x.app_by != null)
                .OrderByDescending(x => x.effectivedate).ToList();
            var tflist = new List<towemp>();
            foreach (var towemp in tfdbl)
            {
                if (tfdbl2.Exists(x => x.lab_no == towemp.lab_no))
                {
                    var tfed = tfdbl2.OrderByDescending(x => x.effectivedate).ToList()
                        .Find(x => x.lab_no == towemp.lab_no);
                    if (towemp.effectivedate > tfed.effectivedate)
                    {
                        if (!tflist.Exists(x => x.lab_no == towemp.lab_no))
                        {
                            tflist.Add(towemp);
                        }
                    }
                    else
                    {
                        if (tfed.effectivedate > otr.overtimedate)
                        {
                            if (!tflist.Exists(x => x.lab_no == towemp.lab_no))
                            {
                                tflist.Add(towemp);
                            }
                        }
                    }
                }
                else
                {
                    if (!tflist.Exists(x => x.lab_no == towemp.lab_no))
                    {
                        tflist.Add(towemp);
                    }
                }
            }

            var d = db.LabourMasters.ToList();
            var d1 = new List<LabourMaster>();
            foreach (var towemp in tflist)
            {
                if (d.Exists(x => x.ID == towemp.lab_no))
                {
                    d1.Add(d.Find(x => x.ID == towemp.lab_no));
                }
            }

            var assignedemplist = db.asignprojects.OrderByDescending(x => x.asigneddate).ToList();
            var assignlistfinal = new List<asignproject>();
            foreach (var asqw in assignedemplist)
            {
                if (assignlistfinal.Exists(x => x.lab_no == asqw.lab_no))
                {
                    var ascheckvar = assignlistfinal.Find(x => x.lab_no == asqw.lab_no);
                    if (ascheckvar.asigneddate < asqw.asigneddate)
                    {
                        assignlistfinal.Remove(ascheckvar);
                        assignlistfinal.Add(asqw);
                    }
                    else if (ascheckvar.asigneddate == asqw.asigneddate && asqw.Project == otr.overtimepro)
                    {
                        assignlistfinal.Remove(ascheckvar);
                        assignlistfinal.Add(asqw);
                    }
                }
                else
                {
                    assignlistfinal.Add(asqw);
                }
            }

            foreach (var asignproject in assignlistfinal)
            {
                if (asignproject.Project == otr.overtimepro)
                {
                    if (d.Exists(x => x.ID == asignproject.lab_no))
                    {
                        if (!d1.Exists(x => x.ID == asignproject.lab_no))
                        {
                            d1.Add(d.Find(x => x.ID == asignproject.lab_no));
                        }
                    }
                }
            }

            ViewBag.lab_no = new SelectList(d1.OrderBy(x=>x.EMPNO), "ID", "EMPNO");
            var otref = db.overtimerefs.Find(otr.Id);
            ViewBag.overtimepro = otref.ProjectList.PROJECT_NAME;
            ViewBag.overtimedatw = otref.overtimedate.Value.ToString("d");
            ViewBag.overtimeref = otref.overtimeref1;
            ViewBag.overtimeHrs = data;
            TempData["mydata"] = otref;
            return View();
        }

        // POST: overtimeemployeelists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project_manager,Head_of_projects,Admin")]
        public ActionResult Create(overtimeemployeelist[] overtimeemployeelist)
        {
            var otref2 = TempData["mydata"] as overtimeref;
            if (ModelState.IsValid)
            {
                var i = 0;
                var otemp2 = new List<overtimeemployeelist>();
                foreach (var otemp in overtimeemployeelist)
                {
                    if (!otemp2.Exists(x => x.lab_no == otemp.lab_no && x.effectivedate == otemp.effectivedate))
                    {
                        otemp.otref = otref2.Id;
                        otemp2.Add(otemp);
                    }
                }

                foreach (var otemp in otemp2)
                {
                    var otemplist = db.overtimeemployeelists.ToList();
                    var otempottotal = otemplist.FindAll(x =>
                        x.effectivedate == otemp.effectivedate && x.lab_no == otemp.lab_no);
                    if (otempottotal.Count == 0)
                    {
                        db.overtimeemployeelists.Add(otemp);
                        db.SaveChanges();
                    }
                    else
                    {
                        var Hrssum = 0;
                        foreach (var othrs in otempottotal)
                        {
                            if (othrs.hrs != null) Hrssum += othrs.hrs.Value;
                        }

                        if (otemp.hrs != null) Hrssum += otemp.hrs.Value;

                        if (Hrssum < 24)
                        {
                            db.overtimeemployeelists.Add(otemp);
                            db.SaveChanges();
                        }
                    }

                    if (i == 0)
                    {
                        SendMail("", "submitted", otref2.Id);
                    }

                    i++;
                }

                return RedirectToAction("Index", "overtimerefs");
            }

            var data = new[]
            {
                new SelectListItem {Text = "0", Value = "0"},
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"},
                new SelectListItem {Text = "3", Value = "3"},
                new SelectListItem {Text = "4", Value = "4"},
                new SelectListItem {Text = "5", Value = "5"},
                new SelectListItem {Text = "6", Value = "6"},
                new SelectListItem {Text = "7", Value = "7"},
                new SelectListItem {Text = "8", Value = "8"},
                new SelectListItem {Text = "9", Value = "9"},
                new SelectListItem {Text = "10", Value = "10"},
                new SelectListItem {Text = "11", Value = "11"},
                new SelectListItem {Text = "12", Value = "12"},
                new SelectListItem {Text = "13", Value = "13"},
                new SelectListItem {Text = "14", Value = "14"},
                new SelectListItem {Text = "15", Value = "15"},
                new SelectListItem {Text = "16", Value = "16"}
            };

            var tfdbl = db.towemps.Where(x => x.towref.mp_to == otref2.overtimepro && x.app_by != null)
                .OrderByDescending(x => x.effectivedate).ToList();
            var tfdbl2 = db.towemps.Where(x => x.towref.mp_from == otref2.overtimepro && x.app_by != null)
                .OrderByDescending(x => x.effectivedate).ToList();
            var tflist = new List<towemp>();
            foreach (var towemp in tfdbl)
            {
                if (tfdbl2.Exists(x => x.lab_no == towemp.lab_no))
                {
                    var tfed = tfdbl2.OrderByDescending(x => x.effectivedate).ToList()
                        .Find(x => x.lab_no == towemp.lab_no);
                    if (towemp.effectivedate > tfed.effectivedate)
                    {
                        if (!tflist.Exists(x => x.lab_no == towemp.lab_no))
                        {
                            tflist.Add(towemp);
                        }
                    }
                    else
                    {
                        if (tfed.effectivedate > otref2.overtimedate)
                        {
                            if (!tflist.Exists(x => x.lab_no == towemp.lab_no))
                            {
                                tflist.Add(towemp);
                            }
                        }
                    }
                }
                else
                {
                    if (!tflist.Exists(x => x.lab_no == towemp.lab_no) && towemp.effectivedate <= otref2.overtimedate)
                    {
                        tflist.Add(towemp);
                    }
                }
            }

            var d = db.LabourMasters.ToList();
            var d1 = new List<LabourMaster>();
            foreach (var towemp in tflist)
            {
                if (d.Exists(x => x.ID == towemp.lab_no))
                {
                    d1.Add(d.Find(x => x.ID == towemp.lab_no));
                }
            }

            ViewBag.lab_no = new SelectList(d1, "ID", "EMPNO");
            var otref = db.overtimerefs.Find(otref2.Id);
            ViewBag.overtimepro = otref.ProjectList.PROJECT_NAME;
            ViewBag.overtimedatw = otref.overtimedate.Value.ToString("d");
            ViewBag.overtimeref = otref.overtimeref1;
            ViewBag.overtimeHrs = new SelectList(data);
            TempData["mydata"] = otref;
            return View(overtimeemployeelist[0]);
        }

        [Authorize(Roles = "Head_of_projects")]
        public ActionResult aprestatus(int tr, string message)
        {
            var trvar = db.overtimerefs.Where(x => x.Id == tr).ToList();
            var templist = db.overtimeemployeelists.ToList();
            var telist = new List<overtimeemployeelist>();
            foreach (var towref in trvar)
            {
                telist.AddRange(towref.overtimeemployeelists);
            }

            if (telist.Count != 0)
            {
                var i = 0;
                foreach (var towemp in telist)
                {
                    if (message.Contains("approved"))
                    {
                        i++;
                        var te = templist.Find(x => x.Id == towemp.Id);
                        te.status = "approved";
                        te.hopAP = this.User.Identity.Name;
                        te.HRAP = "not needed";
                        this.db.Entry(te).State = EntityState.Modified;
                        this.db.SaveChanges();
                        if (i == 1)
                           SendMail("", "approved", tr);
                    }
                    else
                    {
                        i++;
                        var te = templist.Find(x => x.Id == towemp.Id);
                        te.status = "rejected for :" + message;
                        te.HRAP = "not needed";
                        this.db.Entry(te).State = EntityState.Modified;
                        this.db.SaveChanges();
                        if (i == 1)
                            SendMail(message, "rejected", tr);
                    }
                }
            }

            return RedirectToAction("Index", "overtimerefs");
        }

        public void SendMail(string msg, string action, int tr)
        {
            var context = new ApplicationDbContext();
            var trid = db.overtimerefs.Find(tr);
            var userslist = new List<AspNetUser>();
            var message = new MimeMessage();
            var man = this.db.AspNetUsers.ToList();
            message.From.Add(new MailboxAddress("timekeeper", "timekeeper@citiscapegroup.com"));
            if (action == "approved")
            {
                var users = context.Users
                    .Where(x => x.Roles.Select(y => y.RoleId).Contains("6023f0a5-8d24-45d3-9641-b3c2e39aa763") || x.Roles.Select(y => y.RoleId).Contains("8840f8c3-862d-4b1e-9205-47e84c85696e")).ToList();
                var cper = this.db.CsPermissions.Where(x => x.Project == trid.overtimepro).ToList();
                foreach (var csp in cper)
                {
                    var manvar = man.Find(x => x.csid == csp.CsUser);
                    if (users.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }

                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }

                message.Subject = "OVERTIME REQUEST";
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the overtime request for workers from project " + trid.ProjectList.PROJECT_NAME + "has been approved"+ "\n\n\n" + "Thanks Best Regards, "
                };
                if (message.To != null)
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect("outlook.office365.com", 587, false);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate("timekeeper@citiscapegroup.com", "Vam15380");

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            else if (action == "rejected")
            {
                var users = context.Users
                    .Where(x => x.Roles.Select(y => y.RoleId).Contains("6023f0a5-8d24-45d3-9641-b3c2e39aa763")).ToList();
                var cper = this.db.CsPermissions.Where(x => x.Project == trid.overtimepro).ToList();
                foreach (var csp in cper)
                {
                    var manvar = man.Find(x => x.csid == csp.CsUser);
                    if (users.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }

                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }

                message.Subject = "OVERTIME REQUEST";
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the overtime request for workers from project " + trid.ProjectList.PROJECT_NAME + "has been rejected for the reason : "+ msg + "\n\n\n" + "Thanks Best Regards, "
                };
                if (message.To != null)
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect("outlook.office365.com", 587, false);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate("timekeeper@citiscapegroup.com", "Vam15380");

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
            else if (action == "submitted")
            {
                var users = context.Users
                    .Where(x => x.Roles.Select(y => y.RoleId).Contains("7ab5062b-c31b-4f73-992a-f289396292da")).ToList();
                foreach (var csp in users)
                {
                    var manvar = man.Find(x => x.Id == csp.Id);
                    if (users.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }

                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }

                message.Subject = "OVERTIME REQUEST";
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the overtime request for workers from project " + trid.ProjectList.PROJECT_NAME +"has been submitted for ur approval/rejection" + "\n\n\n" + "http://cstimesheet.ddns.net:6333/timesheet/overtimerefs" + "\n\n\n" + "Thanks Best Regards, "
                };
                if (message.To != null)
                {
                    using (var client = new SmtpClient())
                    {
                        client.Connect("outlook.office365.com", 587, false);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate("timekeeper@citiscapegroup.com", "Vam15380");

                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
            }
        }

        // GET: overtimeemployeelists/Edit/5
        [Authorize(Roles = "Project_manager,Head_of_projects")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeemployeelist overtimeemployeelist = db.overtimeemployeelists.Find(id);
            if (overtimeemployeelist == null)
            {
                return HttpNotFound();
            }

            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", overtimeemployeelist.lab_no);
            return View(overtimeemployeelist);
        }

        // POST: overtimeemployeelists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project_manager,Head_of_projects")]
        public ActionResult Edit(overtimeemployeelist overtimeemployeelist)
        {
            if (overtimeemployeelist.status.IsNullOrWhiteSpace())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(overtimeemployeelist).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "overtimeemployeelists", overtimeemployeelist.otref);
                }
            }

            ViewBag.lab_no = new SelectList(db.LabourMasters, "ID", "EMPNO", overtimeemployeelist.lab_no);
            return View(overtimeemployeelist);
        }

        // GET: overtimeemployeelists/Delete/5\
        [Authorize(Users = "sdiniz")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            overtimeemployeelist overtimeemployeelist = db.overtimeemployeelists.Find(id);
            if (overtimeemployeelist == null)
            {
                return HttpNotFound();
            }

            return View(overtimeemployeelist);
        }

        // POST: overtimeemployeelists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "sdiniz")]
        public ActionResult DeleteConfirmed(int id)
        {
            overtimeemployeelist overtimeemployeelist = db.overtimeemployeelists.Find(id);
            var otref = db.overtimerefs.Find(overtimeemployeelist.otref);
            if (overtimeemployeelist.status.IsNullOrWhiteSpace())
            {
                db.overtimeemployeelists.Remove(overtimeemployeelist);
                db.SaveChanges();
                if (otref.overtimeemployeelists.Count == 0)
                {
                    db.overtimerefs.Remove(otref);
                    db.SaveChanges();
                    return RedirectToAction("Index", "overtimerefs");
                }

                return RedirectToAction("Index", "overtimeemployeelists", otref);
            }

            return View(overtimeemployeelist);
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