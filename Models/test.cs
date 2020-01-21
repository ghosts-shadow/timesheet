using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlygodknows.Models
{
    using System.ComponentModel.DataAnnotations;

    public class testlist
    {
        public List<test> Tests { get; set; }
    }

    public class test
    {
        public long empno { get; set; }
        public string hours { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }

        public string approved_by { get; set; }

        public string submitted_by { get; set; }
    }
}