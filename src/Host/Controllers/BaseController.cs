using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Host.Business.IDbServices;
using Host.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    public class BaseController : Controller
    {

        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid GetUserid()
        {
            try
            {
                var id = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "sub")?.Value);
                return id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string Getuserrole()
        {
            var s = User.Claims.FirstOrDefault(p => p.Type == "role")?.Value;
            return s;
        }

        
    }
}