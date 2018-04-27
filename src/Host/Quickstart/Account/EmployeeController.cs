using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Quickstart.Account
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;

        public EmployeeController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]

        public IActionResult EmployeeProfile()
        {
            var employees = _employeesService.GetAllEmployee();
            var model = new List<UserInfoModel>();
            model = employees;
           
            return View("EmployeeProfile", model);

        }
    }
}