using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.DataModel;
using Microsoft.AspNetCore.Mvc;

/*namespace Host.Controllers
{
   public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity([FromBody]StationActivityDto requestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return RedirectToAction("Station","Company");
                if(requestDto.Activities.Any(i => i.ActivityId != 0))
                {
                    await _activityService.UpdateActivity(requestDto);
                    return RedirectToAction("Station", "Company");
                }
                await _activityService.AddActivity(requestDto);
                return RedirectToAction("Station", "Company");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


    }

}*/
