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
    using Microsoft.AspNet.Identity;

    using PagedList;

    [Authorize]
    public class summaryController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: Attendances
        public ActionResult Index()
        {

            var uid = this.User.Identity.GetUserId();
            var uid1 = this.db.AspNetUsers.Find(uid);
            var projectlist = new List<ProjectList>();
            if (uid1.csid != 0 && !(this.User.IsInRole("Admin") || this.User.IsInRole("Head_of_projects") || this.User.IsInRole("HR_manager") || this.User.IsInRole("logistics_officer") || this.User.IsInRole("Admin_View")))
            {
                var scid = this.db.CsPermissions.Where(x => x.CsUser == uid1.csid).ToList();
                var t = new List<ProjectList>();
                foreach (var i in scid) t.Add(this.db.ProjectLists.Find(i.Project));
                projectlist = t;
            }
            else
            {
                projectlist = this.db.ProjectLists.ToList();
            }
            var attendances = db.Attendances.Include(a => a.LabourMaster).Include(a => a.MainTimeSheet);
            
            var att = new List<Attendance>();
            foreach (var plist in projectlist)
            {
                foreach (var attendance in attendances.Where(x => x.MainTimeSheet.Project == plist.ID))
                {
                    if (att.Exists(x=>x.MainTimeSheet.TMonth.Month == attendance.MainTimeSheet.TMonth.Month && x.MainTimeSheet.TMonth.Year
                                      == attendance.MainTimeSheet.TMonth.Year && x.MainTimeSheet.Project == plist.ID))
                    {
                        var att1 = att.Find(
                            x => x.MainTimeSheet.TMonth.Month == attendance.MainTimeSheet.TMonth.Month
                                 && x.MainTimeSheet.TMonth.Year == attendance.MainTimeSheet.TMonth.Year && x.MainTimeSheet.Project == plist.ID);
                        att.Remove(att1);
                        if (att1 != null)
                        {
                        att1.TotalHours += attendance.TotalHours;
                        att1.TotalOverTime += attendance.TotalOverTime;
                        att1.TotalAbsent += attendance.TotalAbsent;
                        att1.TotalVL += attendance.TotalVL;
                        att1.TotalTransefer += attendance.TotalTransefer;
                        att1.FridayHours += attendance.FridayHours;
                        att1.Holidays += attendance.Holidays;
                        if (attendance.LabourMaster.Position != null)
                        {
                        if (attendance.LabourMaster.Position.ToLower() =="labor".ToLower() || attendance.LabourMaster.Position.ToLower() == "LABOURER".ToLower() || attendance.LabourMaster.Position.ToLower() =="helper".ToLower())
                        {
                            att1.Unskilled += 1;
                        }
                        else
                        {
                            att1.Skilled += 1;
                        }
                            
                        }

                        att1.TotalWorkers += 1;
                        att.Add(att1);
                        
                            
                        }
                    }
                    else
                    {
                        if (attendance.LabourMaster.Position != null)
                        {
                            if (attendance.LabourMaster.Position.ToLower() == "labor".ToLower()
                                || attendance.LabourMaster.Position.ToLower() == "LABOURER".ToLower()
                                || attendance.LabourMaster.Position.ToLower() == "helper".ToLower())
                            {
                                attendance.Unskilled = 1;
                                    attendance.Skilled = 0;
                            }
                            else
                            {
                                attendance.Skilled = 1;
                                attendance.Unskilled = 0;
                            }

                        }

                        attendance.TotalWorkers = 1;
                        att.Add(attendance);
                    }
                }

            }
            return View(att.OrderByDescending(x=>x.MainTimeSheet.TMonth));
        }
        public ActionResult Rate()
        {
            var attendances = db.Attendances.Include(a => a.LabourMaster).Include(a => a.MainTimeSheet);
            var projectlist = this.db.ProjectLists.ToList();
            var att = new List<Attendance>();
            foreach (var plist in projectlist)
            {
                foreach (var attendance in attendances.Where(x => x.MainTimeSheet.Project == plist.ID))
                {
                    if (att.Exists(x => x.MainTimeSheet.TMonth.Month == attendance.MainTimeSheet.TMonth.Month && x.MainTimeSheet.TMonth.Year
                                      == attendance.MainTimeSheet.TMonth.Year && x.MainTimeSheet.Project == plist.ID))
                    {
                        var att1 = att.Find(
                            x => x.MainTimeSheet.TMonth.Month == attendance.MainTimeSheet.TMonth.Month
                                 && x.MainTimeSheet.TMonth.Year == attendance.MainTimeSheet.TMonth.Year && x.MainTimeSheet.Project == plist.ID);
                        att.Remove(att1);
                        if (att1 != null)
                        {
                            att1.TotalHours += attendance.TotalHours;
                            att1.TotalOverTime += attendance.TotalOverTime;
                            att1.TotalAbsent += attendance.TotalAbsent;
                            att1.TotalVL += attendance.TotalVL;
                            att1.TotalTransefer += attendance.TotalTransefer;
                            att1.FridayHours += attendance.FridayHours;
                            att1.Holidays += attendance.Holidays;
                            if (attendance.Holidays == null)
                            {
                                attendance.Holidays = 0;
                            }

                            if (attendance.FridayHours == null)
                            {
                                attendance.FridayHours = 0;
                            }

                            if (attendance.TotalHours == null)
                            {
                                attendance.TotalHours = 0;
                            }

                            if (attendance.TotalOverTime == null)
                            {
                                attendance.TotalOverTime = 0;
                            }
                            if (attendance.LabourMaster.Position != null)
                            {
                                if (attendance.LabourMaster.Position.ToLower() == "labor".ToLower() || attendance.LabourMaster.Position.ToLower() == "LABOURER".ToLower() || attendance.LabourMaster.Position.ToLower() == "helper".ToLower())
                                {
                                    att1.Unskilled += 1;
                                    var nt = attendance.TotalHours - attendance.TotalOverTime - attendance.Holidays
                                             - attendance.FridayHours;
                                    att1.Unskilledhours += (int)nt;
                                    att1.Unskilledrothours += (int)attendance.TotalOverTime;
                                    att1.Unskilledhothours += (int)attendance.Holidays;
                                    att1.Unskilledfothours += (int)attendance.FridayHours;
                                    att1.Unskilledthours += (int)attendance.TotalHours;
                                }
                                else
                                {
                                    att1.Skilled += 1;
                                    var nt = attendance.TotalHours - attendance.TotalOverTime - attendance.Holidays
                                             - attendance.FridayHours;
                                    att1.Skilledhours += (int)nt;
                                    att1.Skilledrothours += (int)attendance.TotalOverTime;
                                    att1.Skilledhothours += (int)attendance.Holidays;
                                    att1.Skilledfothours += (int)attendance.FridayHours;
                                    att1.Skilledthours += (int)attendance.TotalHours;
                                }

                            }

                            att1.TotalWorkers += 1;
                            att.Add(att1);


                        }
                    }
                    else
                    {
                                if (attendance.Holidays == null)
                                {
                                    attendance.Holidays = 0;
                                }
                                if (attendance.FridayHours == null)
                                {
                                    attendance.FridayHours = 0;
                                }
                                if (attendance.TotalHours == null)
                                {
                                    attendance.TotalHours = 0;
                                }
                                if (attendance.TotalOverTime == null)
                                {
                                    attendance.TotalOverTime = 0;
                                }
                        if (attendance.LabourMaster.Position != null)
                        {
                            if (attendance.LabourMaster.Position.ToLower() == "labor".ToLower()
                                || attendance.LabourMaster.Position.ToLower() == "LABOURER".ToLower()
                                || attendance.LabourMaster.Position.ToLower() == "helper".ToLower())
                            {
                                attendance.Unskilled = 1;
                                attendance.Skilled = 0;
                                var nt = attendance.TotalHours - attendance.TotalOverTime - attendance.Holidays
                                         - attendance.FridayHours;
                                attendance.Unskilledhours = (int)nt;
                                attendance.Unskilledrothours = (int)attendance.TotalOverTime;
                                attendance.Unskilledhothours = (int)attendance.Holidays;
                                attendance.Unskilledfothours = (int)attendance.FridayHours;
                                attendance.Unskilledthours = (int)attendance.TotalHours;
                                attendance.Skilledhours = 0;
                                attendance.Skilledrothours = 0;
                                attendance.Skilledhothours = 0;
                                attendance.Skilledfothours = 0;
                                attendance.Skilledthours = 0;
                            }
                            else
                            {
                                attendance.Skilled = 1;
                                attendance.Unskilled = 0;
                                var nt = attendance.TotalHours - attendance.TotalOverTime - attendance.Holidays
                                         - attendance.FridayHours;
                                attendance.Unskilledhours = 0;
                                attendance.Unskilledrothours = 0;
                                attendance.Unskilledhothours = 0;
                                attendance.Unskilledfothours = 0;
                                attendance.Unskilledthours = 0;
                                attendance.Skilledhours = (int)nt;
                                attendance.Skilledrothours = (int)attendance.TotalOverTime;
                                attendance.Skilledhothours = (int)attendance.Holidays;
                                attendance.Skilledfothours = (int)attendance.FridayHours;
                                attendance.Skilledthours = (int)attendance.TotalHours;
                            }

                        }

                        attendance.TotalWorkers = 1;
                        att.Add(attendance);
                    }
                }

            }
            return View(att.OrderByDescending(x => x.MainTimeSheet.TMonth));
        }
        public ActionResult Details(DateTime? mtsmonth, long? csmps)
        {
            var ll = new List<Attendance>();
            var atlist = new List<Attendance>();
            if (csmps.HasValue && mtsmonth.HasValue)
            {
                DateTime.TryParse(mtsmonth.Value.ToString(), out var dm);
                long.TryParse(csmps.ToString(), out var lcsmps);
                var attendances = db.Attendances.Include(a => a.LabourMaster).Include(a => a.MainTimeSheet).ToList();
                atlist = attendances.FindAll(x => x.MainTimeSheet.Project == lcsmps && x.MainTimeSheet.TMonth.Month
                                                                                   == dm.Month
                                                                                   && x.MainTimeSheet.TMonth.Year
                                                                                   == dm.Year);
                return this.View(atlist.OrderBy(x=>x.MainTimeSheet.ManPowerSupplier));
            }

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
