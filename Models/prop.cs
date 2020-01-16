using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlygodknows.Models
{
    public class proplist
    {
        public List<prop> props { get; set; }
    }
    public class prop
    {
        public int empno { get; set; }

        public long projectid { get; set; }
    }
}