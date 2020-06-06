using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class FilterBranchLabModel
    {
        public string labBranchFireBaseId { get; set; }
        public List<string> Status { get; set; }
    }
}