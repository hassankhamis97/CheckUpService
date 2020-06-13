using LabService.BL;
using LabService.Enum;
using LabService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LabService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class LabController : ApiController
    {

         AnalysisManager analysisManager;

         public LabController()
        {
            analysisManager = new AnalysisManager();
        }

        [HttpPost]
        public Response addLab([FromBody] Laboratory laboratory)
        {
            return analysisManager.addLab(laboratory);
        }

        [HttpPost]
        public Response addLabBranch([FromBody] LabBranchDB labBranch)
        {
            return analysisManager.addLabBranch(labBranch);
        }
    }
}
