using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MailKit.Net.Smtp;
using Microsoft.AspNet.Identity;
using MimeKit;
using onlygodknows.Models;

namespace onlygodknows.Controllers
{
    using OfficeOpenXml;

    [Authorize(Roles = "Admin,Employee,Head_of_projects,HR_manager,Project_manager,logistics_officer,Admin_View")]
    public class towempsController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: towemps
        public ActionResult Index(int? id)
        {
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var t = new List<ProjectList>();
            var towrlist = db.towrefs.ToList();
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var towemps = db.towemps.Include(t1 => t1.LabourMaster).Include(t1 => t1.towref).ToList();
            var tr = this.db.towrefs.Find(id);
            ViewBag.tw = tr.Id;
            ViewBag.form = tr.ProjectList1.PROJECT_NAME;
            ViewBag.to = tr.ProjectList.PROJECT_NAME;
            ViewBag.R_no = tr.R_no;
            ViewBag.refe1 = tr.refe1;
            ViewBag.mpcdate = tr.mpcdate;
            var tow = towemps.FindAll(x => x.rowref == id);
            foreach (var towemp in tow)
            {
                if (t.Exists(x => x.ID == tr.mp_to))
                {
                    towemp.towref.AR = true;
                }
            }

            return View(tow.OrderBy(x=>x.LabourMaster.EMPNO).ToList());
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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult Create(towref tw1)
        {
            var d = db.LabourMasters.Where(x => x.EMPNO >= 4).ToList();
            var d1 = new List<LabourMaster>();
            if (tw1.mp_from == 698)
            {
                d1 = d;
            }
            else
            {
                var dateids = new DateTime(tw1.mpcdate.Value.Year, tw1.mpcdate.Value.Month, 1);
                var tflist = new List<towemp>();
                var tflist1 = new List<towemp>();
                var tfdbl = db.towemps.Where(x => x.towref.mp_to == tw1.mp_from && x.app_by != null)
                    .OrderByDescending(x => x.effectivedate).ToList();
                var tfdbl2 = db.towemps.Where(x => x.towref.mp_from == tw1.mp_from && x.app_by != null)
                    .OrderByDescending(x => x.effectivedate).ToList();
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
                            if (tfed.effectivedate > dateids)
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
                        else if (ascheckvar.asigneddate == asqw.asigneddate && asqw.Project == tw1.mp_from)
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
                    if (asignproject.Project == tw1.mp_from)
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
            }
                ViewBag.lab_no = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                    "EMPNO");
                ViewBag.lab_name = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                    "Person_Name");
                ViewBag.lab_position = new SelectList(d1.OrderBy(x => x.EMPNO),
                    "ID",
                    "Position");
                ViewBag.lab_mps = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                    "ManPowerSupply");
                ViewBag.lab_mpslist = new SelectList(db.ManPowerSuppliers, "ID", "Supplier");

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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult Create([Bind(Include = "Id,lab_no,effectivedate,rowref")]
            towemp[] towemp)
        {
            towref tw = TempData["mydata"] as towref;
            if (ModelState.IsValid)
            {
                var i = 0;
                foreach (var towemp1 in towemp)
                {
                    if (i == 0)
                    {
                        towemp1.rowref = tw.Id;
                        db.towemps.Add(towemp1);
                        db.SaveChanges();
                        SendMail("", "submitted", tw.Id);
                    }

                    var checkemplist = db.towemps.Where(x => x.rowref == tw.Id).ToList();
                    if (!checkemplist.Exists(x => x.lab_no == towemp1.lab_no))
                    {
                        towemp1.rowref = tw.Id;
                        db.towemps.Add(towemp1);
                        db.SaveChanges();
                    }

                    i++;
                }

                return RedirectToAction("Index", "towrefs");
            }

            var tw1 = tw;
            var d = db.LabourMasters.Where(x => x.EMPNO >= 4).ToList();
            var d1 = new List<LabourMaster>();
            if (tw1.mp_from == 698)
            {
                d1 = d;
            }
            else
            {
                var dateids = new DateTime(tw1.mpcdate.Value.Year, tw1.mpcdate.Value.Month, 1);
                var tflist = new List<towemp>();
                var tflist1 = new List<towemp>();
                var tfdbl = db.towemps.Where(x => x.towref.mp_to == tw1.mp_from && x.app_by != null)
                    .OrderByDescending(x => x.effectivedate).ToList();
                var tfdbl2 = db.towemps.Where(x => x.towref.mp_from == tw1.mp_from && x.app_by != null)
                    .OrderByDescending(x => x.effectivedate).ToList();
                foreach (var towemp1 in tfdbl)
                {
                    if (tfdbl2.Exists(x => x.lab_no == towemp1.lab_no))
                    {
                        var tfed = tfdbl2.OrderByDescending(x => x.effectivedate).ToList()
                            .Find(x => x.lab_no == towemp1.lab_no);
                        if (towemp1.effectivedate > tfed.effectivedate)
                        {
                            if (!tflist.Exists(x => x.lab_no == towemp1.lab_no))
                            {
                                tflist.Add(towemp1);
                            }
                        }
                        else
                        {
                            if (tfed.effectivedate > dateids)
                            {
                                if (!tflist.Exists(x => x.lab_no == towemp1.lab_no))
                                {
                                    tflist.Add(towemp1);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!tflist.Exists(x => x.lab_no == towemp1.lab_no) && towemp1.effectivedate <= dateids)
                        {
                            tflist.Add(towemp1);
                        }
                    }
                }


                foreach (var towemp1 in tflist)
                {
                    if (d.Exists(x => x.ID == towemp1.lab_no))
                    {
                        d1.Add(d.Find(x => x.ID == towemp1.lab_no));
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
                        else if (ascheckvar.asigneddate == asqw.asigneddate && asqw.Project == tw1.mp_from)
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
                    if (asignproject.Project == tw1.mp_from)
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
            }
            ViewBag.lab_no = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                "EMPNO");
            ViewBag.lab_name = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                "Person_Name");
            ViewBag.lab_position = new SelectList(d1.OrderBy(x => x.EMPNO),
                "ID",
                "Position");
            ViewBag.lab_mps = new SelectList(d1.OrderBy(x => x.EMPNO), "ID",
                "ManPowerSupply");
            ViewBag.lab_mpslist = new SelectList(db.ManPowerSuppliers, "ID", "Supplier");
            return View(towemp[1]);
        }

        // GET: towemps/Edit/5
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult Edit([Bind(Include = "Id,lab_no,effectivedate,rowref")]
            towemp towemp)
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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
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
        [Authorize(Roles = "HR_manager,Project_manager,logistics_officer")]
        public ActionResult DeleteConfirmed(int id)
        {
            towemp towemp = db.towemps.Find(id);
            db.towemps.Remove(towemp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public void DownloadExcel(int tr)
        {
            var Ep = new ExcelPackage();
            var Sheet = Ep.Workbook.Worksheets.Add("transferred_workers");
            var towemps = db.towemps.Include(t => t.LabourMaster).Include(t => t.towref).ToList();
            var tw = this.db.towrefs.Find(tr);
            var mansuplierlist = db.ManPowerSuppliers.ToList();
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
                /*switch (tw1.LabourMaster.ManPowerSupply)
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
                    case 9:
                        Sheet.Cells[string.Format("E{0}", i)].Value = "COMMON";
                        break;
                }*/
                var mansup = mansuplierlist.Find(x => x.ID == tw1.LabourMaster.ManPowerSupply);
                Sheet.Cells[string.Format("E{0}", i)].Value = mansup.Supplier;
                i++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();
            this.Response.Clear();
            this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            this.Response.AddHeader("content-disposition", "filename =transfer.xlsx");
            this.Response.BinaryWrite(Ep.GetAsByteArray());
            this.Response.End();
        }

        [Authorize(Roles = "Project_manager")]
        public ActionResult aprestatus(int tr, string message)
        {
            var trvar = db.towrefs.Where(x => x.Id == tr).ToList();
            var templist = db.towemps.ToList();
            var telist = new List<towemp>();
            foreach (var towref in trvar)
            {
                telist.AddRange(towref.towemps);
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
                        te.ARstatus = "approved";
                        te.app_by = this.User.Identity.Name;
                        this.db.Entry(te).State = EntityState.Modified;
                        this.db.SaveChanges();
                        if (i == 1)
                            SendMail("", "approved", tr);
                    }
                    else
                    {
                        i++;
                        var te = templist.Find(x => x.Id == towemp.Id);
                        te.ARstatus = "rejected for :" + message;
                        this.db.Entry(te).State = EntityState.Modified;
                        this.db.SaveChanges();
                        if (i == 1)
                            SendMail(message, "rejected", tr);
                    }
                }
            }

            return RedirectToAction("Index", "towrefs");
        }

        public void SendMail(string msg, string action, int tr)
        {
            var trvar = db.towrefs.ToList().Find(x => x.Id == tr);
            var man = this.db.AspNetUsers.ToList();
            var asa = new List<AspNetUser>();
            var context = new ApplicationDbContext();
            var users = context.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains("6023f0a5-8d24-45d3-9641-b3c2e39aa763") ||
                            x.Roles.Select(y => y.RoleId).Contains("4d175b2a-31a2-448d-8a2e-cde6c328c721")).ToList();
            var users1 = context.Users
                .Where(x => x.Roles.Select(y => y.RoleId).Contains("8840f8c3-862d-4b1e-9205-47e84c85696e") ||
                            x.Roles.Select(y => y.RoleId).Contains("4d175b2a-31a2-448d-8a2e-cde6c328c721")).ToList();
            var cper = this.db.CsPermissions.Where(x => x.Project == trvar.mp_to).ToList();
            var cper1 = this.db.CsPermissions.Where(x => x.Project == trvar.mp_from).ToList();
            var userslist = new List<AspNetUser>();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("timekeeper", "timekeeper@citiscapegroup.com"));
            if (action.Contains("submitted"))
            {
                foreach (var csp in cper)
                {
                    var manvar = man.Find(x => x.csid == csp.CsUser);
                    if (manvar != null && users.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }
                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }
                message.Cc.Add(new MailboxAddress("Mohamed Khairy", "mkhairy@citiscapegroup.com"));
                message.Cc.Add(new MailboxAddress("Zyad Nader", "zNader@citiscapegroup.com"));

                message.Subject = "remobilization of staff";
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the transfer of workers from project " +
                           trvar.ProjectList1.PROJECT_NAME + " to project " + trvar.ProjectList.PROJECT_NAME +
                           "has been submitted for ur approval/rejection" + "\n\n\n" +
                           "http://cstimesheet.ddns.net:6333/timesheet/towrefs" + "\n\n\n" + "Thanks Best Regards, "
                };
                if (message.To.Count != 0)
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
            else if (action.Contains("approved"))
            {
                message.Subject = "";
                foreach (var csp in cper1)
                {
                    var manvar = man.Find(x => x.csid == csp.CsUser);
                    if (manvar != null && users1.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }
                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }

                message.Subject = "remobilization of staff";
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the transfer of workers from project " +
                           trvar.ProjectList1.PROJECT_NAME + " to project " + trvar.ProjectList.PROJECT_NAME +
                           "has been approvaled" + "\n\n\n" + "http://cstimesheet.ddns.net:6333/timesheet/towrefs" +
                           "\n\n\n" + "Thanks Best Regards, "
                };

               // message.Cc.Add(new MailboxAddress("HR", "yrashid@citiscapegroup.com"));
                message.Cc.Add(new MailboxAddress("Mohamed Khairy", "mkhairy@citiscapegroup.com"));
                message.Cc.Add(new MailboxAddress("Zyad Nader", "zNader@citiscapegroup.com"));
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
            else if (action.Contains("rejected"))
            {
                foreach (var csp in cper1)
                {
                    var manvar = man.Find(x => x.csid == csp.CsUser);
                    if (manvar != null && users1.Exists(x => x.Id == manvar.Id))
                    {
                        userslist.Add(manvar);
                    }
                }

                foreach (var netUser in userslist)
                {
                    message.To.Add((new MailboxAddress(netUser.UserName, netUser.Email)));
                }

                message.Subject = "remobilization of staff";
                message.Cc.Add(new MailboxAddress("Mohamed Khairy", "mkhairy@citiscapegroup.com"));
                message.Cc.Add(new MailboxAddress("Zyad Nader", "zNader@citiscapegroup.com"));
                message.Body = new TextPart("plain")
                {
                    Text = @"Dear Sir/ma'am," + "\n\n" + "Please note that the transfer of workers from project " +
                           trvar.ProjectList1.PROJECT_NAME + " to project " + trvar.ProjectList.PROJECT_NAME +
                           "has been rejected for " + msg + "\n\n\n" +
                           "http://cstimesheet.ddns.net:6333/timesheet/towrefs" + "\n\n\n" + "Thanks Best Regards, "
                };
                if (message.To.Count != 0)
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

        [Authorize(Roles = "Admin")]
        public ActionResult search(string searchemp)
        {
            var searchlist = db.towemps.OrderByDescending(x=>x.effectivedate).ThenBy(x=>x.LabourMaster.EMPNO).ToList();
            var emplist = db.LabourMasters.OrderBy(x=>x.EMPNO).ThenBy(x=>x.ManPowerSupply).ToList();
            var searchvar = new towemp();
            ViewBag.errormsg = "";
            if (searchemp != null)
            {
                if (long.TryParse(searchemp,out var empid))
                {
                    var empno = emplist.Find(x => x.EMPNO == empid);
                    if (empno != null)
                    {
                        if (searchlist.Exists(x => x.lab_no == empno.ID))
                        {
                            searchvar = searchlist.Find(x => x.lab_no == empno.ID);
                        }
                        else
                        {
                            ViewBag.errormsg = "employee no " + empno.EMPNO + " is not assigned to any projects";
                        }
                    }
                    else
                    {
                        ViewBag.errormsg = "no such employee exist in the data base,plz check again with proper employee no";
                    }

                }
            }
            return View(searchvar);
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