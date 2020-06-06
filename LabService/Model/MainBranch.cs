using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LabService.Model
{
    public class MainBranch
    {

        public string labHotline { get; set; }
        public string labPhoto { get; set; }
        public List<LabBranchMenu> branches { get; set; }
    }
}