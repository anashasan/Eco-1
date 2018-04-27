using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Quickstart.Client
{
    [Authorize]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View("ClientView");
        }
    }
}