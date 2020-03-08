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

namespace onlygodknows.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using onlygodknows.Models;

    public class ProjmanController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();

        private int i = 0;

        [Authorize(Roles = "Project_manager")]
        public ActionResult approved()
        {
            /*var t = new List<ProjectList>();
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
            else
            {
                t = this.db.ProjectLists.ToList();
            }

            var pl = new List<ProjectList>();
            var t1 = t.First();
            long f1 = t1.ID;
            foreach (var v in t)
            {

                if (v.ID == f1)
                {
                    pl.Add(v);
                }
            }

            var pl1 = pl.First();
            var ap1 = new List<approval>();
            ap1 = this.db.approvals.Where(x => x.P_id == pl1.ID && x.status == "submitted").ToList();*/
            var da = new DateTime();
            long p = 0, mp = 0;

            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);

            long.TryParse(this.TempData["mydata2"].ToString(), out p);

            long.TryParse(this.TempData["mydata1"].ToString(), out mp);

            var ap2 = this.db.approvals
                .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
            foreach (var approval in ap2)
            {
                approval.status = "approved";
                approval.Ausername = this.User.Identity.Name;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            return this.RedirectToAction("PMapproval");
        }

        [Authorize(Roles = "Project_manager")]
        public ActionResult approved1(long? mp, long? p, DateTime? da)
        {
            var ap2 = this.db.approvals
                .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
            foreach (var approval in ap2)
            {
                approval.status = "approved";
                approval.Ausername = this.User.Identity.Name;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            return this.RedirectToAction("appsum");
        }

        public ActionResult appsum()
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

            var ap2 = new List<approval>();
            foreach (var listp in t)
            {
                var aaa = this.db.approvals.Where(x => x.P_id == listp.ID && x.status == "submitted").ToList();

                foreach (var approval in aaa)
                    if (!ap2.Exists(x => x.MPS_id == approval.MPS_id && x.adate == approval.adate))
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
                var atlist = this.db.Attendances.Where(x => x.SubMain == mdknonsence.Attendance.SubMain);
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

            return new JsonResult { Data = aap, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        [Authorize(Roles = "Project_manager")]
        public ActionResult rejected(string why)
        {
            var da = new DateTime();
            long p = 0, mp = 0;

            DateTime.TryParse(this.TempData["mydata"].ToString(), out da);

            long.TryParse(this.TempData["mydata2"].ToString(), out p);

            long.TryParse(this.TempData["mydata1"].ToString(), out mp);

            var ap2 = this.db.approvals
                .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
            foreach (var approval in ap2)
            {
                approval.status = "rejected for " + why;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            return this.RedirectToAction("PMapproval");
        }

        [Authorize(Roles = "Project_manager")]
        public ActionResult rejected1(string why, long? mp, long? p, DateTime? da)
        {
            var ap2 = this.db.approvals
                .Where(x => x.adate == da && x.MPS_id == mp && x.P_id == p && x.status == "submitted").ToList();
            foreach (var approval in ap2)
            {
                approval.status = "rejected for " + why;
                this.db.Entry(approval).State = EntityState.Modified;
                this.db.SaveChanges();
            }

            return this.RedirectToAction("appsum");
        }
        //this task is done according to MD.Khairy and will only show attendance of each day leaving the rest of the values blank
    }
}