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
    public class AnalysisServiceController : ApiController
    {
        AnalysisManager analysisManager;

        public AnalysisServiceController()
        {
            analysisManager = new AnalysisManager();
        }

        [HttpPost]
        public Response AddNewAnalysis([FromBody] TestModel test)
        {
           return analysisManager.AddNewAnalysis(test);
        }

        [HttpPost]
        public List<ClientAnalysisRequest> ClientAnalysisRequests([FromBody] ClientAnalysisParameter clientAnalysisParameter)
        {
            return analysisManager.ClientAnalysisRequests(clientAnalysisParameter);
        }

        [HttpPost]
        public List<ClientAnalysisRequest> ClientAnalysisFilterRequests([FromBody] ClientAnalysisParameter clientAnalysisParameter)
        {
            return analysisManager.ClientAnalysisFilterRequests(clientAnalysisParameter);
        }

        [HttpGet]
        public TestModel GetSpecificTest([FromUri]long testId)
        {
            return analysisManager.GetSpecificTest(testId);
        }

        [HttpGet]
        public Response DeleteClientAnalysisRequest([FromUri]long testId)
        {
            return analysisManager.DeleteClientAnalysisRequest(testId);
        }

        [HttpGet]
        public List<LabModel> GetLaboratories()
        {
            return analysisManager.GetLaboratories();
        }

        [HttpGet]
        public List<LabBranchModel> GetLabBranches([FromUri] string fireBaseLabId, [FromUri] string userId)
        {
            return analysisManager.GetLabBranches(fireBaseLabId, userId);
        }

        [HttpGet]
        public LabBranchFullModel GetFullInfoLabBranches([FromUri] string fireBaseLabId)
        {
            return analysisManager.GetFullInfoLabBranches(fireBaseLabId);
        }


        [HttpPost]
        public List<TestModel> GetTestsBySpecificLabBranches([FromBody] FilterBranchLabModel branchObj)
        {
            return analysisManager.GetTestsBySpecificLabBranches(branchObj);
        }

        [HttpPost]
        public Response UpdateAnalysis([FromBody] TestModel test)
        {
            return analysisManager.UpdateAnalysis(test);
        }

        [HttpPost]
        public Response ConfirmAnalysis([FromBody] TestModel test)
        {
            return analysisManager.ConfirmAnalysis(test);
        }

        [HttpPost]
        public Response RefuseAnalysis([FromBody] TestModel test)
        {
            return analysisManager.RefuseAnalysis(test);
        }

        [HttpPost]
        public Response UpdateTakeSampleStatus([FromBody] TestModel test)
        {
            return analysisManager.UpdateTakeSampleStatus(test);
        }

        [HttpGet]
        public List<LabMenu> GetLabMenus([FromUri] int take, [FromUri] int skip)
        {
            return analysisManager.GetLabMenus(take,skip);
        }

        [HttpGet]
        public MainBranch GetLabBranchMenus([FromUri] string labId, [FromUri] int take, [FromUri] int skip, [FromUri] double latitude, [FromUri] double longitude,[FromUri] long governId = 0)
        {
            return analysisManager.GetLabBranchMenus(labId, take, skip, latitude, longitude,governId);
        }

        [HttpGet]
        public List<LabMenu> GetLabMenus([FromUri]string searchName)
        {
            return analysisManager.GetLabMenus(searchName);
        }

        [HttpGet]
        public List<GovenModel> GetGoverns()
        {
            return analysisManager.GetGoverns();
        }

        [HttpGet]
        public HbA1cSample GetHbA1cSampleStatistics([FromUri]string userId ,[FromUri] string year)
        {
            return analysisManager.GetHbA1cSampleStatistics(userId,year);
        }

        [HttpGet]
        public List<ReviewModel> GetBranchReviews([FromUri] string branchId, [FromUri] int take, [FromUri] int skip)
        {
            return analysisManager.GetBranchReviews(branchId,take,skip);
        }

        [HttpPost]
        public Response AddReview([FromBody] Review review)
        {
            return analysisManager.AddReview(review);
        }
        [HttpGet]
        public NofificationBadge GetNotificationNumbers([FromUri] string userId)
        {
            return analysisManager.GetNotificationNumbers(userId);
        }
        [HttpGet]
        public int GetNewRequestNotification([FromUri] string branchId)
        {
            return analysisManager.GetNewRequestNotification(branchId);
        }
        [HttpGet]
        public bool GetIsFirstDealWithBranch([FromUri] string userId, [FromUri] string branchId)
        {
            return analysisManager.GetIsFirstDealWithBranch(userId,branchId);
        }
        
        [HttpGet]
        public bool UpdateNotifiedFalse([FromUri] long testId)
        {
            return analysisManager.UpdateNotifiedFalse(testId);
        }
        [HttpGet]
        public bool UpdateNotifiedLabFalse([FromUri] long testId)
        {
            return analysisManager.UpdateNotifiedLabFalse(testId);
        }
        [HttpGet]
        public string GetName()
        {
            return "hassan";
        }

        [HttpGet]
        public List<UserTest> GetUsersByEmployeeId([FromUri] string employeeId)
        {
            return analysisManager.GetUsersByEmployeeId(employeeId);
        }

        [HttpPost]
        public Response SaveAndUpdateHealthProfile([FromBody] HealthProfileModel healthProfile)
        {
            return analysisManager.SaveAndUpdateHealthProfile(healthProfile);
        }

        [HttpGet]
        public HealthProfileModel RetrieveHealthProfile([FromUri] string userId)
        {
            return analysisManager.RetrieveHealthProfile(userId);
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
