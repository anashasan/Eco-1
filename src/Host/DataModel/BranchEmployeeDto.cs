using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class BranchEmployeeDto
    {
        public int BranchEmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string Phone { get; set; }
        public SelectList Companies { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }
        public SelectList Branches { get; set; }
    }
}
