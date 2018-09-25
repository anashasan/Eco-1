using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ClientCompanyDropDownDto
    {
        public SelectList Companies { get; set; }
        public SelectList Users { get; set; }
        public int CompanyId { get; set; }
        public string UserId { get; set; }
        public Guid ClientCompanyId { get; set; }
    }
}
