using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class HbA1cSample
    {
        public List<string> year { get; set; }
        public List<double> sample { get; set; }
    }
}