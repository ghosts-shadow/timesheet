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
    
    public partial class promanlist
    {
        public int Id { get; set; }
        public long ManPowerSupplier { get; set; }
        public long Project { get; set; }
    
        public virtual ManPowerSupplier ManPowerSupplier1 { get; set; }
        public virtual ProjectList ProjectList { get; set; }
    }
}
