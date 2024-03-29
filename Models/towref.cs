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
    
    public partial class towref
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public towref()
        {
            this.towemps = new HashSet<towemp>();
            this.trattfiles = new HashSet<trattfile>();
        }
    
        public int Id { get; set; }
        [Display(Name = "your reference")]
        public string refe1 { get; set; }

        [Display(Name = "to")]
        public Nullable<long> mp_to { get; set; }

        [Display(Name = "from")]
        public Nullable<long> mp_from { get; set; }
        public string R_no { get; set; }

        [Display(Name = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> mpcdate { get; set; }

        public bool AR { get; set; }
    
        public virtual ProjectList ProjectList { get; set; }
        public virtual ProjectList ProjectList1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<towemp> towemps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trattfile> trattfiles { get; set; }
    }
}
