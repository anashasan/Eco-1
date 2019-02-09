using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityPerformDto1
    {
        public int StationLocationId { get; set; }
        public string EmployeeId { get; set; }
        public string StationName { get; set; }
        public List<ActivityPerformDetailDto1> Activities { get; set; }
    }
}
