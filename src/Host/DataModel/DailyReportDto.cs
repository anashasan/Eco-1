using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class DailyReportDto
    {
        public int ActivityPerformDetailId { get; set; }
        public int BranchId { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int StationNumber { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public string ActivityName { get; set; }
        public int Perform { get; set; }
        public bool IsPerform { get; set; }
    }
}
