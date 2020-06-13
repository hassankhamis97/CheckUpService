using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class LabBranchDB
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string govern { get; set; }
        public string image { get; set; }
        public string phone { get; set; }
        public bool? isAvailableFromHome { get; set; }
        public string timeFrom { get; set; }
        public string timeTo { get; set; }
        public string holidays { get; set; }
        public double? longitude { get; set; }
        public double? latitude { get; set; }
        public string FireBaseId { get; set; }
        public long? LabId { get; set; }
        public long? addressId { get; set; }
        public double? rating { get; set; }
        public long? reviewId { get; set; }
        public long? governId { get; set; }

        public Address address { get; set; }
    }
}