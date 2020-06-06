using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class LabBranchFullModel
    {
        public long Id { get; set; }
        public string govern { get; set; }
        public string branchPhoto { get; set; }
        public string phone { get; set; }
        public Nullable<bool> isAvailableFromHome { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public string holidays { get; set; }
        //public double? longitude { get; set; }
        //public double? latitude { get; set; }
        public string branchId { get; set; }
        public string labId { get; set; }
        public string labName { get; set; }

        public AddressModel address { get; set; }
    }
}