using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GetCompanyViewDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public string EmployeeName { get; set; }
    }
}
