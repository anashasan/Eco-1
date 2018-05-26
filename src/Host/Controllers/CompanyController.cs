using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.DataModel;
using Host.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private readonly QRCodeGenerator _qRCodeGenerator;
        private readonly IStationLocationService _stationLocationService;
       


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
                                 QRCodeGenerator qRCodeGenerator,
                                 IStationLocationService stationLocationService
                                 )
        {
            _companyService = companyService;
            _stationService = stationService;
            _activityService = activityService;
            _branchService = branchService;
            _branchEmployeeService = branchEmployeeService;
            _locationService =locationService;
            _qRCodeGenerator = qRCodeGenerator;
            _stationLocationService = stationLocationService;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
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
        public ActionResult Station()
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
        [HttpPost("Company/AddActivity")]
        public async Task<IActionResult> AddActivity([FromBody] StationActivityDto requestDto)
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
        }

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
        public  IActionResult AddCompany(CompanyDto requestDto)
        {
            try
            {
                var userId = GetUserid().ToString();
                requestDto.UserId = userId;
                if (!ModelState.IsValid)
                    return RedirectToAction("CompanyCreation");
                if(requestDto.CompanyId.HasValue && requestDto.CompanyId != 0)
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
            var allCompany =await _companyService.GetAllCompany();
            if(allCompany == null)
            {
                var model = new CompanyDto();
                return View("CompanyCreation", model);
            }

            return View("CompanyCreation",allCompany);
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
            if(branchModel == null)
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
            var companiesList =await _companyService.GetAllCompany();
            var brancModel = new BranchDto
            {
                Companies = new SelectList(companiesList,"CompanyId", "Name")
            };

            return View("AddBranch",brancModel);
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

        #region BranchEmployee

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult BranchEmployee()
        {
            var model = _branchEmployeeService.GetAllBranchEmployee();
            if(model == null)
            {
                var branchEmployeeModel = new BranchEmployeeDto();
                return View("BranchEmployee", branchEmployeeModel);
            }
            return View("BranchEmployee", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddBranchEmployee()
        {
            var companiesList = await _companyService.GetAllCompany();
            var branchEmployeeModel = new BranchEmployeeDto
            {
                Companies = new SelectList(companiesList, "CompanyId", "Name")
            };
            return View("AddBranchEmployee", branchEmployeeModel);
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Company/GetBranchByCompanyId/Id/{id}")]
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult AddBrancEmployee(BranchEmployeeDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction("BranchEmployee");
                _branchEmployeeService.AddBranchEmployee(requestDto);
                return RedirectToAction("BranchEmployee");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetBranchEmployeeById(int id)
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
        }

        #endregion

        public IActionResult QrCode()
        {
            var qrCode = _qRCodeGenerator.QrCode();
            return View("QRCode", qrCode);
        }

        public IActionResult Activity()
        {
            return View("Activity");
        }

        public IActionResult LocationCreation()
        {
            var location = _locationService.GetAllLocation();
            return View("LocationCreation",location);
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
                    return RedirectToAction("LocationCreation");

                if (requestDto.LocationId != 0)
                {
                    await _locationService.UpdateLocation(requestDto);
                    return RedirectToAction("GetLocationById",new { id = requestDto.BranchId } );
                }

                await _locationService.AddLocation(requestDto);
                return RedirectToAction("GetLocationById",new { id = requestDto.BranchId });


                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                    throw;
            }

        }
        [HttpGet]
        public  IActionResult GetLocationById(int id)
        {
            var location = _locationService.GetLocationByBranchId(id);
            ViewBag.BranchId = id;
            return View("LocationCreation", location);
        }

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
        public  async Task <IActionResult> AddStationLocation(StationLocationDto requestDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return RedirectToAction("StationLocation");
                }
                await _stationLocationService.AddStationLocation(requestDto);
                return RedirectToAction("StationLocation", new { locationId = requestDto.LocationId});
           

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        public  IActionResult AddStationLocation(int locationId, int stationId)
        {
            var stationlist =  _stationService.GetAllStation();
            var stationloction = new StationLocationDto
            {
                Stations = new SelectList(stationlist, "StationId", "Name"),
                LocationId = locationId
            };
            ViewBag.LocationId = locationId;
            return View("AddStationLocation",stationloction);

                

        }

      

        
    }
}