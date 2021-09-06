// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjmanController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProjmanController type.
// </summary>
//
//  this will all change in april
// --------------------------------------------------------------------------------------------------------------------

using System.Data.Entity.Core.Metadata.Edm;
using Microsoft.Ajax.Utilities;
using MimeKit;

namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using onlygodknows.Models;
    using MimeKit;
    using SmtpClient = MailKit.Net.Smtp.SmtpClient;

    public class ProjmanController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        private int i = 0;

        [Authorize(Roles = "Project_manager,HR_manager")]
        public ActionResult approved()
        {
            var da = new DateTime();
            long p = 0, mp = 0;

            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);

            long.TryParse(this.TempData["mydata2"].ToString(), out p);

            long.TryParse(this.TempData["mydata1"].ToString(), out mp);
            if (User.IsInRole("Project_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
                var suplist = db.ManPowerSuppliers.ToList();
                var proplist = db.ProjectLists.ToList();
                var sup = suplist.Find(x => x.ID == mp).Supplier;
                var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;
                var j = 0;
                foreach (var approval in ap2)
                {
                    j++;
                    approval.status = "approved";
                    approval.Ausername = this.User.Identity.Name;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    if (j == 1)
                    {
                        SendMail(sup, prop, da, this.User.Identity.Name, approval.Susername,
                            "is approved", true, " ", "HR_manager");
                    }
                }
            }

            if (User.IsInRole("HR_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "approved").ToList();
                var suplist = db.ManPowerSuppliers.ToList();
                var proplist = db.ProjectLists.ToList();
                var sup = suplist.Find(x => x.ID == mp).Supplier;
                var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;
                var j = 0;
                foreach (var approval in ap2)
                {
                    j++;
                    approval.status = "approved by HR";
                    approval.Husername = this.User.Identity.Name;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    if (j == 1)
                    {
                        SendMail(sup, prop, da, this.User.Identity.Name, approval.Ausername,
                            "is approved by hr", true, " ", null);
                    }
                }
            }

            return this.RedirectToAction("PMapproval");
        }

        [Authorize(Roles = "Project_manager,HR_manager")]
        public ActionResult approved1(long? mp, long? p, DateTime? da)
        {
            if (User.IsInRole("Project_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
                var suplist = db.ManPowerSuppliers.ToList();
                var proplist = db.ProjectLists.ToList();
                var sup = suplist.Find(x => x.ID == mp).Supplier;
                var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;

                var j = 0;
                foreach (var approval in ap2)
                {
                    j++;
                    approval.status = "approved";
                    approval.Ausername = this.User.Identity.Name;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    if (j == 1)
                    {
                        if (da != null)
                            SendMail(sup, prop, da.Value, this.User.Identity.Name, approval.Susername,
                                "approved", true, " ", "HR_manager");
                    }
                }
            }

            if (User.IsInRole("HR_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "approved").ToList();
                var suplist = db.ManPowerSuppliers.ToList();
                var proplist = db.ProjectLists.ToList();
                var sup = suplist.Find(x => x.ID == mp).Supplier;
                var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;

                var j = 0;
                foreach (var approval in ap2)
                {
                    j++;
                    approval.status = "approved by HR";
                    approval.Husername = this.User.Identity.Name;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    if (j == 1)
                    {
                        if (da != null)
                            SendMail(sup, prop, da.Value, this.User.Identity.Name, approval.Ausername,
                                "approved by HR", true, " ", null);
                    }
                }
            }

            return this.RedirectToAction("appsum");
        }

        public ActionResult appsum()
        {
            var t = new List<ProjectList>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("HR_manager")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                t = new List<ProjectList>();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var ap2 = new List<approval>();
            foreach (var listp in t)
            {
                var aaa = this.db.approvals.Where(x => x.P_id == listp.ID && x.status == "submitted").ToList();
                if (User.IsInRole("HR_manager"))
                {
                    aaa = this.db.approvals.Where(x => x.P_id == listp.ID && x.status == "approved").ToList();
                }

                foreach (var approval in aaa)
                    if (!ap2.Exists(x =>
                        x.MPS_id == approval.MPS_id && x.adate == approval.adate && x.P_id == listp.ID))
                        ap2.Add(approval);
            }

            var apno = new List<approvalnonsence>();
            foreach (var mdknonsence in ap2)
            {
                var apnon = new approvalnonsence();
                apnon.adate = mdknonsence.adate;
                apnon.MPS_id = mdknonsence.MPS_id;
                apnon.P_id = mdknonsence.P_id;
                apnon.MPS_name = mdknonsence.Attendance.MainTimeSheet.ManPowerSupplier1.Supplier;
                apnon.P_name = mdknonsence.Attendance.MainTimeSheet.ProjectList.PROJECT_NAME;
                var atlist = this.db.Attendances.Where(x => x.SubMain == mdknonsence.Attendance.SubMain).ToList();
                int.TryParse(mdknonsence.adate.Value.Year.ToString(), out var inty);
                int.TryParse(mdknonsence.adate.Value.Month.ToString(), out var intm);
                var date = new DateTime(inty, intm, 1);
                var fri = new List<int>();
                for (var i = 0; i < DateTime.DaysInMonth(inty, intm); i++)
                {
                    if (date.DayOfWeek.Equals(DayOfWeek.Friday)) fri.Add(date.Day);

                    date = date.AddDays(1);
                }

                if (apnon.O_T == null) apnon.O_T = 0;

                if (apnon.N_T == null) apnon.N_T = 0;
                if (apnon.friday == null) apnon.friday = 0;

                if (!fri.Contains(mdknonsence.adate.Value.Day))
                    foreach (var at in atlist)
                    {
                        long.TryParse(at.MainTimeSheet.ManPowerSupplier1.NormalTimeUpto.ToString(), out var n_t);
                        {
                            if (mdknonsence.adate.Value.Day == 1)
                            {
                                var a = new long();
                                long.TryParse(at.C1, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 2)
                            {
                                var a = new long();
                                long.TryParse(at.C2, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 3)
                            {
                                var a = new long();
                                long.TryParse(at.C3, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 4)
                            {
                                var a = new long();
                                long.TryParse(at.C4, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 5)
                            {
                                var a = new long();
                                long.TryParse(at.C5, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 6)
                            {
                                var a = new long();
                                long.TryParse(at.C6, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 7)
                            {
                                var a = new long();
                                long.TryParse(at.C7, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 8)
                            {
                                var a = new long();
                                long.TryParse(at.C8, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 9)
                            {
                                var a = new long();
                                long.TryParse(at.C9, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 10)
                            {
                                var a = new long();
                                long.TryParse(at.C10, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 11)
                            {
                                var a = new long();
                                long.TryParse(at.C11, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 12)
                            {
                                var a = new long();
                                long.TryParse(at.C12, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 13)
                            {
                                var a = new long();
                                long.TryParse(at.C13, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 14)
                            {
                                var a = new long();
                                long.TryParse(at.C14, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 15)
                            {
                                var a = new long();
                                long.TryParse(at.C15, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 16)
                            {
                                var a = new long();
                                long.TryParse(at.C16, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 17)
                            {
                                var a = new long();
                                long.TryParse(at.C17, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 18)
                            {
                                var a = new long();
                                long.TryParse(at.C18, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 19)
                            {
                                var a = new long();
                                long.TryParse(at.C19, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 20)
                            {
                                var a = new long();
                                long.TryParse(at.C20, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 21)
                            {
                                var a = new long();
                                long.TryParse(at.C21, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 22)
                            {
                                var a = new long();
                                long.TryParse(at.C22, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 23)
                            {
                                var a = new long();
                                long.TryParse(at.C23, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 24)
                            {
                                var a = new long();
                                long.TryParse(at.C24, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 25)
                            {
                                var a = new long();
                                long.TryParse(at.C25, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 26)
                            {
                                var a = new long();
                                long.TryParse(at.C26, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 27)
                            {
                                var a = new long();
                                long.TryParse(at.C27, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 28)
                            {
                                var a = new long();
                                long.TryParse(at.C28, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 29)
                            {
                                var a = new long();
                                long.TryParse(at.C29, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 30)
                            {
                                var a = new long();
                                long.TryParse(at.C30, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }

                            if (mdknonsence.adate.Value.Day == 31)
                            {
                                var a = new long();
                                long.TryParse(at.C31, out a);
                                if (a > n_t)
                                {
                                    apnon.O_T += a - n_t;
                                    apnon.N_T += n_t;
                                }
                                else
                                {
                                    apnon.N_T += a;
                                }
                            }
                        }
                    }
                else
                    foreach (var at in atlist)
                    {
                        if (mdknonsence.adate.Value.Day == 1)
                        {
                            var a = new long();
                            long.TryParse(at.C1, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 2)
                        {
                            var a = new long();
                            long.TryParse(at.C2, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 3)
                        {
                            var a = new long();
                            long.TryParse(at.C3, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 4)
                        {
                            var a = new long();
                            long.TryParse(at.C4, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 5)
                        {
                            var a = new long();
                            long.TryParse(at.C5, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 6)
                        {
                            var a = new long();
                            long.TryParse(at.C6, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 7)
                        {
                            var a = new long();
                            long.TryParse(at.C7, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 8)
                        {
                            var a = new long();
                            long.TryParse(at.C8, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 9)
                        {
                            var a = new long();
                            long.TryParse(at.C9, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 10)
                        {
                            var a = new long();
                            long.TryParse(at.C10, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 11)
                        {
                            var a = new long();
                            long.TryParse(at.C11, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 12)
                        {
                            var a = new long();
                            long.TryParse(at.C12, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 13)
                        {
                            var a = new long();
                            long.TryParse(at.C13, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 14)
                        {
                            var a = new long();
                            long.TryParse(at.C14, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 15)
                        {
                            var a = new long();
                            long.TryParse(at.C15, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 16)
                        {
                            var a = new long();
                            long.TryParse(at.C16, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 17)
                        {
                            var a = new long();
                            long.TryParse(at.C17, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 18)
                        {
                            var a = new long();
                            long.TryParse(at.C18, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 19)
                        {
                            var a = new long();
                            long.TryParse(at.C19, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 20)
                        {
                            var a = new long();
                            long.TryParse(at.C20, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 21)
                        {
                            var a = new long();
                            long.TryParse(at.C21, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 22)
                        {
                            var a = new long();
                            long.TryParse(at.C22, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 23)
                        {
                            var a = new long();
                            long.TryParse(at.C23, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 24)
                        {
                            var a = new long();
                            long.TryParse(at.C24, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 25)
                        {
                            var a = new long();
                            long.TryParse(at.C25, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 26)
                        {
                            var a = new long();
                            long.TryParse(at.C26, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 27)
                        {
                            var a = new long();
                            long.TryParse(at.C27, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 28)
                        {
                            var a = new long();
                            long.TryParse(at.C28, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 29)
                        {
                            var a = new long();
                            long.TryParse(at.C29, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 30)
                        {
                            var a = new long();
                            long.TryParse(at.C30, out a);
                            apnon.friday += a;
                        }

                        if (mdknonsence.adate.Value.Day == 31)
                        {
                            var a = new long();
                            long.TryParse(at.C31, out a);
                            apnon.friday += a;
                        }
                    }

                apno.Add(apnon);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(apno);
            }

            return this.View(apno);
        }

        public ActionResult getsubmitted()
        {
            var aap = new List<approval>();
            var a = 0;
            var submitted =
                this.db.approvals.Where(x => x.adate.Value.Month == DateTime.Now.Month && x.status == "submitted");
            foreach (var app1 in submitted)
            {
                if (aap.Count == 0) aap.Add(app1);

                if (app1.MPS_id != aap[a].MPS_id && app1.P_id != aap[a].P_id)
                {
                    aap.Add(app1);
                    a++;
                }
            }

            return new JsonResult {Data = aap, JsonRequestBehavior = JsonRequestBehavior.AllowGet};
        }

        public ActionResult PMapproval(long? manPower, long? pro, DateTime? mtsmonth2)
        {
            var t = new List<ProjectList>();
            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            if (uid1.csid != 0 && !this.User.IsInRole("Admin"))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                t = new List<ProjectList>();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
            }
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            if (manPower.HasValue) this.TempData["mydata1"] = manPower;

            if (mtsmonth2.HasValue) this.TempData["mydata"] = mtsmonth2;

            if (pro.HasValue) this.TempData["mydata2"] = pro;
            this.ViewBag.csp = new SelectList(t, "ID", "PROJECT_NAME").OrderBy(x => x.Text);
            this.ViewBag.csmps = new SelectList(this.db.ManPowerSuppliers, "ID", "Supplier");
            if (mtsmonth2.HasValue) this.ViewBag.csdate = mtsmonth2.Value.ToLongDateString();
            var ap2 = new List<approval>();
            ap2 = this.db.approvals.Where(
                x => x.P_id == pro && x.adate == mtsmonth2 && x.MPS_id == manPower && x.status == "submitted").ToList();

            if (User.IsInRole("HR_manager"))
            {
                ap2 = this.db.approvals.Where(
                        x => x.P_id == pro && x.adate == mtsmonth2 && x.MPS_id == manPower && x.status == "approved")
                    .ToList();
            }

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

            if (ap2.Count > 0) this.ViewBag.suser = ap2.First().Susername;
            return this.View(ap2);
        }

        [Authorize(Roles = "Project_manager,HR_manager")]
        public ActionResult rejected(string why)
        {
            var da = new DateTime();
            long p = 0, mp = 0;

            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);

            long.TryParse(this.TempData["mydata2"].ToString(), out p);

            long.TryParse(this.TempData["mydata1"].ToString(), out mp);
            var suplist = db.ManPowerSuppliers.ToList();
            var proplist = db.ProjectLists.ToList();
            var sup = suplist.Find(x => x.ID == mp).Supplier;
            var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;
            if (User.IsInRole("Project_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
                var j = 0;
                foreach (var approval in ap2)
                {
                    approval.status = "rejected for " + why;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    j++;
                    if (j == 1)
                    {
                        SendMail(sup, prop, da, this.User.Identity.Name, approval.Susername,
                            "rejected", false, why, null);
                    }
                }
            }

            if (User.IsInRole("HR_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "approved").ToList();
                var j = 0;
                foreach (var approval in ap2)
                {
                    approval.status = "rejected by HR for " + why;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    j++;
                    if (j == 1)
                    {
                        SendMail(sup, prop, da, this.User.Identity.Name, approval.Ausername,
                            "rejected by HR", false, why, null);
                    }
                }
            }

            return this.RedirectToAction("PMapproval");
        }

        [Authorize(Roles = "Project_manager,HR_manager")]
        public ActionResult rejected1(string why, long? mp, long? p, DateTime? da)
        {
            var suplist = db.ManPowerSuppliers.ToList();
            var proplist = db.ProjectLists.ToList();
            var sup = suplist.Find(x => x.ID == mp).Supplier;
            var prop = proplist.Find(x => x.ID == p).PROJECT_NAME;

            if (User.IsInRole("Project_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
                var j = 0;
                foreach (var approval in ap2)
                {
                    approval.status = "rejected for " + why;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    j++;
                    if (j == 1)
                    {
                        if (da != null)
                            SendMail(sup, prop, da.Value, this.User.Identity.Name, approval.Susername,
                                "rejected", false, why, null);
                    }
                }
            }

            if (User.IsInRole("HR_manager"))
            {
                var ap2 = this.db.approvals
                    .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "approved").ToList();
                var j = 0;
                foreach (var approval in ap2)
                {
                    approval.status = "rejected by HR for " + why;
                    this.db.Entry(approval).State = EntityState.Modified;
                    this.db.SaveChanges();
                    j++;
                    if (j == 1)
                    {
                        if (da != null)
                            SendMail(sup, prop, da.Value, this.User.Identity.Name, approval.Ausername,
                                "rejected by HR", false, why, null);
                    }
                }
            }

            return this.RedirectToAction("appsum");
        }
        //this task is done according to MD.Khairy and will only show attendance of each day leaving the rest of the values blank

        public void SendMail(string sup, string prop, DateTime da, string aName, string sName, string msg, bool a_r,
            string com, string HRrole)
        {
            var man = this.db.AspNetUsers.ToList();
            var context = new ApplicationDbContext();
            var users = context.Users
                .Where(x => x.UserName == sName).ToList();
            var asa = new List<AspNetUser>();
            var cper = this.db.CsPermissions.ToList();
            foreach (var user1 in users) asa.Add(man.Find(x => x.Id == user1.Id));

            var pname = this.db.ProjectLists.ToList();
            var pname1 = pname.Find(x => x.PROJECT_NAME == prop);
            var fuser1 = new List<CsPermission>();
            foreach (var netUser in asa)
                if (cper.Exists(x => x.CsUser == netUser.csid && x.Project == pname1.ID))
                    fuser1.Add(cper.Find(x => x.CsUser == netUser.csid && x.Project == pname1.ID));
            var pno = fuser1.FindAll(x => x.Project == pname1.ID);
            var pasa = new List<AspNetUser>();
            foreach (var permission in pno) pasa.Add(asa.Find(x => x.csid == permission.CsUser));
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("timekeeper", "timekeeper@citiscapegroup.com"));
                message.To.Add(new MailboxAddress(pasa.First().UserName, pasa.First().Email));
                // string[] ccstring =
                // {
                //     "mkhairy@citiscapegroup.com", "efathy@citiscapegroup.com", "zNader@citiscapegroup.com",
                //     "amohamed@itiscapegroup.com"
                // };
                // foreach (var VARIABLE in ccstring)
                // {
                //     message.Cc.Add(new MailboxAddress(VARIABLE));
                // }
                var cspid = pasa.First().csid;
                pasa.Remove(pasa.First());
                foreach (var ccpasa in pasa) message.Cc.Add(new MailboxAddress(ccpasa.Email));
                if (!HRrole.IsNullOrWhiteSpace())
                {
                    var userHR = context.Users
                        .Where(x => x.UserName == aName).ToList();
                    foreach (var user1 in userHR)
                    {
                        message.Cc.Add(new MailboxAddress(user1.Email));
                    }
                }

                if (msg.Contains("rejected by HR") || msg.Contains("approved by HR"))
                {
                    var usertklist = context.Users
                        .Where(x => x.Roles.Select(y => y.RoleId).Contains("8840f8c3-862d-4b1e-9205-47e84c85696e"))
                        .ToList();
                    var asa1 = new List<AspNetUser>();
                    foreach (var user1 in usertklist) asa1.Add(man.Find(x => x.Id == user1.Id));
                    var fuser12 = new List<CsPermission>();
                    foreach (var netUser in asa1)
                        if (cper.Exists(x => x.CsUser == netUser.csid && x.Project == pname1.ID))
                            fuser12.Add(cper.Find(x => x.CsUser == netUser.csid && x.Project == pname1.ID));
                    var pno1 = fuser12.FindAll(x => x.Project == pname1.ID);
                    var pasa1 = new List<AspNetUser>();
                    foreach (var permission in pno1) pasa1.Add(asa1.Find(x => x.csid == permission.CsUser));
                    foreach (var usere3 in pasa1)
                    {
                        message.To.Add(new MailboxAddress(usere3.UserName, usere3.Email));
                    }
                }

                if (a_r)
                {
                    message.Subject = "TIME-SHEET APPROVED";
                    message.Body = new TextPart("plain")
                    {
                        Text = @"Dear Sir/ma'am,
Please note that the Time-Sheet for the date " + da.ToShortDateString() + ", ManPowerSupplier: " + sup +
                               " and Project name: " + prop + " has been  " + msg + "\n\nBest regards\n" + aName +
                               "\n\n\n\n"
                    };
                }
                else
                {
                    message.Subject = "TIME-SHEET rejected";
                    message.Body = new TextPart("plain")
                    {
                        Text = @"Dear Sir,
Please note that the Time-Sheet for the date " + da.ToShortDateString() + ", ManPowerSupplier: " + sup +
                               " and Project name: " + prop + " has been " + msg + " for the reason:" + com +
                               "\n\nBest regards\n" + aName +
                               "\n\n\n\n"
                    };
                }


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
        [Authorize(Users = "sdiniz,khairy,sherif,Hiba,ameer,amohamed,yahya")]
        public ActionResult timesheetsub(DateTime? subdate)
        {
            var approsubdate = new List<approval>();
            if (subdate != null)
            {
                var projlist = db.ProjectLists.Where(x => x.STATUS == "active").ToList();
                var aplist = db.approvals.Where(x => x.adate == subdate).ToList();
                var apsubdate = new List<approval>();
                foreach (var aq in aplist)
                    if (!apsubdate.Exists(x => x.P_id == aq.P_id && x.MPS_id == aq.MPS_id))
                        apsubdate.Add(aq);
                foreach (var list in projlist)
                {
                    if (apsubdate.Exists(x => x.P_id == list.ID))
                    {
                        var ap1list = apsubdate.FindAll(x => x.P_id == list.ID);
                        approsubdate.AddRange(ap1list);
                    }
                    else
                    {
                        var pmanlist = db.promanlists.Where(x => x.Project == list.ID).ToList();
                        foreach (var VARIABLE in pmanlist)
                        {
                            var attendances1 = db.MainTimeSheets.Where(x =>
                                    x.TMonth.Month == subdate.Value.Month && x.TMonth.Year == subdate.Value.Year &&
                                    x.Project == list.ID && x.ManPowerSupplier == VARIABLE.ManPowerSupplier)
                                .ToList();
                            var attendances = new List<Attendance>();
                            if (attendances1.Count !=0)
                            {
                                foreach (var sheet in attendances1)
                                {
                                    attendances.AddRange(sheet.Attendances.ToList());
                                }
                            }
                            if (attendances.Count != 0)
                            {
                                var mat = attendances.LastOrDefault();
                                var apvariable = new approval();
                                apvariable.P_id = list.ID;
                                apvariable.MPS_id = VARIABLE.ManPowerSupplier;
                                apvariable.adate = subdate;
                                apvariable.status = "not submitted";
                                apvariable.A_id = mat.ID;
                                apvariable.Attendance = mat;
                                approsubdate.Add(apvariable);
                            }
                            else
                            {
                                var mts = new MainTimeSheet();
                                var mat = new Attendance();
                                mts.ManPowerSupplier1 = VARIABLE.ManPowerSupplier1;
                                mts.ManPowerSupplier = VARIABLE.ManPowerSupplier;
                                mts.Project = VARIABLE.Project;
                                mts.ProjectList = VARIABLE.ProjectList;
                                mts.TMonth = subdate.Value;
                                mat.MainTimeSheet = mts;
                                var apvariable = new approval();
                                apvariable.P_id = list.ID;
                                apvariable.MPS_id = VARIABLE.ManPowerSupplier;
                                apvariable.adate = subdate;
                                apvariable.status = "not submitted";
                                apvariable.A_id = null;
                                apvariable.Attendance = mat;
                                approsubdate.Add(apvariable);
                            }
                        }
                    }
                }
            }

            return View(approsubdate);
        }
    }
}