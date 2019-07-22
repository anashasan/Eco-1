﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using System.Drawing;
using System.Globalization;
using Host.Business.DbServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;

namespace Host.Controllers
{
    [Authorize(Roles = "Admin,Operation,Client")]
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
        private readonly IGraphService _graphService;
        private readonly IEmployeesService _employeeService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IJsonDataService _jsonDataService;
        private readonly IClientCompanyService _clientCompanyService;
        private readonly IEmployeeProfileService _employeeProfileService;
        private readonly DownloadObservationPdf _downloadObservationPdf;

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
                                 IActivityPerformService activityPerformService,
                                 IGraphService graphService,
                                 IEmployeesService employeesService,
                                 IHostingEnvironment hostingEnvironment,
                                 IJsonDataService jsonDataService,
                                 IClientCompanyService clientCompanyService,
                                 IEmployeeProfileService employeeProfileService,
                                 DownloadObservationPdf downloadObservationPdf
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
            _graphService = graphService;
            _employeeService = employeesService;
            _hostingEnvironment = hostingEnvironment;
            _jsonDataService = jsonDataService;
            _clientCompanyService = clientCompanyService;
            _employeeProfileService = employeeProfileService;
            _downloadObservationPdf = downloadObservationPdf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Graph()
        {
            var report = await _activityPerformService.ActivityFilterReport();
            return View("ExampleGraph", report);
        }

        public IActionResult MonthlyGraph()
        {
            return View("Graph");
        }

        public IActionResult TotalActivityGraph()
        {

            return View("TotalActivityGraph");
        }

        [HttpGet("Company/TotalCountActivity")]
        public IActionResult TotalCountActivity()
        {
            var models = _graphService.GetTotalCountActivity();
            return Json(models);
        }
        [HttpGet]
        public IActionResult ActivityPerfromReport(int branchId)
        {
            ViewBag.Role = Getuserrole();
            return View("ActivityPerformReport");
        }

        [AllowAnonymous]
        [EnableCors("eco-report-grid")]
        [HttpGet("Company/data")]
        public async Task<IActionResult> ActivityPerformDailyReport(
            [FromQuery]int? locationId,
            [FromQuery]string fromDate,
            [FromQuery]string toDate,
            [FromQuery]int branchId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var cultureInfo = new CultureInfo("ur-PK");
            var isValidStartDate = DateTime.TryParse(fromDate, cultureInfo,
                DateTimeStyles.NoCurrentDateDefault, out var startDate);

            var isValidEndDate = DateTime.TryParse(toDate, cultureInfo,
                DateTimeStyles.NoCurrentDateDefault, out var endDate);

            var model = await _activityPerformService.ActivityReport(
                locationId,
                isValidStartDate ? startDate : (DateTime?)null,
                isValidEndDate ? endDate : (DateTime?)null,
                branchId);
            return Json(model);
        }

        [AllowAnonymous]
        [EnableCors("eco-report-grid")]
        [HttpGet("Company/locations/branchId/{branchId}")]
        public async Task<IActionResult> LocationByBranchId([FromRoute] int branchId)
        {
            try
            {
                var locations = await _stationLocationService.GetLocationByBranchIdAsync(branchId);
                return Json(locations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
        /// 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CompanyCreation(int? companyId)
        {
            var allCompany = await _companyService.GetAllCompany(companyId);
            if (allCompany == null)
            {
                var model = new CompanyDto();
                return View("CompanyCreation", model);
            }
            ViewBag.Roles = Getuserrole();
            return View("CompanyCreation", allCompany);
        }

        [Authorize(Roles = "Client")]

        public async Task<IActionResult> ClientCompanyList(CompanyDto companyDto)
        {
            var allCompany = new List<CompanyDto>();
            allCompany.Add(companyDto);
            if (allCompany == null)
            {
                var model = new CompanyDto();
                return View("CompanyCreation", model);
            }
            ViewBag.Roles = Getuserrole();
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
        /// 

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
        public async Task<IActionResult> AddBranch(int companyId, int branchId, string companyname)

        {
            if (branchId != 0)
            {
                var companiesList = await _companyService.GetAllCompany();
                ViewBag.CompanyId = companyId;
                var model = _branchService.GetBranchById(branchId);
                // model.Companies = new SelectList(companiesList, "CompanyId", "Name", model.CompanyId);
                return View("AddBranch", model);
            }

            var company = await _companyService.GetAllCompany();

            var branches = new BranchDto
            {
                CompanyName = companyname,
                // Companies = new SelectList(company, "CompanyId", "Name"),
                CompanyId = companyId
            };


            return View("AddBranch", branches);


        }



        [HttpGet]
        public IActionResult GetBranchByCompanyId(int companyId)
        {
            var branch = _branchService.GetBranchByCompanyId(companyId);

            ViewBag.CompanyId = companyId;
            ViewBag.Role = Getuserrole();
            return View("BranchCreation");

        }
        [HttpGet("Company/GetAllBranchesByCompanyId/data")]
        public IActionResult GetAllBranchesByCompanyId(int companyId)
        {
            var branch = _branchService.GetBranchByCompanyId(companyId);
            return Json(new { data = branch });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

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
        public async Task<IActionResult> DeleteBrancEmployee([FromRoute]int id, [FromRoute]int branchId)
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
        public async Task<IActionResult> AddBranchEmployee(int branchId, int branchemployeeId, int companyId)
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
                ViewBag.CompanyId = companyId;
                return View("AddBranchEmployee", model);
            }

            var company = await _companyService.GetAllCompany();
            var branchEmployeeModel = new BranchEmployeeDto
            {
                Companies = new SelectList(company, "CompanyId", "Name"),
                BranchId = branchId

            };
            ViewBag.BranchId = branchId;
            ViewBag.CompanyId = companyId;
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
                if (requestDto.BranchEmployeeId != null && requestDto.BranchEmployeeId != 0)
                {
                    _branchEmployeeService.UpdateBranchEmployee(requestDto);
                    return RedirectToAction("GetBranchEmployeeByBranchId", new { branchId = requestDto.BranchId, companyId = requestDto.CompanyId });
                }
                else
                {
                    _branchEmployeeService.AddBranchEmployee(requestDto);
                    return RedirectToAction("GetBranchEmployeeByBranchId", new { branchId = requestDto.BranchId, companyId = requestDto.CompanyId });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetBranchEmployeeByBranchId(int branchId, int companyId)
        {
            var branchemployee = _branchEmployeeService.GetBranchEmployeeByBranchId(branchId);
            ViewBag.BranchId = branchId;
            ViewBag.CompanyId = companyId;
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
        [HttpGet("company/location/data")]
        public IActionResult LocationCreation()
        {
            var location = _locationService.GetAllLocation();
            return Json(new { data = location });
        }

        public IActionResult AddLocation(int id, int companyId, int locationId)
        {
            if (locationId != 0)
            {
                var model = _locationService.GetLocationById(locationId);
                model.BranchId = id;
                model.CompanyId = companyId;
                return View("AddLocation", model);
            }

            var location = new LocationDto
            {
                BranchId = id,

            };
            ViewBag.CompanyId = companyId;


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
                    return RedirectToAction("GetLocationById", new { id = requestDto.BranchId, companyId = requestDto.CompanyId });
                }

                await _locationService.AddLocation(requestDto);
                return RedirectToAction("GetLocationById", new { id = requestDto.BranchId, companyId = requestDto.CompanyId });



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpGet]
        public IActionResult GetLocationById(int id, int companyId)
        {
            //  var location = _locationService.GetLocationByBranchId(id);
            ViewBag.BranchId = id;
            ViewBag.CompanyId = companyId;
            return View("LocationCreation");
        }
        [HttpGet("Company/GetAllLocationById/data")]
        public IActionResult GetAllLocationById(int id, int companyId)
        {
            var location = _locationService.GetLocationByBranchId(id);
            return Json(new { data = location });
        }

        [HttpGet]
        public IActionResult StationLocation(int locationId, int companyId, int id)
        {
            var stationLocation = _stationLocationService.GetStationLocationByLocationId(locationId);
            if (!stationLocation.Any())
            {
                var stations = new List<StationLocationDto>();
                ViewBag.LocationId = locationId;
                ViewBag.CompanyId = companyId;
                ViewBag.BranchId = id;
                return View("StationLocation", stations);
            }
            ViewBag.LocationId = locationId;
            ViewBag.CompanyId = companyId;
            ViewBag.BranchId = id;


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
                    return RedirectToAction("StationLocation", new { locationId = requestDto.LocationId, companyId = requestDto.CompanyId, id = requestDto.BranchId });
                }

                _stationLocationService.AddStationLocation(requestDto);
                return RedirectToAction("StationLocation", new { locationId = requestDto.LocationId, companyId = requestDto.CompanyId, id = requestDto.BranchId });


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public IActionResult AddStationLocation(int locationId, int stationlocationId, int companyId, int id)
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
                model.Stations = new SelectList(stationlist, "StationId", "Name", model.StationId);
                model.LocationId = stationlocationId;
                ViewBag.LocationId = locationId;
                ViewBag.BranchId = id;
                ViewBag.CompanyId = companyId;
                return View("AddStationLocation", model);
            }

            //return View("AddStationLocation", stationloction);




            var station = _stationService.GetAllStation();
            var stationlocation = new StationLocationDto
            {
                Stations = new SelectList(station, "StationId", "Name"),
                LocationId = locationId,
                BranchId = id,
                CompanyId = companyId
            };



            return View("AddStationLocation", stationlocation);
        }

        public IActionResult UpdateBranchStation(int stationlocationId, int branchId, int locationId, int companyId)
        {
            var stationList = _stationService.GetAllStation();
            var locationList = _locationService.GetLocationByBranchId(branchId);

            var model = _stationLocationService.GetStationLocationById(stationlocationId);
            model.Stations = new SelectList(stationList, "StationId", "Name", model.StationId);
            model.Locations = new SelectList(locationList, "LocationId", "Name", model.LocationId);


            return View("UpdateBranchStation", model);
        }

        [HttpPost]

        public IActionResult UpdateBranchStation(StationLocationDto requestDto)
        {
            try
            {
                _stationLocationService.BranchStationLocation(requestDto);
                return RedirectToAction("GetStationByBranchId", new { branchId = requestDto.BranchId, companyId = requestDto.CompanyId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<IActionResult> AddClientCompany(Guid clientCompanId, int companyId, string userId)
        {

            var companies = new List<CompanyDto>();
            var users = new List<ClientDto>();
            var model = new ClientCompanyDropDownDto();
            if (Guid.Empty != clientCompanId)
            {
                companies = await _companyService.GetAllCompany();
                users = _employeeProfileService.GetAllClient();
                model = new ClientCompanyDropDownDto();
                model.ClientCompanyId = clientCompanId;
                model.Companies = new SelectList(companies, "CompanyId", "Name", companyId);
                model.Users = new SelectList(users, "UserId", "ClientName", userId);

                return View("AddClientCompany", model);
            }
            companies = await _companyService.GetAllCompany();
            users = _employeeProfileService.GetAllClient();
            model = new ClientCompanyDropDownDto();
            model.Companies = new SelectList(companies, "CompanyId", "Name");
            model.Users = new SelectList(users, "UserId", "ClientName");

            return View("AddClientCompany", model);


        }

        [HttpPost]
        public IActionResult AddClientCompany(ClientComanyDto dto)
        {
            if (Guid.Empty != dto.ClientCompanyId)
            {
                _clientCompanyService.UpdateClientCompany(dto);
                return RedirectToAction("ClientCompany");
            }
            else

            {
                _clientCompanyService.AddClientCompany(dto);

                return RedirectToAction("ClientCompany");
            }
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

        public IActionResult DownloadObservationReport(int branchId, int? locationId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {

                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                var bytes = _downloadObservationPdf.DownloadObservation(branchId, locationId, fromDate, toDate, _hostingEnvironment);
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";
                var response = File(bytes, "application/pdf", $"{"abc"}.pdf");
                Response.ContentType = "application/pdf";
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult Download(int id, int locationId, string code, int sno)
        {
            try
            {

                var stationName = _stationLocationService.GetStationNameById(id);
                var stationLocation = _locationService.GetLocationById(locationId);
                ViewBag.LocationId = locationId;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                var bytes = DownloadPdf.Download(id, stationName, stationLocation.Name, code, sno, _hostingEnvironment);
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                var response = File(bytes, "application/pdf", $"{stationName + stationLocation.Name}.pdf");
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + stationName + stationLocation.Name + ".pdf");
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
        public IActionResult DownloadAllPdf([FromBody]List<DownloadPdfDto> downloadPdf)
        {
            try
            {
                var list = new List<FileContentResult>();
                var stationLocation = _locationService.GetLocationById(downloadPdf.Select(i => i.LocationId).FirstOrDefault());

                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                var bytes = DownloadPdf.DownloadAllPdf(downloadPdf, stationLocation.Name, _hostingEnvironment);
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                var response = File(bytes, "application/pdf", $"{downloadPdf.Select(i => i.StationName).FirstOrDefault() + stationLocation.Name}.pdf");
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + downloadPdf.Select(i => i.StationName).FirstOrDefault() + stationLocation.Name + ".pdf");
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


        public IActionResult Station()
        {
            return View("StationCreation");
        }

        [HttpGet("company/station/data")]
        public IActionResult GetAllStation()
        {
            var station = _stationService.GetAllStation();
            return Json(new { data = station });
        }

        [HttpGet("company/company/data")]
        public async Task<IActionResult> GetallCompanyAsync(int? companyId)
        {
            var company = await _companyService.GetAllCompany(companyId);
            return Json(new { data = company });
        }

        public IActionResult GetAllBranches()
        {
            var branch = _branchService.GetAllBranch();
            return Json(new { data = branch });
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
                var activities = await _stationLocationService.GetStationActivityByCode(code);
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

        [AllowAnonymous]
        [HttpPost("Company/ActivityPerform")]
        public async Task<IActionResult> ActivityPerform([FromBody] ActivityPerformDto requestDto)
        {
            if (!ModelState.IsValid)
                return Json("data is not valid");
            var id = await _activityPerformService.ActivityPerform(requestDto);
            return Json(id);
        }

        //[AllowAnonymous]
        //[HttpPost("Company/ActivityPerform1")]
        //public async Task<IActionResult> ActivityPerform1([FromBody] ActivityPerformDto requestDto1)
        //{
        //    if (!ModelState.IsValid)
        //        return Json("data is not valid");
        //    var id = await _activityPerformService.ActivityPerform1(requestDto1);
        //    return Json(id);
        //}


        public IActionResult GetStationByBranchId(int branchId, int companyId)
        {
            var stationbranch = _stationLocationService.GetStationLocationByBranchId(branchId);
            ViewBag.CompanyId = companyId;
            ViewBag.BranchId = branchId;
            return View("StationBranch", stationbranch);

        }
        [HttpGet("Company/GetAllStationByBranchId/data")]
        public IActionResult GetAllStationByBranchId(int branchId, int companyId)
        {
            var stationbranch = _stationLocationService.GetStationLocationByBranchId(branchId);
            return Json(new { data = stationbranch });
        }

        [HttpGet("Company/GetStations/data")]
        public IActionResult GetStationofBranch(int branchId)
        {
            var stationbranch = _stationLocationService.GetStationLocationByBranchId(branchId);
            return Json(new { data = stationbranch });
        }




        [HttpGet("Company/DailyReport")]
        public IActionResult DailyReport([FromQuery]DateTime to, [FromQuery]DateTime from)
        {
            var report = _activityPerformService.ActivityFilterReport();
            return Json(report);
        }

        [HttpGet]
        public IActionResult DeleteCompany(int companyId)
        {
            try
            {
                _companyService.DeleteCompany(companyId);
                return RedirectToAction("CompanyCreation");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult DeleteBranch(int branchId, int companyId)
        {
            try
            {
                _branchService.DeleteBranch(branchId);
                return RedirectToAction("GetBranchByCompanyId", new { companyId = companyId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult DeleteLocation(int locationId, int branchId, int companyId)
        {
            try
            {
                _locationService.DeleteLocation(locationId);

                return RedirectToAction("GetLocationById", new { id = branchId, CompanyId = companyId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult DeleteStationLocation(int stationLocationId, int branchId, int companyId)
        {
            try
            {
                _stationLocationService.DeleteStationLocation(stationLocationId);

                return RedirectToAction("GetStationByBranchId", new { branchId = branchId, companyId = companyId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult DeleteStation(int stationId)
        {
            try
            {
                _stationService.DeleteStation(stationId);
                return RedirectToAction("Station");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult DeleteStationActivity(int activityId, int stationid)
        {
            try
            {
                _activityService.DeleteActivityById(activityId);
                return RedirectToAction("GetActivityById", new { stationId = stationid });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("Company/Report/BranchId/{branchId}/locationId/{locationId}")]
        public IActionResult GetActivityPerformReport([FromRoute]int branchId, [FromRoute] int locationId)
        {
            try
            {
                var model = _activityPerformService.ActivityFilterReporByBranchIdt(branchId, locationId);
                return Json(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpGet("Company/StationLocation/CheckSno/sno/{sno}/branchId/{branchId}")]
        public bool CheckSnoExist([FromRoute] int sno, [FromRoute]int branchId)
        {
            var snoExist = _stationLocationService.CheckSnoExist(sno, branchId);
            return snoExist;
        }


        [HttpGet("Company/Employee/CheckEmail/email/{emai}")]
        public bool CheckEmailAddress([FromRoute]string email)
        {
            return _employeeService.CheckEmailIsExist(email);
        }

        [AllowAnonymous]
        [HttpGet("Company/GetJson")]
        public IActionResult GetJson([FromQuery] Guid code)
        {

            var json = _jsonDataService.GetJsonData(code, out string message);
            if (string.IsNullOrEmpty(message))
            {
                return Json(json);
            }
            else
            {
                return Json(message);
            }

        }

        public IActionResult Test()
        {
            return View("Test");
        }


        public IActionResult ClientCompany()
        {
            try
            {
                var model = _clientCompanyService.GetAllClientCompany();
                if (model != null)
                {
                    return View("ClientCompany", model);
                }

                var models = new List<ClientComanyDto>();
                return View("ClientCompany", models);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet("Company/ObservationReport")]
        public IActionResult ObservationReport([FromQuery]int branchId, int? locationId, [FromQuery]DateTime? fromDate, [FromQuery]DateTime? toDate)
        {
            try
            {
                var observationReport = _activityService.GetObservationReport(branchId, locationId, fromDate, toDate);
                return Json(observationReport);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        //[HttpGet("Company/ObservationReportForm/branchId/{branchId}/companyname/{companyname}/branchname{branchname}")]
        public IActionResult ObservationReportForm(int branchId, string companyname, string branchname, DateTime? fromdate, DateTime? todate)
        {
            try
            {
                ViewBag.CompanyName = companyname;
                ViewBag.BranchName = branchname;
                ViewBag.BranchId = branchId;
                int? locationId = null;
                DateTime? fromDate = fromdate;
                DateTime? toDate = todate;
                ViewBag.Role = Getuserrole();
                var observationReport = _activityService.GetObservationReport(branchId, locationId, fromDate, toDate);
                return View("ObservationReport", observationReport);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[HttpPost("Company/UpdateObservation")]
        //public IActionResult UpdateObservationForm([FromBody]EditObseravationDto dto)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(dto);
        //        }

        //        _activityService.UpdateActivityObservation(dto);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}


        [AllowAnonymous]
        [HttpGet("Company/GraphReport")]
        public IActionResult GetGraphReport(
               [FromQuery]int? locationId,
            [FromQuery]string fromDate,
            [FromQuery]string toDate,
            [FromQuery]int branchId)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var cultureInfo = new CultureInfo("ur-PK");
            var isValidStartDate = DateTime.TryParse(fromDate, cultureInfo,
                DateTimeStyles.NoCurrentDateDefault, out var startDate);

            var isValidEndDate = DateTime.TryParse(toDate, cultureInfo,
                DateTimeStyles.NoCurrentDateDefault, out var endDate);
            return Json(_activityPerformService.StationReport(
                 locationId,
                isValidStartDate ? startDate : (DateTime?)null,
                isValidEndDate ? endDate : (DateTime?)null,
                branchId));
        }
        //public IActionResult DailyGraph(int branchid)
        //{
        //    return View("TestGraph");
        //}

        public IActionResult DailyGraph(int branchid)
        {
            ViewBag.Role = Getuserrole();
            return View("ReportGraph");
        }

        //[AllowAnonymous]
        //[EnableCors("eco-report-grid")]
        //[HttpGet("Company/GetData")]
        //public IActionResult GetData( [FromQuery]int? locationId,
        //    [FromQuery]string fromDate,
        //    [FromQuery]string toDate,
        //    [FromQuery]int branchId,
        //    [FromQuery] int SNo
        //    )
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var cultureInfo = new CultureInfo("ur-PK");
        //    var isValidStartDate = DateTime.TryParse(fromDate, cultureInfo,
        //        DateTimeStyles.NoCurrentDateDefault, out var startDate);

        //    var isValidEndDate = DateTime.TryParse(toDate, cultureInfo,
        //        DateTimeStyles.NoCurrentDateDefault, out var endDate);
        //    return Json(_activityPerformService.GetDailyReportByBranchId(locationId,
        //         isValidStartDate ? startDate : (DateTime?)null,
        //         isValidEndDate ? endDate : (DateTime?)null,
        //         branchId, SNo));
        //}

        //[AllowAnonymous]
        //[EnableCors("eco-report-grid")]
        //[HttpPost("Company/UpdateData")]
        //public IActionResult UpdateData([FromBody]GetDailyReportDto requestDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest((ModelState));
        //    _activityPerformService.UpdateDailyReport(requestDto);
        //    return Ok();
        //}

        [HttpGet]
        public IActionResult GetBranchObservationById(int id, int branchId)
        {

            var observation = _activityService.GetActivityObservationById(id);
            observation.BranchId = branchId;
            ViewBag.Role = Getuserrole();
            return View("EditObservationReport", observation);


        }

        // [HttpPut("Company/UpdateActiviyObservation")]
        public IActionResult UpdateActiviyObservation(EditObseravationDto editObseravationDto)
        {
            try
            {
                _activityService.UpdateActivityObservation(editObseravationDto);

                return RedirectToAction("ObservationReportForm", new { branchId = editObseravationDto.BranchId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IActionResult DeleteObservationReport(int id, int branchid)
        {
            try
            {
                _activityService.DeleteObservationById(id);
                return RedirectToAction("ObservationReportForm", new { branchId = branchid });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult GetEditDailyReportByBranchId(int branchid, int companyid)
        {

            //DateTime? fromDate = null;
            //DateTime? toDate = null;
            var model = _activityPerformService.GetDailyReportByBranchId(branchid);
            ViewBag.CompanyId = companyid;
            ViewBag.BranchId = branchid;
            return View("DailyReportData", model);
        }


        public IActionResult GetDailyEditDailyReportById(int id, int branchId, int companyId)
        {

            var activity = _activityPerformService.GetEditActivityReportById(id);
            activity.BranchId = branchId;
            activity.CompanyId = companyId;
            activity.PkActivityPerformDetailId = id;
            return View("GetEditActivityReport", activity);


        }

        public IActionResult UpdateActiviyReport(EditDailyReportDto dailyReportDto)
        {
            try
            {
                _activityPerformService.UpdateDailyReport(dailyReportDto);

                return RedirectToAction("GetEditDailyReportByBranchId", new { branchId = dailyReportDto.BranchId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}