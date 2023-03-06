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
    public class access_dateController : Controller
    {
        private LogisticsSoftEntities db = new LogisticsSoftEntities();

        // GET: access_date
        public ActionResult Index()
        {
            var empnolist = db.LabourMasters.ToList();
            var projectlist = db.ProjectLists.ToList();
            var accdatalist = db.access_date.Where(x => x.modified_date.Day == DateTime.Today.Day && x.modified_date.Month == DateTime.Today.Month && x.modified_date.Year == DateTime.Today.Year).ToList();
            //var accdatalist = db.access_date.Where(x => x.modified_date.Day == 15 && x.modified_date.Month == 12 && x.modified_date.Year == 2022).ToList();
            var maintimelist = db.MainTimeSheets.ToList();
            var mpslist = db.ManPowerSuppliers.ToList();
            var temp = new access_date();
            foreach (var entrydata in accdatalist)
            {
                var emp = empnolist.Find(x => x.EMPNO == entrydata.emp_no);
                if (emp == null)
                {
                    goto end;
                }
                var entrydate = entrydata.entrydate;
                var mps = mpslist.Find(x => x.ID == emp.ManPowerSupply);
                var project = projectlist.Find(x => x.ID == entrydata.project_id);
                var tempmts = new MainTimeSheet();
                if (maintimelist.Exists(x =>
                    x.TMonth.Month == entrydate.Value.Month &&
                    x.TMonth.Year == entrydate.Value.Year && x.Project == project.ID &&
                    x.ManPowerSupplier == emp.ManPowerSupply))
                {
                     tempmts = maintimelist.Find(x =>
                        x.TMonth.Month == entrydate.Value.Month &&
                        x.TMonth.Year == entrydate.Value.Year && x.Project == project.ID &&
                        x.ManPowerSupplier == emp.ManPowerSupply);
                }
                else
                {
                    tempmts.ManPowerSupplier = emp.ManPowerSupply;
                    tempmts.Project = project.ID;
                    tempmts.TMonth = entrydate.Value;
                    db.MainTimeSheets.Add(tempmts);
                    db.SaveChanges();
                    maintimelist = db.MainTimeSheets.ToList();
                    tempmts = maintimelist.Find(x =>
                        x.TMonth.Month == entrydate.Value.Month && x.Project == project.ID &&
                        x.ManPowerSupplier == emp.ManPowerSupply);
                }

                var tempatt = new Attendance();
                if (tempmts.Attendances != null && tempmts.Attendances.Count != 0)
                {
                    var notpresent = false;
                    var test = tempmts.Attendances.ToList();
                    tempatt = test.Find(x => x.EmpID == emp.ID);
                    if (tempatt == null)
                    {
                        notpresent = true; 
                        tempatt = new Attendance();
                        tempatt.EmpID = emp.ID;
                        tempatt.SubMain = tempmts.ID;
                        tempatt.TotalHours = 0;
                    }
                    if (entrydate.Value.Day == 1)
                    {
                        tempatt.C1 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 2)
                    {
                        tempatt.C2 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 3)
                    {
                        tempatt.C3 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 4)
                    {
                        tempatt.C4 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 5)
                    {
                        tempatt.C5 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 6)
                    {
                        tempatt.C6 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 7)
                    {
                        tempatt.C7 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 8)
                    {
                        tempatt.C8 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 9)
                    {
                        tempatt.C9 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 10)
                    {
                        tempatt.C10 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 11)
                    {
                        tempatt.C11 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 12)
                    {
                        tempatt.C12 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 13)
                    {
                        tempatt.C13 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 14)
                    {
                        tempatt.C14 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 15)
                    {
                        tempatt.C15 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 16)
                    {
                        tempatt.C16 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 17)
                    {
                        tempatt.C17 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 18)
                    {
                        tempatt.C18 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 19)
                    {
                        tempatt.C19 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 20)
                    {
                        tempatt.C20 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 21)
                    {
                        tempatt.C21 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 22)
                    {
                        tempatt.C22 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 23)
                    {
                        tempatt.C23 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 24)
                    {
                        tempatt.C24 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 25)
                    {
                        tempatt.C25 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 26)
                    {
                        tempatt.C26 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 27)
                    {
                        tempatt.C27 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 28)
                    {
                        tempatt.C28 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 29)
                    {
                        tempatt.C29 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 30)
                    {
                        tempatt.C30 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 31)
                    {
                        tempatt.C31 = entrydata.hours;
                    }

                    if (tempatt.TotalHours.HasValue)
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours += tempcal;
                    }
                    else
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours = tempcal;
                    }
                    if (tempatt.TotalAbsent.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent = 1;
                        }
                    }
                    if (tempatt.TotalSickLeave.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave = 1;
                        }
                    }
                    if (tempatt.TotalTransefer.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer = 1;
                        }
                    }
                    if (tempatt.TotalVL.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL = 1;
                        }
                    }
                    if (tempatt.TotalOverTime.HasValue)
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime += (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    else
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime = (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    if (notpresent)
                    {
                        db.Attendances.Add(tempatt);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(tempatt).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    tempatt.EmpID = emp.ID;
                    tempatt.SubMain = tempmts.ID;
                    if (entrydate.Value.Day == 1)
                    {
                        tempatt.C1 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 2)
                    {
                        tempatt.C2 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 3)
                    {
                        tempatt.C3 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 4)
                    {
                        tempatt.C4 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 5)
                    {
                        tempatt.C5 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 6)
                    {
                        tempatt.C6 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 7)
                    {
                        tempatt.C7 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 8)
                    {
                        tempatt.C8 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 9)
                    {
                        tempatt.C9 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 10)
                    {
                        tempatt.C10 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 11)
                    {
                        tempatt.C11 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 12)
                    {
                        tempatt.C12 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 13)
                    {
                        tempatt.C13 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 14)
                    {
                        tempatt.C14 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 15)
                    {
                        tempatt.C15 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 16)
                    {
                        tempatt.C16 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 17)
                    {
                        tempatt.C17 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 18)
                    {
                        tempatt.C18 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 19)
                    {
                        tempatt.C19 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 20)
                    {
                        tempatt.C20 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 21)
                    {
                        tempatt.C21 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 22)
                    {
                        tempatt.C22 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 23)
                    {
                        tempatt.C23 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 24)
                    {
                        tempatt.C24 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 25)
                    {
                        tempatt.C25 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 26)
                    {
                        tempatt.C26 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 27)
                    {
                        tempatt.C27 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 28)
                    {
                        tempatt.C28 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 29)
                    {
                        tempatt.C29 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 30)
                    {
                        tempatt.C30 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 31)
                    {
                        tempatt.C31 = entrydata.hours;
                    }

                    if (tempatt.TotalHours.HasValue)
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours += tempcal;
                    }
                    else
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours = tempcal;
                    }
                    if (tempatt.TotalAbsent.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent = 1;
                        }
                    }
                    if (tempatt.TotalSickLeave.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave = 1;
                        }
                    }
                    if (tempatt.TotalTransefer.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer = 1;
                        }
                    }
                    if (tempatt.TotalVL.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL = 1;
                        }
                    }
                    if (tempatt.TotalOverTime.HasValue)
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime += (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    else
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime = (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    db.Attendances.Add(tempatt);
                    db.SaveChanges();
                }
                end: ;
            }

            return View();
        }

        public void getdata(DateTime searchdate,int proid)
        {
            var ap1 = this.db.approvals.ToList();
            var empnolist = db.LabourMasters.ToList();
            var projectlist = db.ProjectLists.ToList();
            //var accdatalist = db.access_date.Where(x => x.modified_date.Day == DateTime.Today.Day && x.modified_date.Month == DateTime.Today.Month && x.modified_date.Year == DateTime.Today.Year).ToList();
            var accdatalist = db.access_date.Where(x => /*x.entrydate.Value.Day == searchdate.Day &&*/ x.entrydate.Value.Month == searchdate.Month && x.entrydate.Value.Year == searchdate.Year && x.project_id == proid).ToList();
            var maintimelist = db.MainTimeSheets.ToList(); 
             var mpslist = db.ManPowerSuppliers.ToList(); 
             var temp = new access_date();
            foreach (var entrydata in accdatalist)
            {
                var emp = empnolist.Find(x => x.EMPNO == entrydata.emp_no);
                if (emp == null)
                {
                    goto end;
                }
                var entrydate = entrydata.entrydate;
                var mps = mpslist.Find(x => x.ID == emp.ManPowerSupply);
                var project = projectlist.Find(x => x.ID == entrydata.project_id);
                var tempmts = new MainTimeSheet();
                if (maintimelist.Exists(x =>
                    x.TMonth.Month == entrydate.Value.Month &&
                    x.TMonth.Year == entrydate.Value.Year && x.Project == project.ID &&
                    x.ManPowerSupplier == emp.ManPowerSupply))
                {
                    tempmts = maintimelist.Find(x =>
                       x.TMonth.Month == entrydate.Value.Month &&
                       x.TMonth.Year == entrydate.Value.Year && x.Project == project.ID &&
                       x.ManPowerSupplier == emp.ManPowerSupply);
                }
                else
                {
                    tempmts.ManPowerSupplier = emp.ManPowerSupply;
                    tempmts.Project = project.ID;
                    tempmts.TMonth = entrydate.Value;
                    db.MainTimeSheets.Add(tempmts);
                    db.SaveChanges();
                    maintimelist = db.MainTimeSheets.ToList();
                    tempmts = maintimelist.Find(x =>
                        x.TMonth.Month == entrydate.Value.Month && x.Project == project.ID &&
                        x.ManPowerSupplier == emp.ManPowerSupply);
                }

                var tempatt = new Attendance();
                if (tempmts.Attendances != null && tempmts.Attendances.Count != 0)
                {
                    var ap = ap1.FindAll(x =>
                    x.MPS_id == tempmts.ManPowerSupplier && x.P_id == tempmts.Project);
                    var notpresent = false;
                    var test = tempmts.Attendances.ToList();
                    tempatt = test.Find(x => x.EmpID == emp.ID);
                    if (ap.Exists(
                        x => x.adate == entrydate
                             && x.status == "approved"))
                    {
                        goto end;
                    }
                    if (tempatt == null)
                    {
                        notpresent = true;
                        tempatt = new Attendance();
                        tempatt.EmpID = emp.ID;
                        tempatt.SubMain = tempmts.ID;
                        tempatt.TotalHours = 0;
                    }
                    if (entrydate.Value.Day == 1)
                    {
                        tempatt.C1 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 2)
                    {
                        tempatt.C2 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 3)
                    {
                        tempatt.C3 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 4)
                    {
                        tempatt.C4 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 5)
                    {
                        tempatt.C5 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 6)
                    {
                        tempatt.C6 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 7)
                    {
                        tempatt.C7 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 8)
                    {
                        tempatt.C8 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 9)
                    {
                        tempatt.C9 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 10)
                    {
                        tempatt.C10 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 11)
                    {
                        tempatt.C11 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 12)
                    {
                        tempatt.C12 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 13)
                    {
                        tempatt.C13 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 14)
                    {
                        tempatt.C14 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 15)
                    {
                        tempatt.C15 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 16)
                    {
                        tempatt.C16 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 17)
                    {
                        tempatt.C17 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 18)
                    {
                        tempatt.C18 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 19)
                    {
                        tempatt.C19 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 20)
                    {
                        tempatt.C20 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 21)
                    {
                        tempatt.C21 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 22)
                    {
                        tempatt.C22 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 23)
                    {
                        tempatt.C23 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 24)
                    {
                        tempatt.C24 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 25)
                    {
                        tempatt.C25 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 26)
                    {
                        tempatt.C26 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 27)
                    {
                        tempatt.C27 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 28)
                    {
                        tempatt.C28 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 29)
                    {
                        tempatt.C29 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 30)
                    {
                        tempatt.C30 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 31)
                    {
                        tempatt.C31 = entrydata.hours;
                    }

                    if (tempatt.TotalHours.HasValue)
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours += tempcal;
                    }
                    else
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours = tempcal;
                    }
                    if (tempatt.TotalAbsent.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent = 1;
                        }
                    }
                    if (tempatt.TotalSickLeave.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave = 1;
                        }
                    }
                    if (tempatt.TotalTransefer.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer = 1;
                        }
                    }
                    if (tempatt.TotalVL.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL = 1;
                        }
                    }
                    if (tempatt.TotalOverTime.HasValue)
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime += (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    else
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime = (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    if (notpresent)
                    {
                        db.Attendances.Add(tempatt);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(tempatt).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    tempatt.EmpID = emp.ID;
                    tempatt.SubMain = tempmts.ID;
                    if (entrydate.Value.Day == 1)
                    {
                        tempatt.C1 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 2)
                    {
                        tempatt.C2 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 3)
                    {
                        tempatt.C3 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 4)
                    {
                        tempatt.C4 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 5)
                    {
                        tempatt.C5 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 6)
                    {
                        tempatt.C6 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 7)
                    {
                        tempatt.C7 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 8)
                    {
                        tempatt.C8 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 9)
                    {
                        tempatt.C9 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 10)
                    {
                        tempatt.C10 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 11)
                    {
                        tempatt.C11 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 12)
                    {
                        tempatt.C12 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 13)
                    {
                        tempatt.C13 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 14)
                    {
                        tempatt.C14 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 15)
                    {
                        tempatt.C15 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 16)
                    {
                        tempatt.C16 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 17)
                    {
                        tempatt.C17 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 18)
                    {
                        tempatt.C18 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 19)
                    {
                        tempatt.C19 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 20)
                    {
                        tempatt.C20 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 21)
                    {
                        tempatt.C21 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 22)
                    {
                        tempatt.C22 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 23)
                    {
                        tempatt.C23 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 24)
                    {
                        tempatt.C24 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 25)
                    {
                        tempatt.C25 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 26)
                    {
                        tempatt.C26 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 27)
                    {
                        tempatt.C27 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 28)
                    {
                        tempatt.C28 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 29)
                    {
                        tempatt.C29 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 30)
                    {
                        tempatt.C30 = entrydata.hours;
                    }
                    if (entrydate.Value.Day == 31)
                    {
                        tempatt.C31 = entrydata.hours;
                    }

                    if (tempatt.TotalHours.HasValue)
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours += tempcal;
                    }
                    else
                    {
                        var tempcal = 0;
                        int.TryParse(entrydata.hours, out tempcal);
                        tempatt.TotalHours = tempcal;
                    }
                    if (tempatt.TotalAbsent.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "A")
                        {
                            tempatt.TotalAbsent = 1;
                        }
                    }
                    if (tempatt.TotalSickLeave.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "S")
                        {
                            tempatt.TotalSickLeave = 1;
                        }
                    }
                    if (tempatt.TotalTransefer.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "T")
                        {
                            tempatt.TotalTransefer = 1;
                        }
                    }
                    if (tempatt.TotalVL.HasValue)
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL += 1;
                        }
                    }
                    else
                    {
                        if (entrydata.hours.ToUpper() == "V")
                        {
                            tempatt.TotalVL = 1;
                        }
                    }
                    if (tempatt.TotalOverTime.HasValue)
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime += (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    else
                    {
                        var tempcal = 0d;
                        double.TryParse(entrydata.hours, out tempcal);
                        if (tempcal > mps.NormalTimeUpto)
                        {
                            tempatt.TotalOverTime = (long)(tempcal - mps.NormalTimeUpto);
                        }
                    }
                    db.Attendances.Add(tempatt);
                    db.SaveChanges();
                }
            end:;
            }

        }
        /*
        // GET: access_date/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            access_date access_date = db.access_date.Find(id);
            if (access_date == null)
            {
                return HttpNotFound();
            }
            return View(access_date);
        }

        // GET: access_date/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: access_date/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,emp_no,entrydate,hours,project_id,modified_date")] access_date access_date)
        {
            if (ModelState.IsValid)
            {
                db.access_date.Add(access_date);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(access_date);
        }

        // GET: access_date/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            access_date access_date = db.access_date.Find(id);
            if (access_date == null)
            {
                return HttpNotFound();
            }
            return View(access_date);
        }

        // POST: access_date/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,emp_no,entrydate,hours,project_id,modified_date")] access_date access_date)
        {
            if (ModelState.IsValid)
            {
                db.Entry(access_date).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(access_date);
        }

        // GET: access_date/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            access_date access_date = db.access_date.Find(id);
            if (access_date == null)
            {
                return HttpNotFound();
            }
            return View(access_date);
        }

        // POST: access_date/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            access_date access_date = db.access_date.Find(id);
            db.access_date.Remove(access_date);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

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