//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onlygodknows.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class overtimeemployeelist
    {
        public int Id { get; set; }
        public Nullable<long> lab_no { get; set; }
        public Nullable<System.DateTime> effectivedate { get; set; }
        public Nullable<int> hrs { get; set; }
        public Nullable<int> otref { get; set; }
        public string hopAP { get; set; }
        public string HRAP { get; set; }
        public string status { get; set; }
    
        public virtual LabourMaster LabourMaster { get; set; }
        public virtual overtimeref overtimeref { get; set; }
    }
}
