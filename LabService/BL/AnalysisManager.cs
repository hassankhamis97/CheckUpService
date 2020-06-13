using LabService.Enum;
using LabService.Model;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace LabService.BL
{
    public class AnalysisManager
    {
        CheckUpEntities checkUpContext;
        public AnalysisManager()
        {
            checkUpContext = new CheckUpEntities();
        }



        internal Response AddNewAnalysis(TestModel test)
        {
            Response response;

            try
            {

                Test testDb = CopyAToB(test);
                testDb.isNotifiedLab = true;
                testDb.isNotified = false;
                DateTime dateNow = DateTime.UtcNow;
                dateNow.AddHours(4);
                testDb.dateRequestFormat = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, dateNow.Hour, dateNow.Minute, dateNow.Second);
                Laboratory laboratory = checkUpContext.Laboratories.Where(w => w.FireBaseId == testDb.labId).FirstOrDefault();

                if (laboratory != null)
                {
                    testDb.labIdDB = laboratory.Id;
                }

                LabBranch labBranch = checkUpContext.LabBranches.Where(w => w.FireBaseId == testDb.branchId).FirstOrDefault();

                if (labBranch != null)
                {
                    testDb.branchIdDB = labBranch.Id;
                }

                testDb.TestNames = new List<TestName>();

                for (int i = 0; i < test.testName.Count; i++)
                {
                    testDb.TestNames.Add(new TestName() { Name = test.testName[i] });
                }

                testDb.ResultFilespaths = new List<ResultFilespath>();

                for (int i = 0; i < test.resultFilespaths.Count; i++)
                {
                    testDb.ResultFilespaths.Add(new ResultFilespath() { Name = test.resultFilespaths[i] });
                }

                testDb.RoushettaPaths = new List<RoushettaPath>();

                for (int i = 0; i < test.roushettaPaths.Count; i++)
                {
                    testDb.RoushettaPaths.Add(new RoushettaPath() { Name = test.roushettaPaths[i] });
                }
                if (test.address != null)
                {
                    Address addressObj = new Address();
                    addressObj.address1 = test.address.address1;
                    addressObj.apartmentNo = test.address.apartmentNo;
                    addressObj.floorNo = test.address.floorNo;
                    addressObj.buildingNo = test.address.buildingNo;

                    addressObj.longitude = test.address.longitude;
                    addressObj.latitude = test.address.latitude;

                    testDb.Address = addressObj;
                }


                checkUpContext.Tests.Add(testDb);
                checkUpContext.SaveChanges();
                response = Response.Success;
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }

            return response;
        }

        private Test CopyAToB(TestModel test)
        {
            Test testDb = new Test();
            var typeOfA = test.GetType();
            var typeOfB = testDb.GetType();

            foreach (var propertyOfA in typeOfA.GetProperties())
            {
                var propertyOfB = typeOfB.GetProperty(propertyOfA.Name);

                if (propertyOfB != null)
                {
                    propertyOfB.SetValue(testDb, propertyOfA.GetValue(test));
                }
            }

            return testDb;
        }

        internal List<ClientAnalysisRequest> ClientAnalysisRequests(ClientAnalysisParameter clientAnalysisParameter)
        {
            List<ClientAnalysisRequest> response = new List<ClientAnalysisRequest>();
            try
            {
                response = checkUpContext.Tests.Where(w => w.userId == clientAnalysisParameter.userId &&
                                           clientAnalysisParameter.status.Contains(w.status))
                                           .OrderByDescending(o => o.dateRequestFormat)
                                           .Skip(clientAnalysisParameter.skip)
                                           .Take(clientAnalysisParameter.take)
                                           .Select(s => new ClientAnalysisRequest()
                                           {
                                               id = s.Id,
                                               status = s.status,
                                               dateRequest = s.dateRequest,
                                               labName = s.Laboratory != null ? s.Laboratory.name : string.Empty,
                                               labPhoto = s.Laboratory != null ? s.Laboratory.image : string.Empty,
                                               isNotified = s.isNotified
                                           }).ToList();
            }
            catch (Exception ex)
            {
                response = null;
            }

            return response;
        }

        internal List<ClientAnalysisRequest> ClientAnalysisFilterRequests(ClientAnalysisParameter clientAnalysisParameter)
        {
            List<ClientAnalysisRequest> lst = new List<ClientAnalysisRequest>();

            try
            {
                List<long> labDbIds;
                if (clientAnalysisParameter.dateFrom != null)
                {
                    clientAnalysisParameter.dateFromFormat = DateTime.ParseExact(clientAnalysisParameter.dateFrom, "MMM d, yyyy", null);
                    clientAnalysisParameter.dateToFormat = DateTime.ParseExact(clientAnalysisParameter.dateTo, "MMM d, yyyy", null);
                }
                //if (clientAnalysisParameter.labIds.Count > 0)
                //{
                labDbIds = checkUpContext.Laboratories.Where(w => clientAnalysisParameter.labIds.Contains(w.FireBaseId))
                .Select(s => s.Id).ToList();
                //}

                lst = checkUpContext.Tests.ToList().Where(w => w.userId == clientAnalysisParameter.userId &&
                                                clientAnalysisParameter.status.Contains(w.status)
                                                && (!labDbIds.Any() || labDbIds.Contains(w.labIdDB ?? 0))
                                                && (!clientAnalysisParameter.dateFromFormat.HasValue || (
                                                 !clientAnalysisParameter.dateToFormat.HasValue
                                                 && w.dateRequestFormat.Value.Date == clientAnalysisParameter.dateFromFormat.Value.Date
                                                )
                                                || (w.dateRequestFormat.Value.Date >= clientAnalysisParameter.dateFromFormat.Value.Date
                                                && w.dateRequestFormat.Value.Date <= clientAnalysisParameter.dateToFormat.Value.Date)
                                                ))
                                                .OrderByDescending(o => o.dateRequestFormat)
                                                .Skip(clientAnalysisParameter.skip)
                                                .Take(clientAnalysisParameter.take)
                                                .Select(s => new ClientAnalysisRequest()
                                                {
                                                    id = s.Id,
                                                    status = s.status,
                                                    dateRequest = s.dateRequest,
                                                    labName = s.Laboratory != null ? s.Laboratory.name : string.Empty,
                                                    labPhoto = s.Laboratory != null ? s.Laboratory.image : string.Empty,
                                                    isNotified = s.isNotified
                                                }).ToList();
            }
            catch (Exception ex)
            {
                lst = null;
            }

            return lst;
        }

        internal Response DeleteClientAnalysisRequest(long id)
        {
            Response response;

            try
            {
                List<TestName> testNames = checkUpContext.TestNames.Where(w => w.TestId == id).ToList();
                testNames.Clear();

                List<ResultFilespath> resultFilespath = checkUpContext.ResultFilespaths.Where(w => w.TestId == id).ToList();
                resultFilespath.Clear();

                List<RoushettaPath> roushettaPath = checkUpContext.RoushettaPaths.Where(w => w.TestId == id).ToList();
                roushettaPath.Clear();

                Test test = checkUpContext.Tests.Where(w => w.Id == id).FirstOrDefault();

                if (test != null)
                {
                    checkUpContext.Tests.Remove(test);
                }

                checkUpContext.SaveChanges();
                response = Response.Success;
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }
            return response;

        }

        internal TestModel GetSpecificTest(long testId)
        {
            TestModel response = new TestModel();

            try
            {
                Test test = checkUpContext.Tests.Where(w => w.Id == testId).FirstOrDefault();

                if (test != null)
                {
                    response.dateRequest = test.dateRequest;
                    response.userId = test.userId;
                    response.userIdDB = test.userIdDB;
                    response.description = test.description;
                    response.isFromHome = test.isFromHome;
                    response.dateRequest = test.dateRequest;
                    response.timeRequest = test.timeRequest;
                    response.dateForTakingSample = test.dateForTakingSample;
                    response.timeForTakingSample = test.timeForTakingSample;
                    response.dateResult = test.dateResult;
                    response.timeResult = test.timeResult;
                    response.branchId = test.branchId;
                    response.branchIdDB = test.branchIdDB;
                    response.labId = test.labId;
                    response.labIdDB = test.labIdDB;
                    response.addressId = test.addressId;
                    response.hba1c = test.hba1c;
                    response.status = test.status;
                    response.isNotified = test.isNotified;
                    response.totalCost = test.totalCost;
                    response.precautions = test.precautions;
                    response.employeeId = test.employeeId;
                    response.radioReason = test.radioReason;
                    response.generatedCode = test.generatedCode;
                    response.timeStampRequest = test.timeStampRequest;
                    response.testName = test.TestNames.Select(s => s.Name).ToList();
                    response.resultFilespaths = test.ResultFilespaths.Select(s => s.Name).ToList();
                    response.roushettaPaths = test.RoushettaPaths.Select(s => s.Name).ToList();
                    response.refuseReason = test.refuseReason;
                    response.dateRequestFormat = test.dateRequestFormat;
                    response.id = test.Id;
                    if (response.isFromHome == true)
                    {
                        response.address = MapToAddress(test.Address);
                    }

                }
            }
            catch (Exception ex)
            {

                response = null;
            }

            return response;
        }

        private TestModel MapTestModelToTest(Test test)
        {
            TestModel testModel = new TestModel();
            var typeOfA = test.GetType();
            var typeOfB = testModel.GetType();

            foreach (var propertyOfA in typeOfA.GetProperties())
            {
                var propertyOfB = typeOfB.GetProperty(propertyOfA.Name);

                if (propertyOfB != null)
                {
                    propertyOfB.SetValue(testModel, propertyOfA.GetValue(test));
                }
            }

            return testModel;
        }

        internal List<LabModel> GetLaboratories()
        {
            List<LabModel> lst = new List<LabModel>();

            try
            {
                lst = checkUpContext.Laboratories.Select(s => new LabModel()
                 {
                     id = s.Id,
                     name = s.name,
                     fireBaseId = s.FireBaseId
                 }).ToList();
            }
            catch (Exception ex)
            {
                lst = null;
            }
            return lst;
        }

        internal List<LabBranchModel> GetLabBranches(string fireBaseLabId, string userId)
        {
            List<LabBranchModel> lst = new List<LabBranchModel>();
            try
            {
                lst = checkUpContext.LabBranches.Where(w => w.Laboratory.FireBaseId == fireBaseLabId)
                .Select(s => new LabBranchModel()
                {
                    id = s.Id,
                    name = s.govern,
                    fireBaseId = s.FireBaseId,

                }).ToList();
            }
            catch (Exception ex)
            {
                lst = null;
            }
            return lst;
        }

        internal LabBranchFullModel GetFullInfoLabBranches(string fireBaseLabId)
        {
            LabBranchFullModel response = new LabBranchFullModel();

            try
            {
                response = checkUpContext.LabBranches.Where(w => w.FireBaseId == fireBaseLabId).AsEnumerable()
                .Select(s => new LabBranchFullModel()
                {
                    Id = s.Id,
                    govern = s.Govern1 != null ? s.Govern1.Name : null,
                    branchId = s.FireBaseId,
                    branchPhoto = s.Laboratory != null ? s.Laboratory.image : null,
                    isAvailableFromHome = s.isAvailableFromHome,
                    phone = s.phone,
                    timeFrom = s.timeFrom,
                    timeTo = s.timeTo,
                    labName = s.Laboratory != null ? s.Laboratory.name : string.Empty,
                    labId = s.Laboratory != null ? s.Laboratory.FireBaseId : string.Empty,
                    address = MapToAddress(s.Address),
                    holidays = s.holidays,
                    ratingReviewNo = s.Reviews.Any() ? s.Reviews.Where(w => w.rateNumber.HasValue).Average(a => a.rateNumber ?? 0) : 0
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                response = null;
            }
            return response;
        }



        internal List<TestModel> GetTestsBySpecificLabBranches(FilterBranchLabModel branchObj)
        {
            List<TestModel> response = new List<TestModel>();

            try
            {
                response = checkUpContext.Tests.Where(w => w.branchId == branchObj.labBranchFireBaseId &&
                 branchObj.Status.Contains(w.status)
                ).OrderByDescending(o => o.dateRequestFormat).Select(s => new TestModel()
                {
                    id = s.Id,
                    dateRequest = s.dateRequest,
                    userId = s.userId,
                    userIdDB = s.userIdDB,
                    description = s.description,
                    isFromHome = s.isFromHome,
                    timeRequest = s.timeRequest,
                    dateForTakingSample = s.dateForTakingSample,
                    timeForTakingSample = s.timeForTakingSample,
                    dateResult = s.dateResult,
                    timeResult = s.timeResult,
                    branchId = s.branchId,
                    branchIdDB = s.branchIdDB,
                    labId = s.labId,
                    labIdDB = s.labIdDB,
                    addressId = s.addressId,
                    hba1c = s.hba1c,
                    status = s.status,
                    isNotified = s.isNotified,
                    totalCost = s.totalCost,
                    precautions = s.precautions,
                    employeeId = s.employeeId,
                    radioReason = s.radioReason,
                    generatedCode = s.generatedCode,
                    timeStampRequest = s.timeStampRequest,
                    testName = s.TestNames.Select(a => a.Name).ToList(),
                    resultFilespaths = s.ResultFilespaths.Select(a => a.Name).ToList(),
                    roushettaPaths = s.RoushettaPaths.Select(a => a.Name).ToList(),
                    refuseReason = s.refuseReason,
                    dateRequestFormat = s.dateRequestFormat
                }).OrderByDescending(o => o.dateRequestFormat).ToList();
            }
            catch (Exception)
            {
                response = null;
            }

            return response;
        }



        internal Response UpdateAnalysis(TestModel test)
        {
            Response response = Response.Success;

            try
            {
                Test testDb = checkUpContext.Tests.Where(w => w.Id == test.id).FirstOrDefault();

                if (testDb != null)
                {
                    DateTime dateNow = DateTime.UtcNow;
                    dateNow.AddHours(4);
                    testDb.description = string.IsNullOrEmpty(test.description) ? null : test.description;
                    testDb.hba1c = string.IsNullOrEmpty(test.hba1c) ? null : test.hba1c;
                    testDb.status = string.IsNullOrEmpty(test.status) ? null : test.status;
                    testDb.isNotified = true;
                    testDb.dateResult = dateNow.ToString("MMM dd, yyyy");
                    testDb.timeResult = dateNow.ToString("h:mm tt");

                    if (testDb.ResultFilespaths == null)
                    {
                        testDb.ResultFilespaths = new List<ResultFilespath>();
                    }

                    for (int i = 0; i < test.resultFilespaths.Count; i++)
                    {
                        testDb.ResultFilespaths.Add(new ResultFilespath() { Name = test.resultFilespaths[i] });
                    }

                }

                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }

            return response;
        }

        internal Response ConfirmAnalysis(TestModel test)
        {
            Response response = Response.Success;

            try
            {
                Test testDb = checkUpContext.Tests.Where(w => w.Id == test.id).FirstOrDefault();

                if (testDb != null)
                {
                    testDb.totalCost = string.IsNullOrEmpty(test.totalCost) ? null : test.totalCost;
                    testDb.generatedCode = string.IsNullOrEmpty(test.generatedCode) ? null : test.generatedCode;
                    testDb.precautions = string.IsNullOrEmpty(test.precautions) ? null : test.precautions;
                    testDb.employeeId = string.IsNullOrEmpty(test.employeeId) ? null : test.employeeId;
                    testDb.status = string.IsNullOrEmpty(test.status) ? null : test.status;
                    testDb.isNotified = true;
                    testDb.isNotifiedLab = false;

                }
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {

                response = Response.Fail;
            }

            return response;
        }

        internal Response RefuseAnalysis(TestModel test)
        {
            Response response = Response.Success;
            try
            {
                Test testDb = checkUpContext.Tests.Where(w => w.Id == test.id).FirstOrDefault();

                if (testDb != null)
                {
                    testDb.radioReason = string.IsNullOrEmpty(test.radioReason) ? null : test.radioReason;
                    testDb.refuseReason = string.IsNullOrEmpty(test.refuseReason) ? null : test.refuseReason;
                    testDb.status = string.IsNullOrEmpty(test.status) ? null : test.status;
                    testDb.isNotified = true;
                    testDb.isNotifiedLab = false;

                }
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }

            return response;
        }

        internal Response UpdateTakeSampleStatus(TestModel test)
        {
            Response response = Response.Success;
            try
            {
                Test testDb = checkUpContext.Tests.Where(w => w.Id == test.id).FirstOrDefault();

                if (testDb != null)
                {
                    testDb.status = string.IsNullOrEmpty(test.status) ? null : test.status;
                    if (testDb.status != "Canceled") { 
                      testDb.isNotified = true;
                    }
                }
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {

                response = Response.Fail;
            }

            return response;
        }

        internal List<LabMenu> GetLabMenus(int take, int skip)
        {
            List<LabMenu> labMenus = new List<LabMenu>();
            try
            {
                labMenus = checkUpContext.Laboratories.OrderByDescending(o => o.rating).Skip(skip).Take(take).Select(s => new LabMenu()
            {
                idFB = s.FireBaseId,
                hotline = s.hotline,
                labPhoto = s.image,
                labName = s.name,
                rating = s.rating ?? 0
            }).ToList();
            }
            catch (Exception ex)
            {

                labMenus = null;
            }

            return labMenus;
        }

        internal List<LabMenu> GetLabMenus(string searchName)
        {
            List<LabMenu> labMenus = new List<LabMenu>();
            try
            {
                 labMenus = checkUpContext.Laboratories.Where(w => w.name.Contains(searchName)).Select(s => new LabMenu()
            {
                idFB = s.FireBaseId,
                hotline = s.hotline,
                labPhoto = s.image,
                labName = s.name,
                rating = s.rating ?? 0
            }).OrderByDescending(o => o.rating).ToList();
            }
            catch (Exception)
            {
                labMenus = null;
            }

            return labMenus;
        }

        internal MainBranch GetLabBranchMenus(string labId, int take, int skip, double latitude, double longitude, long governId)
        {
            MainBranch labAndItsBranches = new MainBranch();
            GeoCoordinate userGeoCoordinate = new GeoCoordinate(latitude, longitude);

            try
            {
                 labAndItsBranches = checkUpContext.Laboratories.Where(w => w.FireBaseId == labId).AsEnumerable().Select(s => new MainBranch()
            {
                labHotline = s.hotline,
                labPhoto = s.image,
                branches = s.LabBranches.Where(w => (governId == 0 || w.governId == governId)).Skip(skip).Take(take).Select(c => new LabBranchMenu()
                {
                    idFB = c.FireBaseId,
                    govern = c.Govern1 != null ? c.Govern1.Name : string.Empty,
                    address = MapToAddress(c.Address),
                    rating = c.rating ?? 0,
                    isAvailableFromHome = c.isAvailableFromHome ?? false,
                    userGeoCoordinate = userGeoCoordinate,
                    branchGeoCoordinate = new GeoCoordinate(c.Address.latitude ?? 0, c.Address.longitude ?? 0),
                }).OrderBy(o => o.distance).ToList()

            }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                labAndItsBranches = null;
            }

            return labAndItsBranches;
        }

        private AddressModel MapToAddress(Address address)
        {
            AddressModel addressObj = new AddressModel();
            addressObj.Id = address.Id;
            addressObj.apartmentNo = address.apartmentNo;
            addressObj.buildingNo = address.buildingNo;
            addressObj.address1 = address.address1;
            addressObj.floorNo = address.floorNo;
            addressObj.latitude = address.latitude ?? 0;
            addressObj.longitude = address.longitude ?? 0;
            return addressObj;
        }
        private static long ConvertDateTimeToTimestamp(DateTime value)
        {
            TimeSpan epoch = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            //return the total seconds (which is a UNIX timestamp)
            return Convert.ToInt64(epoch.TotalSeconds);
        }
        //internal List<LabBranchMenu> GetLabBranchMenus()
        //{
        //    List<LabBranchMenu> labbranches = checkUpContext.LabBranches.Select(s => new LabBranchMenu()
        //    {

        //        idFB = s.FireBaseId,
        //        govern = s.govern,
        //        Address = s.Address != null ? s.Address.address1 : string.Empty,
        //        rating = s.rating ?? 0,
        //        distance = 0,
        //        labHotline = s.Laboratory != null ? s.Laboratory.hotline : string.Empty,
        //        labPhoto = s.Laboratory != null ? s.Laboratory.image : string.Empty
        //    }).OrderByDescending(o => o.rating).ToList();

        //    return labbranches;
        //}

        internal List<GovenModel> GetGoverns()
        {
            List<GovenModel> governLst = new List<GovenModel>();

            try
            {
                governLst = checkUpContext.Governs.Select(s => new GovenModel()
                {
                    id = s.Id,
                    name = s.Name
                }).ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
            return governLst;
        }

        internal HbA1cSample GetHbA1cSampleStatistics(string userId, string year)
        {
            HbA1cSample response = new HbA1cSample();

            try
            {
                var tests = checkUpContext.Tests.Where(w => w.userId == userId && w.hba1c != null && w.dateRequest.Contains(year)).Select(s => new
                {
                    year = s.dateRequest,
                    sample = s.hba1c
                }).ToList();
                response.year = tests.Select(s => s.year).ToList();
                response.sample = tests.Select(s => Convert.ToDouble(s.sample)).ToList();
            }
            catch (Exception ex)
            {
                response = null;
            }
          
            return response;
        }

        internal List<ReviewModel> GetBranchReviews(string branchId, int take, int skip)
        {
            List<ReviewModel> response = new List<ReviewModel>();
            try
            {
                 response = checkUpContext.Reviews
                .OrderByDescending(o => o.createdDate)
                .Skip(skip)
                .Take(take)
                .AsEnumerable()
                .Select(s => new ReviewModel()
                {
                    id = s.Id,
                    rateNumber = s.rateNumber,
                    description = s.description,
                    userId = s.userId,
                    date = s.createdDate.Value.ToString("MMM d, yyyy")
                }).ToList();
            }
            catch (Exception ex)
            {
                response = null;
            }
            return response;
        }

        internal Response AddReview(Review review)
        {
            Response response = Response.Success;

            try
            {
                review.createdDate = DateTime.UtcNow;
                review.createdDate.Value.AddHours(4);
                //review.createdDate = DateTime.Now;
                checkUpContext.Reviews.Add(review);
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }
           
            return response;
        }

        internal NofificationBadge GetNotificationNumbers(string userId)
        {
            NofificationBadge nofificationBadge = new NofificationBadge();

            try
            {
                nofificationBadge.requestBadge = checkUpContext.Tests.Where(w => w.userId == userId && w.isNotified == true && w.status != "Done").Count();
                nofificationBadge.resultBadge = checkUpContext.Tests.Where(w => w.userId == userId && w.isNotified == true && w.status == "Done").Count();
            }
            catch (Exception ex)
            {
                nofificationBadge = null;
            }

           
            return nofificationBadge;
        }
        internal int GetNewRequestNotification(string branchId)
        {
            int count;

            try
            {
                count = checkUpContext.Tests.Where(w => w.branchId == branchId && w.isNotifiedLab == true).Count();

            }
            catch (Exception ex)
            {
                count = 0;
            }


            return count;
        }
        internal bool GetIsFirstDealWithBranch(string userId, string branchId)
        {
            bool isFound = true;
            int count = checkUpContext.Tests.Where(w => w.userId == userId && w.branchId == branchId).Count();
            if (count == 1)
            {
                isFound = false;
            }
            return isFound;
        }
        internal bool UpdateNotifiedFalse(long testId)
        {
            try
            {
                Test test = checkUpContext.Tests.Where(w => w.Id == testId).FirstOrDefault();
                if (test != null)
                {
                    test.isNotified = false;
                    checkUpContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        internal bool UpdateNotifiedLabFalse(long testId)
        {
            try
            {
                Test test = checkUpContext.Tests.Where(w => w.Id == testId).FirstOrDefault();
                if (test != null)
                {
                    test.isNotifiedLab = false;
                    checkUpContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        internal List<UserTest> GetUsersByEmployeeId(string employeeId)
        {
            List<UserTest> lst = new List<UserTest>();

            try
            {
                lst = checkUpContext.Tests.Where(w => w.employeeId == employeeId).ToList()
                    
                    .Select(s =>

                    new UserTest() {
                      TestId = s.Id,
                      UserId = s.userId,
                      dateForTakingSample = s.dateForTakingSample,
                      timeForTakingSample = s.timeForTakingSample,
                      generatedCode = s.generatedCode,
                      address = s.Address != null ? MapToAddress(s.Address) : new AddressModel()
                    }).OrderBy(o => o.TakingSampleDate).ToList();
            }
            catch (Exception ex)
            {
                lst = null;
            }
            return lst;
        }

        internal Response SaveAndUpdateHealthProfile(HealthProfileModel healthProfile)
        {
            Response response = Response.Success;
            try
            {
                if (healthProfile.Id == 0)
                {
                    HealthProfile HealthProfileDB = new HealthProfile();
                    HealthProfileDB.isSTakeantiBiotic = healthProfile.isSTakeantiBiotic;
                    HealthProfileDB.isSuffreDiabetes = healthProfile.isSuffreDiabetes;
                    HealthProfileDB.isSuffrePressure = healthProfile.isSuffrePressure;
                    HealthProfileDB.isTakehaemophilia = healthProfile.isTakehaemophilia;
                    HealthProfileDB.userId = healthProfile.userId;
                    HealthProfileDB.DieaseNames = new List<DieaseName>();

                    for (int i = 0; i < healthProfile.dieaseNamesArray.Count; i++)
                    {
                        HealthProfileDB.DieaseNames.Add(new DieaseName() { Name = healthProfile.dieaseNamesArray[i] });
                    }

                    checkUpContext.HealthProfiles.Add(HealthProfileDB);
                }
                else
                {
                    HealthProfile HealthProfileDB = checkUpContext.HealthProfiles.Where(w => w.Id == healthProfile.Id).FirstOrDefault();

                    if (HealthProfileDB != null)
                    {
                        HealthProfileDB.isSTakeantiBiotic = healthProfile.isSTakeantiBiotic;
                        HealthProfileDB.isSuffreDiabetes = healthProfile.isSuffreDiabetes;
                        HealthProfileDB.isSuffrePressure = healthProfile.isSuffrePressure;
                        HealthProfileDB.isTakehaemophilia = healthProfile.isTakehaemophilia;
                        HealthProfileDB.userId = healthProfile.userId;

                        if (HealthProfileDB.DieaseNames == null)
                        {
                            HealthProfileDB.DieaseNames = new List<DieaseName>();
                        }
                        else
                        {
                            HealthProfileDB.DieaseNames.Clear();
                        }

                        for (int i = 0; i < healthProfile.dieaseNamesArray.Count; i++)
                        {
                            HealthProfileDB.DieaseNames.Add(new DieaseName() { Name = healthProfile.dieaseNamesArray[i] });
                        }

                    }

                }
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }
            return response;
        }

        internal HealthProfileModel RetrieveHealthProfile(string userId)
        {
            HealthProfileModel healthProfile = new HealthProfileModel();

            try
            {
               HealthProfile healthProfileDb = checkUpContext.HealthProfiles.Where(w => w.userId == userId).FirstOrDefault();
               healthProfile.Id = healthProfileDb.Id;
               healthProfile.dieaseNamesArray = healthProfileDb.DieaseNames.Select(s => s.Name).ToList();
               healthProfile.isSTakeantiBiotic = healthProfileDb.isSTakeantiBiotic;
               healthProfile.isSuffreDiabetes = healthProfileDb.isSuffreDiabetes;
               healthProfile.isSuffrePressure = healthProfileDb.isSuffrePressure;
               healthProfile.isTakehaemophilia = healthProfileDb.isTakehaemophilia;
               healthProfile.userId = userId;
            }
            catch (Exception ex)
            {
                healthProfile = null;
            }
            return healthProfile;
        }

        internal Response addLab(Laboratory laboratory)
        {
            Response response = Response.Success;

            try
            {
                checkUpContext.Laboratories.Add(laboratory);
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }
            return response;
        }

        internal Response addLabBranch(LabBranchDB labBranch)
        {
            Response response = Response.Success;

            try
            {
                LabBranch labBranchDb = new LabBranch();
                labBranchDb.email = labBranch.email;
                labBranchDb.password = labBranch.password;
                labBranchDb.govern = labBranch.govern;
                labBranchDb.image = labBranch.image;
                labBranchDb.phone = labBranch.phone;
                labBranchDb.isAvailableFromHome = labBranch.isAvailableFromHome;
                labBranchDb.timeFrom = labBranch.timeFrom;
                labBranchDb.timeTo = labBranch.timeTo;
                labBranchDb.holidays = labBranch.holidays;
                labBranchDb.longitude = labBranch.longitude;
                labBranchDb.latitude = labBranch.latitude;
                labBranchDb.FireBaseId = labBranch.FireBaseId;
                labBranchDb.LabId = labBranch.LabId;
                labBranchDb.rating = labBranch.rating;
                labBranchDb.reviewId = labBranch.reviewId;
                labBranchDb.governId = labBranch.governId;
                labBranchDb.Address = labBranch.address;
                checkUpContext.LabBranches.Add(labBranchDb);
                checkUpContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response = Response.Fail;
            }
            return response;
        }
    }


}