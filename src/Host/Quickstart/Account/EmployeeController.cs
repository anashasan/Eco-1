using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.DataModel;
using Host.Helper;
using Host.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Quickstart.Account
{
    [Authorize(Roles ="Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly IRoleService _roleService;

        public EmployeeController(IEmployeesService employeesService,
                                  IRoleService roleService)
        {
            _employeesService = employeesService;
            _roleService = roleService;
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterEmployee(string code)
        {
            try
            {
                var isauthenticated = User.Identities.First().IsAuthenticated;
                if (isauthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (code != "" && code != null)
                {
                    var emp = _roleService.GetEmailById(EncoderAgent.DecryptString(code).ToString());

                    if(emp != null)
                    {
                        var resetPassword = new ResetPasswordViewModel
                        {
                            Email = emp.Email,
                        };

                        return View("ResetPassword", resetPassword);
                    }
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SendLink(ForgetPasswordDto forgetPasswordDto)
        {
            var id = _roleService.GetUserIdByEmail(forgetPasswordDto.Email);
            var url = Request.Host.Value;
            var sendEmail = new EmailOptions();
            sendEmail.SendPasswordResetEmail(forgetPasswordDto.Email, "Reset Your Password", id, url);
            return RedirectToAction("Sign", "Home");

        }

        [HttpGet]
        public IActionResult UserInActive(string userId)
        {
            _employeesService.UserInActive(userId);

            return RedirectToAction("EmployeeProfile");
        }
       

        //[HttpPost]
        //public async Task<IActionResult> RegisterEmployee(RegisterViewModel registerViewModel)
        //{
        //    try
        //    {
        //        var result = await _employeeServices.UpdatePassword(registerViewModel.UserId, registerViewModel.ConfirmPassword);
        //        if (result)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        return View(registerViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw;
        //    }
        //}
    }
}