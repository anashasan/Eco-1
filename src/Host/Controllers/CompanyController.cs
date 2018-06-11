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
        private readonly IActivityTypeService _activityTypeService;



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
                                 IStationLocationService stationLocationService,
                                 IActivityTypeService activityTypeService
                                 )
        {
            _companyService = companyService;
            _stationService = stationService;
            _activityService = activityService;
            _branchService = branchService;
            _branchEmployeeService = branchEmployeeService;
            _locationService = locationService;
            _qRCodeGenerator = qRCodeGenerator;
            _stationLocationService = stationLocationService;
            _activityTypeService = activityTypeService;

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
                if (ModelState.IsValid)
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
                    return RedirectToAction("Branch", new { companyId = requestDto.CompanyId });
                }
                await _branchService.AddBranch(requestDto);
                return RedirectToAction("Branch", new { companyId = requestDto.CompanyId });
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
                var model = _branchService.GetBranchById(branchId);
                var brancModel = new BranchDto
                {
                    Companies = new SelectList(companiesList, "CompanyId", "Name"),
                    CompanyId = companyId

                };

                model.Companies = brancModel.Companies;
                model.CompanyId = companyId;
                ViewBag.CompanyId = companyId;

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


                var stationloctionmodel = new StationLocationDto
                {
                    Stations = new SelectList(stationlist, "StationId", "Name"),
                    LocationId = locationId
                };
                model.Stations = stationloctionmodel.Stations;
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
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var stationName = _stationLocationService.GetStationNameById(id);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Chunk chunk = new Chunk(id.ToString());
                document.Add(chunk);

                Phrase phrase = new Phrase(stationName);
                document.Add(phrase);

                iTextSharp.text.Paragraph para = new iTextSharp.text.Paragraph("This is from paragraph.");
                document.Add(para);

                string text = @"you are successfully created PDF file.";
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
                paragraph.Add(text);
                document.Add(paragraph);

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                string pdfName = "User";
                var response = File(bytes, "application/pdf", $"{pdfName}.pdf");
                Response.Headers.Add("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                return response;
           

                //byte[] bytes = memoryStream.ToArray();
                //memoryStream.Close();
                //Response.Clear();
                //Response.ContentType = "application/pdf";

                //string pdfName = "User";
                ////var response = File("application /pdf", $"{pdfName}.pdf");
                //Response.Headers.Add("Content-Disposition", $"attachment; filename=" + pdfName + ".pdf");

                //Response.Headers.Add("Content-Disposition", $"attachment; filename={pdfName}.pdf");
                //return response;
                //Response.Buffer = true;
                //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                //Response.BinaryWrite(bytes);
                //Response.End();
                //Response.Close();
                //var client = new HttpClient();

                //WebClient webClient = new WebClient();
                //var documentName = "activity";
                //var url = "https://api.qrserver.com/v1/create-qr-code/?data=abc&amp;size=50x50%27";
                //var stream = await client.GetStreamAsync(url);
                //if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                //{
                //    using (System.Net.WebClient abc = new System.Net.WebClient())
                //    {
                //        abc.DownloadFileAsync(new Uri(url),
                //        "D:\\test.png");
                //        ComponentInfo.SetLicense("FREE-LIMITED-KEY");
                //        Document document = new Document();
                //        var doc = new DocumentModel();
                //        doc.Sections.Add(
                //            new Section(doc,
                //                new Paragraph(doc,
                //                    new Picture(doc, url))));
                //        doc.Save("Sample.pdf");
                //    }
                //}

                //var response = File(stream, "application/pdf", $"{documentName}.pdf");
                //Response.Headers.Add("Content-Disposition", $"attachment; filename={documentName}.pdf");
                //return null;
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

        [HttpDelete("Company/StationDelete/id/{id}")]
        public IActionResult DeleteStationActivity(int id)
        {
            
            var activity= _activityService.DeleteActivityById(id);
            return View("AddActivity",activity);
        }

        public IActionResult Station()
        {   
            var stations = _stationService.GetAllStation();
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
       
    } 
}