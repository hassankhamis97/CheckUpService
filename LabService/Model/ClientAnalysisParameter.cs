using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class ClientAnalysisParameter
    {
        public string userId { get; set; }
        public List<string> status { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
        public String dateFrom { get; set; }
        public String dateTo { get; set; }

        public DateTime? dateFromFormat { get; set; }
        public DateTime? dateToFormat { get; set; }
        public List<string> labIds { get; set; }


    }
}