using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlygodknows.Models
{
    public class timesheetViewModel
    {
        public onlygodknows.Models.Attendance attendance { get; set; }
        public IEnumerable<onlygodknows.Models.Attendance> Attendancecollection { get; set; }
    }
}