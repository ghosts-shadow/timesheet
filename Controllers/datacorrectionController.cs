using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlygodknows.Controllers
{
    using System.Data.Entity;

    using Microsoft.Ajax.Utilities;

    using onlygodknows.Models;
    
    public class datacorrectionController : Controller
    {
        private readonly LogisticsSoftEntities db = new LogisticsSoftEntities();
        HomeController ne = new HomeController();
        // GET: datacorrection
        public ActionResult Index()
        {
            var mon = new DateTime(2020,12,1);
           newgo: var atlist = this.db.Attendances.Where(x=>x.MainTimeSheet.TMonth.Month == mon.Month && x.MainTimeSheet.TMonth.Year == mon.Year).OrderBy(x=>x.ID).ToList();
            var finallist =new List<Attendance>();
            var finallist1 =new List<Attendance>();
            foreach (var attendance in atlist)
            {
                if (finallist.Exists(
                    x => x.MainTimeSheet.ProjectList.PROJECT_NAME == attendance.MainTimeSheet.ProjectList.PROJECT_NAME
                         && x.EmpID == attendance.EmpID && x.MainTimeSheet == x.MainTimeSheet))
                {
                    finallist1.Add(attendance);
                }
                var t = new List<long>();
                var fday = ne.GetAll(attendance.MainTimeSheet.TMonth);
                var hlistday = ne.GetAllholi(attendance.MainTimeSheet.TMonth);
                double.TryParse(attendance.MainTimeSheet.ManPowerSupplier1.NormalTimeUpto.ToString(), out var tho);
                long.TryParse(attendance.C1, out var tl0);
                long.TryParse(attendance.C2, out var tl1);
                long.TryParse(attendance.C3, out var tl2);
                long.TryParse(attendance.C4, out var tl3);
                long.TryParse(attendance.C5, out var tl4);
                long.TryParse(attendance.C6, out var tl5);
                long.TryParse(attendance.C7, out var tl6);
                long.TryParse(attendance.C8, out var tl7);
                long.TryParse(attendance.C9, out var tl8);
                long.TryParse(attendance.C10, out var tl9);
                long.TryParse(attendance.C11, out var tl10);
                long.TryParse(attendance.C12, out var tl11);
                long.TryParse(attendance.C13, out var tl12);
                long.TryParse(attendance.C14, out var tl13);
                long.TryParse(attendance.C15, out var tl14);
                long.TryParse(attendance.C16, out var tl15);
                long.TryParse(attendance.C17, out var tl16);
                long.TryParse(attendance.C18, out var tl17);
                long.TryParse(attendance.C19, out var tl18);
                long.TryParse(attendance.C20, out var tl19);
                long.TryParse(attendance.C21, out var tl20);
                long.TryParse(attendance.C22, out var tl21);
                long.TryParse(attendance.C23, out var tl22);
                long.TryParse(attendance.C24, out var tl23);
                long.TryParse(attendance.C25, out var tl24);
                long.TryParse(attendance.C26, out var tl25);
                long.TryParse(attendance.C27, out var tl26);
                long.TryParse(attendance.C28, out var tl27);
                long.TryParse(attendance.C29, out var tl28);
                long.TryParse(attendance.C30, out var tl29);
                long.TryParse(attendance.C31, out var tl30);
                attendance.TotalOverTime = 0;
                attendance.TotalHours = 0;
                t.Add(tl0);
                t.Add(tl1);
                t.Add(tl2);
                t.Add(tl3);
                t.Add(tl4);
                t.Add(tl5);
                t.Add(tl6);
                t.Add(tl7);
                t.Add(tl8);
                t.Add(tl9);
                t.Add(tl10);
                t.Add(tl11);
                t.Add(tl12);
                t.Add(tl13);
                t.Add(tl14);
                t.Add(tl15);
                t.Add(tl16);
                t.Add(tl17);
                t.Add(tl18);
                t.Add(tl19);
                t.Add(tl20);
                t.Add(tl21);
                t.Add(tl22);
                t.Add(tl23);
                t.Add(tl24);
                t.Add(tl25);
                t.Add(tl26);
                t.Add(tl27);
                t.Add(tl28);
                t.Add(tl29);
                t.Add(tl30);
                long tho1 = 0;
                var i = 0;
                foreach (var l in t)
                {
                    i++;
                    if (!(fday.Exists(x => x.Equals(i)) || hlistday.Exists(x => x.Equals(i))))
                        if (l > tho)
                        {
                            tho1 += l - (long)tho;
                            attendance.TotalOverTime = tho1;
                        }

                    attendance.TotalHours += l;
                }

                long fri1 = 0;
                long holi = 0;
                var date = new DateTime(attendance.MainTimeSheet.TMonth.Year, attendance.MainTimeSheet.TMonth.Month, 1);
                for (i = 0; i < DateTime.DaysInMonth(attendance.MainTimeSheet.TMonth.Year, attendance.MainTimeSheet.TMonth.Month); i++)
                {
                    if (date.DayOfWeek.Equals(DayOfWeek.Friday))
                    {
                        if (date.Day == 1)
                        {
                            long.TryParse(attendance.C1, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 2)
                        {
                            long.TryParse(attendance.C2, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 3)
                        {
                            long.TryParse(attendance.C3, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 4)
                        {
                            long.TryParse(attendance.C4, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 5)
                        {
                            long.TryParse(attendance.C5, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 6)
                        {
                            long.TryParse(attendance.C6, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 7)
                        {
                            long.TryParse(attendance.C7, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 8)
                        {
                            long.TryParse(attendance.C8, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 9)
                        {
                            long.TryParse(attendance.C9, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 10)
                        {
                            long.TryParse(attendance.C10, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 11)
                        {
                            long.TryParse(attendance.C11, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 12)
                        {
                            long.TryParse(attendance.C11, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 13)
                        {
                            long.TryParse(attendance.C13, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 14)
                        {
                            long.TryParse(attendance.C14, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 15)
                        {
                            long.TryParse(attendance.C15, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 16)
                        {
                            long.TryParse(attendance.C16, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 17)
                        {
                            long.TryParse(attendance.C17, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 18)
                        {
                            long.TryParse(attendance.C18, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 19)
                        {
                            long.TryParse(attendance.C19, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 20)
                        {
                            long.TryParse(attendance.C20, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 21)
                        {
                            long.TryParse(attendance.C21, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 22)
                        {
                            long.TryParse(attendance.C22, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 23)
                        {
                            long.TryParse(attendance.C23, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 24)
                        {
                            long.TryParse(attendance.C24, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 25)
                        {
                            long.TryParse(attendance.C25, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 26)
                        {
                            long.TryParse(attendance.C26, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 27)
                        {
                            long.TryParse(attendance.C27, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 28)
                        {
                            long.TryParse(attendance.C28, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 29)
                        {
                            long.TryParse(attendance.C29, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 30)
                        {
                            long.TryParse(attendance.C30, out var tl);
                            fri1 = fri1 + tl;
                        }

                        if (date.Day == 31)
                        {
                            long.TryParse(attendance.C31, out var tl);
                            fri1 = fri1 + tl;
                        }

                        attendance.FridayHours = fri1;
                    }

                    date = date.AddDays(1);
                }

                var hday = this.db.Holidays.ToList();
                date = new DateTime(attendance.MainTimeSheet.TMonth.Year, attendance.MainTimeSheet.TMonth.Month, 1);
                for (i = 0; i < DateTime.DaysInMonth(attendance.MainTimeSheet.TMonth.Year, attendance.MainTimeSheet.TMonth.Month); i++)
                {
                    if (hday.Exists(x => x.Date == date))
                    {
                        if (date.Day == 1)
                        {
                            long.TryParse(attendance.C1, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 2)
                        {
                            long.TryParse(attendance.C2, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 3)
                        {
                            long.TryParse(attendance.C3, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 4)
                        {
                            long.TryParse(attendance.C4, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 5)
                        {
                            long.TryParse(attendance.C5, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 6)
                        {
                            long.TryParse(attendance.C6, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 7)
                        {
                            long.TryParse(attendance.C7, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 8)
                        {
                            long.TryParse(attendance.C8, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 9)
                        {
                            long.TryParse(attendance.C9, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 10)
                        {
                            long.TryParse(attendance.C10, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 11)
                        {
                            long.TryParse(attendance.C11, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 12)
                        {
                            long.TryParse(attendance.C11, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 13)
                        {
                            long.TryParse(attendance.C13, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 14)
                        {
                            long.TryParse(attendance.C14, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 15)
                        {
                            long.TryParse(attendance.C15, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 16)
                        {
                            long.TryParse(attendance.C16, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 17)
                        {
                            long.TryParse(attendance.C17, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 18)
                        {
                            long.TryParse(attendance.C18, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 19)
                        {
                            long.TryParse(attendance.C19, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 20)
                        {
                            long.TryParse(attendance.C20, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 21)
                        {
                            long.TryParse(attendance.C21, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 22)
                        {
                            long.TryParse(attendance.C22, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 23)
                        {
                            long.TryParse(attendance.C23, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 24)
                        {
                            long.TryParse(attendance.C24, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 25)
                        {
                            long.TryParse(attendance.C25, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 26)
                        {
                            long.TryParse(attendance.C26, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 27)
                        {
                            long.TryParse(attendance.C27, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 28)
                        {
                            long.TryParse(attendance.C28, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 29)
                        {
                            long.TryParse(attendance.C29, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 30)
                        {
                            long.TryParse(attendance.C30, out var tl);
                            holi = holi + tl;
                        }

                        if (date.Day == 31)
                        {
                            long.TryParse(attendance.C31, out var tl);
                            holi = holi + tl;
                        }

                        if (holi == null)
                        {
                            holi = 0;
                        }
                        attendance.Holidays = holi;
                    }

                    date = date.AddDays(1);
                }
                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                if (attendance.Holidays == null)
                {
                    attendance.Holidays = 0;
                }
                {
                    i = 0;
                    attendance.TotalSickLeave = 0;
                    long ts = 0;
                    if (!attendance.C1.IsNullOrWhiteSpace())
                    {
                        if (attendance.C1.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C2.IsNullOrWhiteSpace())
                    {
                        if (attendance.C2.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C3.IsNullOrWhiteSpace())
                    {
                        if (attendance.C3.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C4.IsNullOrWhiteSpace())
                    {
                        if (attendance.C4.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C5.IsNullOrWhiteSpace())
                    {
                        if (attendance.C5.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C6.IsNullOrWhiteSpace())
                    {
                        if (attendance.C6.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C7.IsNullOrWhiteSpace())
                    {
                        if (attendance.C7.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C8.IsNullOrWhiteSpace())
                    {
                        if (attendance.C8.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C9.IsNullOrWhiteSpace())
                    {
                        if (attendance.C9.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C10.IsNullOrWhiteSpace())
                    {
                        if (attendance.C10.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C11.IsNullOrWhiteSpace())
                    {
                        if (attendance.C11.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C12.IsNullOrWhiteSpace())
                    {
                        if (attendance.C12.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C13.IsNullOrWhiteSpace())
                    {
                        if (attendance.C13.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C14.IsNullOrWhiteSpace())
                    {
                        if (attendance.C14.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C15.IsNullOrWhiteSpace())
                    {
                        if (attendance.C15.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C16.IsNullOrWhiteSpace())
                    {
                        if (attendance.C16.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C17.IsNullOrWhiteSpace())
                    {
                        if (attendance.C17.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C18.IsNullOrWhiteSpace())
                    {
                        if (attendance.C18.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C19.IsNullOrWhiteSpace())
                    {
                        if (attendance.C19.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C20.IsNullOrWhiteSpace())
                    {
                        if (attendance.C20.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C21.IsNullOrWhiteSpace())
                    {
                        if (attendance.C21.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C22.IsNullOrWhiteSpace())
                    {
                        if (attendance.C22.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C23.IsNullOrWhiteSpace())
                    {
                        if (attendance.C23.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C24.IsNullOrWhiteSpace())
                    {
                        if (attendance.C24.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C25.IsNullOrWhiteSpace())
                    {
                        if (attendance.C25.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C26.IsNullOrWhiteSpace())
                    {
                        if (attendance.C26.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C27.IsNullOrWhiteSpace())
                    {
                        if (attendance.C27.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C28.IsNullOrWhiteSpace())
                    {
                        if (attendance.C28.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C29.IsNullOrWhiteSpace())
                    {
                        if (attendance.C29.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C30.IsNullOrWhiteSpace())
                    {
                        if (attendance.C30.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                ts = ts + 1;
                        i++;
                    }

                    if (!attendance.C31.IsNullOrWhiteSpace())
                    {
                        if (attendance.C31.Equals("S"))
                            if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                ts = ts + 1;
                        i++;
                    }

                    attendance.TotalSickLeave = ts;
                }
                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                {
                    i = 0;
                    attendance.TotalVL = 0;
                    long tv = 0;
                    if (!attendance.C1.IsNullOrWhiteSpace())
                    {
                        if (attendance.C1.Equals("V"))

                            if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C2.IsNullOrWhiteSpace())
                    {
                        if (attendance.C2.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C3.IsNullOrWhiteSpace())
                    {
                        if (attendance.C3.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C4.IsNullOrWhiteSpace())
                    {
                        if (attendance.C4.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C5.IsNullOrWhiteSpace())
                    {
                        if (attendance.C5.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C6.IsNullOrWhiteSpace())
                    {
                        if (attendance.C6.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C7.IsNullOrWhiteSpace())
                    {
                        if (attendance.C7.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C8.IsNullOrWhiteSpace())
                    {
                        if (attendance.C8.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C9.IsNullOrWhiteSpace())
                    {
                        if (attendance.C9.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C10.IsNullOrWhiteSpace())
                    {
                        if (attendance.C10.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C11.IsNullOrWhiteSpace())
                    {
                        if (attendance.C11.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C12.IsNullOrWhiteSpace())
                    {
                        if (attendance.C12.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C13.IsNullOrWhiteSpace())
                    {
                        if (attendance.C13.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C14.IsNullOrWhiteSpace())
                    {
                        if (attendance.C14.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C15.IsNullOrWhiteSpace())
                    {
                        if (attendance.C15.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C16.IsNullOrWhiteSpace())
                    {
                        if (attendance.C16.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C17.IsNullOrWhiteSpace())
                    {
                        if (attendance.C17.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C18.IsNullOrWhiteSpace())
                    {
                        if (attendance.C18.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C19.IsNullOrWhiteSpace())
                    {
                        if (attendance.C19.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C20.IsNullOrWhiteSpace())
                    {
                        if (attendance.C20.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C21.IsNullOrWhiteSpace())
                    {
                        if (attendance.C21.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C22.IsNullOrWhiteSpace())
                    {
                        if (attendance.C22.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C23.IsNullOrWhiteSpace())
                    {
                        if (attendance.C23.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C24.IsNullOrWhiteSpace())
                    {
                        if (attendance.C24.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C25.IsNullOrWhiteSpace())
                    {
                        if (attendance.C25.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C26.IsNullOrWhiteSpace())
                    {
                        if (attendance.C26.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C27.IsNullOrWhiteSpace())
                    {
                        if (attendance.C27.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C28.IsNullOrWhiteSpace())
                    {
                        if (attendance.C28.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C29.IsNullOrWhiteSpace())
                    {
                        if (attendance.C29.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C30.IsNullOrWhiteSpace())
                    {
                        if (attendance.C30.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C31.IsNullOrWhiteSpace())
                    {
                        if (attendance.C31.Equals("V"))
                            if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                tv = tv + 1;

                        i++;
                    }

                    attendance.TotalVL = tv;
                }
                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                {
                    i = 0;
                    attendance.TotalAbsent = 0;
                    long tv = 0;
                    if (!attendance.C1.IsNullOrWhiteSpace())
                    {
                        if (attendance.C1.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C2.IsNullOrWhiteSpace())
                    {
                        if (attendance.C2.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C3.IsNullOrWhiteSpace())
                    {
                        if (attendance.C3.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C4.IsNullOrWhiteSpace())
                    {
                        if (attendance.C4.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                tv = tv + 1;
                        i++;
                    }

                    if (!attendance.C5.IsNullOrWhiteSpace())
                    {
                        if (attendance.C5.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C6.IsNullOrWhiteSpace())
                    {
                        if (attendance.C6.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C7.IsNullOrWhiteSpace())
                    {
                        if (attendance.C7.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C8.IsNullOrWhiteSpace())
                    {
                        if (attendance.C8.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C9.IsNullOrWhiteSpace())
                    {
                        if (attendance.C9.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C10.IsNullOrWhiteSpace())
                    {
                        if (attendance.C10.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C11.IsNullOrWhiteSpace())
                    {
                        if (attendance.C11.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C12.IsNullOrWhiteSpace())
                    {
                        if (attendance.C12.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C13.IsNullOrWhiteSpace())
                    {
                        if (attendance.C13.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C14.IsNullOrWhiteSpace())
                    {
                        if (attendance.C14.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C15.IsNullOrWhiteSpace())
                    {
                        if (attendance.C15.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C16.IsNullOrWhiteSpace())
                    {
                        if (attendance.C16.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C17.IsNullOrWhiteSpace())
                    {
                        if (attendance.C17.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C18.IsNullOrWhiteSpace())
                    {
                        if (attendance.C18.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C19.IsNullOrWhiteSpace())
                    {
                        if (attendance.C19.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C20.IsNullOrWhiteSpace())
                    {
                        if (attendance.C20.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C21.IsNullOrWhiteSpace())
                    {
                        if (attendance.C21.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C22.IsNullOrWhiteSpace())
                    {
                        if (attendance.C22.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C23.IsNullOrWhiteSpace())
                    {
                        if (attendance.C23.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C24.IsNullOrWhiteSpace())
                    {
                        if (attendance.C24.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C25.IsNullOrWhiteSpace())
                    {
                        if (attendance.C25.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C26.IsNullOrWhiteSpace())
                    {
                        if (attendance.C26.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C27.IsNullOrWhiteSpace())
                    {
                        if (attendance.C27.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C28.IsNullOrWhiteSpace())
                    {
                        if (attendance.C28.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C29.IsNullOrWhiteSpace())
                    {
                        if (attendance.C29.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C30.IsNullOrWhiteSpace())
                    {
                        if (attendance.C30.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C31.IsNullOrWhiteSpace())
                    {
                        if (attendance.C31.Equals("A"))
                            if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                tv = tv + 1;

                        i++;
                    }

                    attendance.TotalAbsent = tv;
                }
                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                {
                    i = 0;
                    attendance.TotalTransefer = 0;
                    long tv = 0;
                    if (!attendance.C1.IsNullOrWhiteSpace())
                    {
                        if (attendance.C1.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(1)) || hlistday.Exists(x => x.Equals(1))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C2.IsNullOrWhiteSpace())
                    {
                        if (attendance.C2.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(2)) || hlistday.Exists(x => x.Equals(2))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C3.IsNullOrWhiteSpace())
                    {
                        if (attendance.C3.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(3)) || hlistday.Exists(x => x.Equals(3))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C4.IsNullOrWhiteSpace())
                    {
                        if (attendance.C4.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(4)) || hlistday.Exists(x => x.Equals(4))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C5.IsNullOrWhiteSpace())
                    {
                        if (attendance.C5.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(5)) || hlistday.Exists(x => x.Equals(5))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C6.IsNullOrWhiteSpace())
                    {
                        if (attendance.C6.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(6)) || hlistday.Exists(x => x.Equals(6))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C7.IsNullOrWhiteSpace())
                    {
                        if (attendance.C7.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(7)) || hlistday.Exists(x => x.Equals(7))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C8.IsNullOrWhiteSpace())
                    {
                        if (attendance.C8.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(8)) || hlistday.Exists(x => x.Equals(8))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C9.IsNullOrWhiteSpace())
                    {
                        if (attendance.C9.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(9)) || hlistday.Exists(x => x.Equals(9))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C10.IsNullOrWhiteSpace())
                    {
                        if (attendance.C10.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(10)) || hlistday.Exists(x => x.Equals(10))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C11.IsNullOrWhiteSpace())
                    {
                        if (attendance.C11.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(11)) || hlistday.Exists(x => x.Equals(11))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C12.IsNullOrWhiteSpace())
                    {
                        if (attendance.C12.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(12)) || hlistday.Exists(x => x.Equals(12))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C13.IsNullOrWhiteSpace())
                    {
                        if (attendance.C13.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(13)) || hlistday.Exists(x => x.Equals(13))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C14.IsNullOrWhiteSpace())
                    {
                        if (attendance.C14.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(14)) || hlistday.Exists(x => x.Equals(14))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C15.IsNullOrWhiteSpace())
                    {
                        if (attendance.C15.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(15)) || hlistday.Exists(x => x.Equals(15))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C16.IsNullOrWhiteSpace())
                    {
                        if (attendance.C16.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(16)) || hlistday.Exists(x => x.Equals(16))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C17.IsNullOrWhiteSpace())
                    {
                        if (attendance.C17.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(17)) || hlistday.Exists(x => x.Equals(17))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C18.IsNullOrWhiteSpace())
                    {
                        if (attendance.C18.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(18)) || hlistday.Exists(x => x.Equals(18))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C19.IsNullOrWhiteSpace())
                    {
                        if (attendance.C19.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(19)) || hlistday.Exists(x => x.Equals(19))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C20.IsNullOrWhiteSpace())
                    {
                        if (attendance.C20.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(20)) || hlistday.Exists(x => x.Equals(20))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C21.IsNullOrWhiteSpace())
                    {
                        if (attendance.C21.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(21)) || hlistday.Exists(x => x.Equals(21))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C22.IsNullOrWhiteSpace())
                    {
                        if (attendance.C22.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(22)) || hlistday.Exists(x => x.Equals(22))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C23.IsNullOrWhiteSpace())
                    {
                        if (attendance.C23.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(23)) || hlistday.Exists(x => x.Equals(23))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C24.IsNullOrWhiteSpace())
                    {
                        if (attendance.C24.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(24)) || hlistday.Exists(x => x.Equals(24))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C25.IsNullOrWhiteSpace())
                    {
                        if (attendance.C25.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(25)) || hlistday.Exists(x => x.Equals(25))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C26.IsNullOrWhiteSpace())
                    {
                        if (attendance.C26.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(26)) || hlistday.Exists(x => x.Equals(26))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C27.IsNullOrWhiteSpace())
                    {
                        if (attendance.C27.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(27)) || hlistday.Exists(x => x.Equals(27))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C28.IsNullOrWhiteSpace())
                    {
                        if (attendance.C28.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(28)) || hlistday.Exists(x => x.Equals(28))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C29.IsNullOrWhiteSpace())
                    {
                        if (attendance.C29.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(29)) || hlistday.Exists(x => x.Equals(29))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C30.IsNullOrWhiteSpace())
                    {
                        if (attendance.C30.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(30)) || hlistday.Exists(x => x.Equals(30))))
                                tv = tv + 1;

                        i++;
                    }

                    if (!attendance.C31.IsNullOrWhiteSpace())
                    {
                        if (attendance.C31.Equals("T"))
                            if (!(fday.Exists(x => x.Equals(31)) || hlistday.Exists(x => x.Equals(31))))
                                tv = tv + 1;

                        i++;
                    }

                    attendance.TotalTransefer = tv;
                }

                this.db.Entry(attendance).State = EntityState.Modified;
                this.db.SaveChanges();
                finallist.Add(attendance);
            }

            if (finallist1.Count() > 0)
            {
                var x = 0;
            }
            return View(finallist);
        }
    }
}