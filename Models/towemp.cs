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
    using System.ComponentModel.DataAnnotations;

    public partial class towemp
    {
        public int Id { get; set; }
        public Nullable<long> lab_no { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> effectivedate { get; set; }
        public Nullable<int> rowref { get; set; }
        public string ARstatus { get; set; }
        public string app_by { get; set; }
    
        public virtual LabourMaster LabourMaster { get; set; }
        public virtual towref towref { get; set; }
    }
}
