using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class TestModel
    {
        public long id { get; set; }
        public string userId { get; set; }
        public Nullable<long> userIdDB { get; set; }
        public string description { get; set; }
        public Nullable<bool> isFromHome { get; set; }
        public string dateRequest { get; set; }
        public string timeRequest { get; set; }
        public string dateForTakingSample { get; set; }
        public string timeForTakingSample { get; set; }
        public string dateResult { get; set; }
        public string timeResult { get; set; }
        public string branchId { get; set; }
        public Nullable<long> branchIdDB { get; set; }
        public string labId { get; set; }
        public Nullable<long> labIdDB { get; set; }
        public Nullable<long> addressId { get; set; }
        public string hba1c { get; set; }
        public string status { get; set; }
        public bool? isNotified { get; set; }
        public string totalCost { get; set; }
        public string precautions { get; set; }
        public string employeeId { get; set; }
        public string radioReason { get; set; }
        public string generatedCode { get; set; }
        public Nullable<long> timeStampRequest { get; set; }

        public AddressModel address { get; set; }
        public List<string> testName { get; set; }
        public List<string> resultFilespaths { get; set; }
        public List<string> roushettaPaths { get; set; }
        public string refuseReason { get; set; }
        public DateTime? dateRequestFormat { get; set; }

    }
}