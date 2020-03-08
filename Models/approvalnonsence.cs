using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlygodknows.Models
{
    using System.ComponentModel.DataAnnotations;

    public class approvalnonsence
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> adate { get; set; }

        public Nullable<long> P_id { get; set; }

        public Nullable<long> MPS_id { get; set; }

        public string P_name { get; set; }

        public string MPS_name { get; set; }
        public Nullable<long> N_T { get; set; }
        public Nullable<long> O_T { get; set; }
        public Nullable<long> friday { get; set; }

        public virtual Attendance Attendance { get; set; }
    }
}