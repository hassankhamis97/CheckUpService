using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class ClientAnalysisRequest
    {
        public long id { get; set; }
        public string dateRequest { get; set; }
        public string status { get; set; }
        public string labName { get; set; }
        public string labPhoto { get; set; }
        public bool? isNotified { get; set; }


    }
}