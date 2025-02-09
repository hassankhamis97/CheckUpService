//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Address
    {
        public Address()
        {
            this.LabBranches = new HashSet<LabBranch>();
            this.Tests = new HashSet<Test>();
        }
    
        public long Id { get; set; }
        public string address1 { get; set; }
        public string buildingNo { get; set; }
        public string floorNo { get; set; }
        public string apartmentNo { get; set; }
        public Nullable<double> longitude { get; set; }
        public Nullable<double> latitude { get; set; }
    
        public virtual ICollection<LabBranch> LabBranches { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
