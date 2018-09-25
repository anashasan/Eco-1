using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ClientComanyDto
    {
        public Guid ClientCompanyId { get; set; }
        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ClientName { get; set; }
    }
}
