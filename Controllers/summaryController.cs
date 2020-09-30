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

    public class summaryController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: Attendances
        public ActionResult Index()
        {
            var attendances = db.Attendances.Include(a => a.LabourMaster).Include(a => a.MainTimeSheet);
            var projectlist = this.db.ProjectLists.ToList();
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
                        att.Add(att1);
                            
                        }
                    }
                    else
                    {
                        att.Add(attendance);
                    }
                }

            }
            return View(att.OrderByDescending(x=>x.MainTimeSheet.TMonth));
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
