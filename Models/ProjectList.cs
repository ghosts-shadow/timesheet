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
    
    public partial class ProjectList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectList()
        {
            this.MainTimeSheets = new HashSet<MainTimeSheet>();
            this.towrefs = new HashSet<towref>();
            this.towrefs1 = new HashSet<towref>();
            this.Manpowerinoutforms = new HashSet<Manpowerinoutform>();
        }
    
        public long ID { get; set; }

        [Display(Name = "project code")]
        public string ProjectCode { get; set; }

        [Display(Name = "project")]
        public string STAMP_CODE { get; set; }

        public string INQUIRY { get; set; }

        [Display(Name = "project name")]
        public string PROJECT_NAME { get; set; }

        public string CLIENT_MC { get; set; }

        public string Notice_01 { get; set; }

        public string Notice_02 { get; set; }

        public string SCOPE_OF_WORK { get; set; }

        [Display(Name = "start date")]
        public Nullable<System.DateTime> START_DATE { get; set; }

        [Display(Name = "end date")]
        public Nullable<System.DateTime> END_DATE { get; set; }

        public string STATUS { get; set; }

        public Nullable<System.DateTime> INSURANCE_STATUS { get; set; }

        public string LOCATION { get; set; }

        [Display(Name = "PM name")]
        public string PM { get; set; }

        public string PM_CONTACT { get; set; }

        public string Completion_Certificate { get; set; }

        public string REMARKS { get; set; }

        public Nullable<double> Notice { get; set; }

        public string Source { get; set; }

        public bool Closed { get; set; }

        public string Encoded_Absolute_URL { get; set; }

        public string Item_Type { get; set; }

        public string Path { get; set; }

        public string URL_Path { get; set; }

        public string Workflow_Instance_ID { get; set; }

        public string File_Type { get; set; }

        [Display(Name = "excuted by")]
        public string excute_by { get; set; }

        [Display(Name = "project period")]
        public string project_period { get; set; }

        [Display(Name = "equipment budget")]
        public Nullable<decimal> equipment_budget { get; set; }

        [Display(Name = "man power budget")]
        public Nullable<decimal> man_power_budget { get; set; }
        public Nullable<bool> rate_w_wo_at { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MainTimeSheet> MainTimeSheets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<towref> towrefs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<towref> towrefs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Manpowerinoutform> Manpowerinoutforms { get; set; }
    }
}
