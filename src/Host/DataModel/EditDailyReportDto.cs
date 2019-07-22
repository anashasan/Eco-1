using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class EditDailyReportDto
    {
        public int PkActivityPerformDetailId { get; set; }
        public DateTime? Date { get; set; }
        public string Perform { get; set; }
        public bool? IsPerform { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
    }
}
