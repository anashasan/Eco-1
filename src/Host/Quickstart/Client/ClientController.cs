using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.Controllers;
using Host.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Quickstart.Client
{
    [Authorize]
    public class ClientController : BaseController
    {
        private readonly IEmployeeProfileService _employeeProfileService;

        public ClientController(IEmployeeProfileService employeeProfileService)
        {
            _employeeProfileService = employeeProfileService;
        }

        public IActionResult Index()
        {
            return View("ClientView");
        }
        public IActionResult Employee()
        {
            var userId = GetUserid().ToString();
            var employeeModel = _employeeProfileService.GetEmployeeProfileByUserId(userId);
            if(employeeModel == null)
            {
                return View("Employee");
            }
            return View("Employee", employeeModel);
        }
    }
}