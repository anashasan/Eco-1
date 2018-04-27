using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IStationService _stationService;
        private readonly IActivityService _activityService;


        public CompanyController(ICompanyService companyService,
                                 IStationService stationService,
                                 IActivityService activityService)
        {
            _companyService = companyService;
            _stationService = stationService;
            _activityService = activityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return null;
                await _companyService.AddCompany(requestDto);
                return RedirectToAction("index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult CompanyWizard()
        {
            return View("CompanyWizard");
        }

        public ActionResult ViewCompany()
        {
            return View("CompanyView");
        }

        public ActionResult Station()
        {
             var stations = _stationService.GetAllStation();
            return View("Station",stations);
        }

        [HttpPost("Company/AddStation")]
        public async Task<IActionResult> AddStation([FromBody]StationDto requestDto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Station");
            var station =await _stationService.AddStation(requestDto);
            return  RedirectToAction("Station");
        }
        
        public async Task<IActionResult> DeleteStation(int id)
        {
            await _stationService.DeleteStation(id);
            return RedirectToAction("Station");
        }

        [HttpGet("Company/GetStationById/id/{id}")]
        public IActionResult GetStationById([FromRoute]int id)
        {
            var station = _stationService.GetStationById(id);
            return Json(station);
        }

        [HttpPost("Company/AddActivity")]
        public async Task<IActionResult> AddActivity([FromBody] StationActivityDto requestDto)
        {
            if(ModelState.IsValid)
                return RedirectToAction("Station");

            await _activityService.AddActivity(requestDto);
            return RedirectToAction("Station");
        }

    }
}