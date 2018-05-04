// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Host;
using Host.Business.IDbServices;
using Host.Controllers;
using Host.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    public class HomeController : BaseController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IEmployeesService _employeesService;

        public HomeController(IIdentityServerInteractionService interaction,
                              UserManager<ApplicationUser> userManager,
                              IRoleService roleService,
                              IEmployeesService employeesService)
        {
            _interaction = interaction;
            _userManager = userManager;
            _roleService = roleService;
            _employeesService = employeesService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Sign");
        }

        public IActionResult Sign()
        {
            return View("Sign");
        }

        [Authorize]
        public  IActionResult AdminDashboard()
        {
            var userId = GetUserid();
            var roles = Getuserrole();
            var roleName = _roleService.GetRoleNameById(userId.ToString());
            if (roleName == "Client")
            {
                return RedirectToAction("Index","Client");
            }
            ViewBag.UserName = _employeesService.GetUserName(userId.ToString());
            return  View("AdminDashboard");
        }

        public IActionResult RoleView()
        {
            return View("RoleView");
        }

        [Authorize]
        public IActionResult AddCompany()
        {
            return View("Company");
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }

        public IActionResult GetQrCode()
        {
            //var qrcode = new QRCodeTagHelper();
           
            


            return View("QRcode");
        }


    }
}