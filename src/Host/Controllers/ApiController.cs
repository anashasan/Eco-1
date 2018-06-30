using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class ApiController : BaseController
    {
        private readonly IGraphService _graphService;

        public ApiController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [AllowAnonymous]
        [HttpGet("Api/GraphData")]
        public IActionResult GetActivity()
        {
            var graph = _graphService.Graph();
            return Json(graph);
        }
    }
}