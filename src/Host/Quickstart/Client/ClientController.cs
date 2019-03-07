using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.Controllers;
using Host.DataContext;
using Host.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Quickstart.Client
{
    [Authorize(Roles ="Client")]
    public class ClientController : BaseController
    {
        private readonly IEmployeeProfileService _employeeProfileService;
        private readonly ICompanyService _companyService;
        private readonly EcoDbContext _context;

        public ClientController(IEmployeeProfileService employeeProfileService, ICompanyService companyService, EcoDbContext context)
        {
            _employeeProfileService = employeeProfileService;
            _companyService = companyService;
            _context = context;
        }

        public IActionResult Index()
        {

            var companyId = _context.ClientCompany.Where(i => i.FkEmployeeId == GetUserid().ToString()).Select(i => i.FkCompanyId).Single();
            var companyDto = _companyService.GetCompanyById(companyId);

            if (companyDto == null)
            {
                var model = new CompanyDto();
                return RedirectToAction("ClientCompanyList", "Company", companyDto);
            }
            return RedirectToAction("ClientCompanyList", "Company", companyDto);
        }
        public IActionResult Employee()
        {
            var userId = GetUserid().ToString();
            var employeeModel = _employeeProfileService.GetEmployeeProfileByUserId(userId);
            if(employeeModel == null)
            {
                var model = new EmployeeProfileDto();
                return View("Employee", model);
            }
            return View("Employee", employeeModel);
        }
        public IActionResult EditEmployee()
        {
            var userId = GetUserid().ToString();
            var employeeModel = _employeeProfileService.GetEmployeeProfileByUserId(userId);
            if (employeeModel == null)
            {
                return View("EditEmployee");
            }

            return View("EditEmployee",employeeModel);
        }
        
        [HttpPost]
        public IActionResult UpdateEmployee(EmployeeProfileDto requestDto)
        {
            try
            {
                _employeeProfileService.UpdateEmployeeProfile(requestDto);
                return RedirectToAction("Employee");

            }
            catch (Exception)
            {

                throw;
            }

        }

        public IActionResult ClientCompany()
        {
            var companyId = _context.ClientCompany.Where(i => i.FkEmployeeId == GetUserid().ToString()).Select(i => i.FkCompanyId).Single();
            var companyDto =  _companyService.GetCompanyById(companyId);
          
            if (companyDto == null)
            {
                var model = new CompanyDto();
                return RedirectToAction("ClientCompanyList", "Company", companyDto);
            }
            return RedirectToAction("ClientCompanyList", "Company", companyDto);
        }
    }
}