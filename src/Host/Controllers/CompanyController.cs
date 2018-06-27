using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemBox.Document;
using Host.Business.IDbServices;
using Host.DataModel;
using Host.Helper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using iTextSharp.text.factories;
using QRCoder;
using System.Drawing;

namespace Host.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICompanyService _companyService;
        private readonly IStationService _stationService;
        private readonly IActivityService _activityService;
        private readonly IBranchService _branchService;
        private readonly IBranchEmployeeService _branchEmployeeService;
        private readonly ILocationService _locationService;
       // private readonly QRCodeGenerator _qRCodeGenerator;
        private readonly IStationLocationService _stationLocationService;
        private readonly IActivityTypeService _activityTypeService;
        private readonly IActivityPerformService _activityPerformService;
        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyService"></param>
        /// <param name="stationService"></param>
        /// <param name="activityService"></param>
        /// <param name="branchService"></param>
        /// <param name="branchEmployeeService"></param>
        public CompanyController(ICompanyService companyService,
                                 IStationService stationService,
                                 IActivityService activityService,
                                 IBranchService branchService,
                                 IBranchEmployeeService branchEmployeeService,
                                 ILocationService locationService,
                                // QRCodeGenerator qRCodeGenerator,
                                 IStationLocationService stationLocationService,
                                 IActivityTypeService activityTypeService,
                                 IActivityPerformService activityPerformService
                                 )
        {
            _companyService = companyService;
            _stationService = stationService;
            _activityService = activityService;
            _branchService = branchService;
            _branchEmployeeService = branchEmployeeService;
            _locationService = locationService;
           // _qRCodeGenerator = qRCodeGenerator;
            _stationLocationService = stationLocationService;
            _activityTypeService = activityTypeService;
            _activityPerformService = activityPerformService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Graph()
        {
            return View("ExampleGraph");
        }

        //[HttpPost]
        //public async Task<IActionResult> AddCompany(CompanyDto requestDto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return null;
        //        await _companyService.AddCompany(requestDto);
        //        return RedirectToAction("index", "Home");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyWizard()
        {
            return View("CompanyWizard");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCompany()
        {
            var companies = _companyService.GetCompanyList();
            return View("CompanyView", companies);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /*    public ActionResult Station()
        {
            var stations = _stationService.GetAllStation();
            return View("StationCreation", stations);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost("Company/AddStation")]
        public async Task<IActionResult> AddStation([FromBody]StationDto requestDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Station");
            var station = await _stationService.AddStation(requestDto);
            return RedirectToAction("Station");
        }

        public IActionResult AddStation()
        {
            return View("AddStation");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteStation(int id)
        {
            await _stationService.DeleteStation(id);
            return RedirectToAction("Station");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Company/GetStationById/id/{id}")]
        public IActionResult GetStationById([FromRoute]int id)
        {
            var station = _stationService.GetStationById(id);
            return Json(station);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
     //   [HttpPost("Company/AddActivity")]
      /*  public async Task<IActionResult> AddActivity([FromBody] StationActivityDto requestDto)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Station");
            if(requestDto.Activities.Any())
            {
               await _activityService.UpdateActivity(requestDto);
                return RedirectToAction("Station");
            }
            await _activityService.AddActivity(requestDto);
            return RedirectToAction("Station");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Company/GetActivityByStationId/id/{id}")]
        public IActionResult GetActivityByStationId([FromRoute] int id)
        {
            try
            {
                var activities = _activityService.GetActivityByStationId(id);
                return Json(activities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AllCompanyView()
        {
            return View("AllCompanyView");
        }

        #region Company

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCompany(CompanyDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(requestDto);
                var userId = GetUserid().ToString();
                requestDto.UserId = userId;
                if (!ModelState.IsValid)
                    return RedirectToAction("CompanyCreation");
                if (requestDto.CompanyId.HasValue && requestDto.CompanyId != 0)
                {
                    _companyService.UpdateCompany(requestDto);
                    return RedirectToAction("CompanyCreation");
                }
                var company = _companyService.AddCompany(requestDto);
                return RedirectToAction("CompanyCreation");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCompanyById(int id)
        {
            try
            {
                var companies = _companyService.GetCompanyById(id);
                return View("AddCompany", companies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CompanyCreation()
        {
            var allCompany = await _companyService.GetAllCompany();
            if (allCompany == null)
            {
                var model = new CompanyDto();
                return View("CompanyCreation", model);
            }

            return View("CompanyCreation", allCompany);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AddCompany()
        {
            return View("AddCompany");
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Branch()
        {
            var branchModel = _branchService.GetAllBranch();
            if (branchModel == null)
            {
                var model = new BranchDto();
                return View("BranchCreation", model);
            }
            return View("BranchCreation", branchModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBranch(BranchDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(requestDto);
                if (requestDto.BranchId != 0)
                {
                    await _branchService.UpdateBranch(requestDto);
                    return RedirectToAction("GetBranchByCompanyId", new { companyId = requestDto.CompanyId });
                }
                await _branchService.AddBranch(requestDto);
                return RedirectToAction("GetBranchByCompanyId", new { companyId = requestDto.CompanyId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddBranch(int companyId, int branchId)

        {
            if (branchId != 0)
            {
                var companiesList = await _companyService.GetAllCompany();
                ViewBag.CompanyId = companyId;
                var model = _branchService.GetBranchById(branchId);
                model.Companies = new SelectList(companiesList, "CompanyId", "Name", model.CompanyId);
                return View("AddBranch", model);
            }

            var company = await _companyService.GetAllCompany();
            var branches = new BranchDto
            {
                Companies = new SelectList(company, "CompanyId", "Name"),
                CompanyId = companyId
            };


            return View("AddBranch", branches);
            

        }

        [HttpGet]
        public IActionResult GetBranchByCompanyId(int companyId)
        {
            var branch = _branchService.GetBranchByCompanyId(companyId);
            ViewBag.CompanyId = companyId;
            return View("BranchCreation", branch);

        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        /*   public async Task<IActionResult> GetBranchById(int id)
           {
               try
               {
                   var companiesList = await _companyService.GetAllCompany();
                   var branch = _branchService.GetBranchById(id);
                   var branchModel = new BranchDto
                   {
                       Companies = new SelectList(companiesList, "CompanyId", "Name")
                   };
                   branch.Companies = branchModel.Companies;
                   return View("AddBranch", branch);
               }
               catch (Exception e)
               {
                   Console.WriteLine(e);
                   throw;
               }

           }*/

        /*  public IActionResult Branch()
          {
              var branchModel = _branchService.GetAllBranch();
              if (branchModel == null)
              {
                  var model = new BranchDto();
                  return View("BranchCreation", model);
              }
              return View("BranchCreation", branchModel);
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="requestDto"></param>
          /// <returns></returns>
          [HttpPost]
          public async Task<IActionResult> AddBranch(BranchDto requestDto)
          {
              try
              {
                  if (!ModelState.IsValid)
                      return RedirectToAction("Branch");
                  if (requestDto.BranchId != 0)
                  {
                      await _branchService.UpdateBranch(requestDto);
                      return RedirectToAction("Branch");
                  }
                  await _branchService.AddBranch(requestDto);
                  return RedirectToAction("Branch");
              }
              catch (Exception e)
              {
                  Console.WriteLine(e);
                  throw;
              }
          }

          /// <summary>
          /// 
          /// </summary>
          /// <returns></returns>
          public async Task<IActionResult> AddBranch()
          {
              var companiesList = await _companyService.GetAllCompany();
              var brancModel = new BranchDto
              {
                  Companies = new SelectList(companiesList, "CompanyId", "Name")
              };

              return View("AddBranch", brancModel);
          }



          /// <summary>
          /// 
          /// </summary>
          /// <param name="id"></param>
          /// <returns></returns>
          [HttpGet]
          public async Task<IActionResult> GetBranchById(int id)
          {
              try
              {
                  var companiesList = await _companyService.GetAllCompany();
                  var branch = _branchService.GetBranchById(id);
                  var branchModel = new BranchDto
                  {
                      Companies = new SelectList(companiesList, "CompanyId", "Name")
                  };
                  branch.Companies = branchModel.Companies;
                  return View("AddBranch", branch);
              }
              catch (Exception e)
              {
                  Console.WriteLine(e);
                  throw;
              }
          }
          */

        #region BranchEmployee

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult BranchEmployee()
        {
            var model = _branchEmployeeService.GetAllBranchEmployee();
            if (model == null)
            {
                var branchEmployeeModel = new BranchEmployeeDto();
                return View("BranchEmployee", branchEmployeeModel);
            }
            return View("BranchEmployee", model);
        }

        [HttpDelete("Company/DeleteBranchEmployee/id/{id}/branchId/{branchId}")]
        public async Task<IActionResult> DeleteBrancEmployee([FromRoute]int id,[FromRoute]int branchId)
        {
            try
            {
               await _branchEmployeeService.DeleteBranchEmployeeById(id);
                return RedirectToAction("GetBranchEmployeeByBranchId", new { branchId = branchId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddBranchEmployee(int branchId, int branchemployeeId)
        {
            if (branchemployeeId != 0)
            {
                var companiesList = await _companyService.GetAllCompany();
                var model = _branchEmployeeService.GetBranchEmployeeById(branchemployeeId);
                var branchEmployee = new BranchEmployeeDto
                {
                    Companies = new SelectList(companiesList, "CompanyId", "Name"),
                    BranchId = branchId

                };

                model.Companies = branchEmployee.Companies;
                model.BranchId = branchId;
                ViewBag.BranchId = branchId;
                return View("AddBranchEmployee", model);
            }

            var company = await _companyService.GetAllCompany();
            var branchEmployeeModel = new BranchEmployeeDto
            {
                Companies = new SelectList(company, "CompanyId", "Name"),
                BranchId = branchId

            };

            return View("AddBranchEmployee", branchEmployeeModel);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /*  [HttpGet("Company/GetBranchByCompanyId/Id/{id}")]
        public  IActionResult GetBranchByCompanyId([FromRoute] int id)
        {
            try
            {
                var branchModel = _branchService.GetBranchByCompanyId(id);
                return Json(branchModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);    
                throw;
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult AddBranchEmployee(BranchEmployeeDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(requestDto);
                if(requestDto.BranchEmployeeId !=null && requestDto.BranchEmployeeId != 0)
                {
                    _branchEmployeeService.UpdateBranchEmployee(requestDto);
                    return RedirectToAction("GetBranchEmployeeByBranchId", new { branchId = requestDto.BranchId });
                }
                else
                {
                    _branchEmployeeService.AddBranchEmployee(requestDto);
                    return RedirectToAction("GetBranchEmployeeByBranchId", new { branchId = requestDto.BranchId });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        /*  public IActionResult GetBranchEmployeeById(int id)
          {
              try
              {
                  var model = _branchEmployeeService.GetBranchEmployeeById(id);
                  return View("AddBranchEmployee", model);
              }
              catch (Exception e)
              {
                  Console.WriteLine(e);
                  throw;
              }
          }*/

        [HttpGet]
        public IActionResult GetBranchEmployeeByBranchId(int branchId)
        {
            var branchemployee = _branchEmployeeService.GetBranchEmployeeByBranchId(branchId);
            ViewBag.BranchId = branchId;
            return View("BranchEmployee", branchemployee);
        }

        [HttpGet]
        public IActionResult GetBranchEmployeeByCompanyId(int companyId)
        {
            var branchemployee = _branchEmployeeService.GetBranchEmployeeByCompanyId(companyId);
            ViewBag.CompanyId = companyId;
            return View("BranchEmployee", branchemployee);
        }
        #endregion

      /*  public IActionResult QrCode()
        {
            var qrCode = _qRCodeGenerator.QrCode();
            return View("QRCode", qrCode);
        }*/

        public IActionResult Activity()
        {
            return View("Activity");
        }

        public IActionResult LocationCreation()
        {
            var location = _locationService.GetAllLocation();
            return View("LocationCreation", location);
        }

        public IActionResult AddLocation(int id, int locationId)
        {
            if (locationId != 0)
            {
                var model = _locationService.GetLocationById(locationId);
                model.BranchId = id;
                return View("AddLocation", model);
            }

            var location = new LocationDto
            {
                BranchId = id
            };
            return View("AddLocation", location);
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(LocationDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(requestDto);

                if (requestDto.LocationId != 0)
                {
                    await _locationService.UpdateLocation(requestDto);
                    return RedirectToAction("GetLocationById", new { id = requestDto.BranchId });
                }

                await _locationService.AddLocation(requestDto);
                return RedirectToAction("GetLocationById", new { id = requestDto.BranchId });



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpGet]
        public IActionResult GetLocationById(int id)
        {
            var location = _locationService.GetLocationByBranchId(id);
            ViewBag.BranchId = id;
            return View("LocationCreation", location);
        }

        [HttpGet]
        public IActionResult StationLocation(int locationId)
        {
            var stationLocation = _stationLocationService.GetStationLocationByLocationId(locationId);
            if (!stationLocation.Any())
            {
                var stations = new List<StationLocationDto>();
                ViewBag.LocationId = locationId;
                return View("StationLocation", stations);
            }
            ViewBag.LocationId = locationId;

            return View("StationLocation", stationLocation);

        }

        [HttpPost]
        public async Task<IActionResult> AddStationLocation(StationLocationDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("StationLocation");
                }

                if (requestDto.StationLocationId != 0)
                {
                    await _stationLocationService.UpdateStationLocation(requestDto);
                    return RedirectToAction("StationLocation", new { locationId = requestDto.LocationId });
                }
                await _stationLocationService.AddStationLocation(requestDto);
                return RedirectToAction("StationLocation", new { locationId = requestDto.LocationId });


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public IActionResult AddStationLocation(int locationId, int stationlocationId)
        {
            if (stationlocationId != 0)
            {
                var stationlist = _stationService.GetAllStation();
                var model = _stationLocationService.GetStationLocationById(stationlocationId);


                //var stationloctionmodel = new StationLocationDto
                //{
                //    Stations = new SelectList(stationlist, "StationId", "Name"),
                //    LocationId = locationId
                //};
                model.Stations = new SelectList(stationlist,"StationId","Name",model.StationId);
                model.LocationId = stationlocationId;
                ViewBag.LocationId = locationId;
                return View("AddStationLocation", model);
            }

            //return View("AddStationLocation", stationloction);




            var station = _stationService.GetAllStation();
            var stationlocation = new StationLocationDto
            {
                Stations = new SelectList(station, "StationId", "Name"),
                LocationId = locationId
            };


            return View("AddStationLocation", stationlocation);
        }




        /*  public  IActionResult EditStationLocation(int locationId,int stationlocationId)
          {
              if (stationlocationId != 0)
              {
                  var stations =  _stationService.GetAllStation();
                  var model = _stationLocationService.GetStationLocationById(stationlocationId);
                  var stationlocationmodel = new StationLocationDto
                  {
                      Stations = new SelectList(stations, "StationId", "Name"),
                      LocationId = locationId
                  };
                  model.Stations = stationlocationmodel.Stations;
                  model.LocationId = stationlocationId;
                  return View("AddStationLocation", model);
              }


              var location = new StationLocationDto
              {
                  LocationId = locationId
              };
              return View("AddStationLocation", location);
          }*/

        public IActionResult ActivityCreation()
        {
            return View("ActivityCreation");
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id,int locationId)
        {
            try
            {
                var stationName = _stationLocationService.GetStationNameById(id);
                var stationLocation = _locationService.GetLocationById(locationId);
                ViewBag.LocationId = locationId;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                var bytes = DownloadPdf.Download(id, stationName, stationLocation.Name);
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                
                var response = File(bytes, "application/pdf", $"{stationName + stationLocation.Name}.pdf");
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + stationName + stationLocation.Name+ ".pdf");
                Response.ContentType = "application/pdf";
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddActivity(ActivityDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(requestDto);
                }

                if (requestDto.ActivityId != 0)
                {
                    await _activityService.UpdateActivity(requestDto);
                    return RedirectToAction("GetActivityById", new { stationId = requestDto.StationId });
                }

                await _activityService.AddActivity(requestDto);
                return RedirectToAction("GetActivityById", new { stationId = requestDto.StationId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        public IActionResult AddActivity(int stationId, int activityId)
        {
            var types = _activityTypeService.GetAllActivityType();
            if (activityId != 0)
            {
                var model = _activityService.GetActivityById(activityId);
                model.Types = new SelectList(types, "ActivityTypeId", "Type", model.ActivityTypeId);
                model.StationId = stationId;
                return View("AddActivity", model);
            }
            var activity = new ActivityDto
            {
                StationId = stationId,
                Types = new SelectList(types, "ActivityTypeId", "Type"),
            };
            return View("AddActivity", activity);

        }

        [HttpGet]
        public IActionResult GetActivityById(int stationId)
        {

            var activity = _activityService.GetActivityByStationId(stationId);

            ViewBag.StationId = stationId;
            return View("ActivityCreation", activity);
        }

        //[HttpDelete("Company/StationDelete/id/{id}")]
        //public IActionResult DeleteStationActivity(int id)
        //{
            
        //    //var activity= _activityService.DeleteActivityById(id);
        //    //return View("AddActivity",activity);
        //}

        public IActionResult Station(PagingParams pagingParams)
        {   
            var stations = _stationService.GetAllStationPagination(pagingParams);
            return View("StationCreation", stations);
        }

        [HttpPost]
        public async Task<IActionResult> AddStation(StationDto requestDto)
        {


            if (!ModelState.IsValid)
                return View(requestDto);
            if (requestDto.StationId != 0 && requestDto.StationId != null)
            {
                await _stationService.UpdateStation(requestDto);
                return RedirectToAction("Station");

            }
            await _stationService.AddStation(requestDto);
            return RedirectToAction("Station");
        }

        public IActionResult AddStation()
        {
            return View("AddStation");
        }
        [HttpGet]
        public IActionResult GetStationById(int id)
        {
            var station = _stationService.GetStationById(id);
            return View("AddStation", station);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name=" ActivityId"></param> 
        /// <param name=" Name"></param>
        ///<param name="StationId"></param>
        ///<param name="ActivityTypeId"></param> 
        ///<param name="Type"></param>
        ///<param name="StationActivityId"></param>
        ///<param name=" Observation"></param>
        ///<param name="EmployeeId"></param>
        ///<param name="IsPerform"></param>
        ///<param name="Perform"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Company/GetStationActivityByCode/Code/{code}")]
        public async Task<IActionResult> GetStationActivityByCode([FromRoute]string code)
        {
            try
            {
                var activities =await _stationLocationService.GetStationActivityByCode(code);
                if (activities.Activities != null)
                {

                    return Json(activities);
                }
                return Json("Code is invalid");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       [HttpPost("Company/ActivityPerform")]
       public async Task<IActionResult> ActivityPerform([FromBody] ActivityPerformDto requestDto)
        {
            if (!ModelState.IsValid)
                return Json(requestDto);
            var id = await _activityPerformService.ActivityPerform(requestDto);
            return  Json(id);
        }
       
    } 
}