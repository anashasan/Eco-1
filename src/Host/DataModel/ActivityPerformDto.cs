using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityPerformDto
    {
        public int StationId { get; set; }
        public string EmployeeId { get; set; }
        public string StationName { get; set; }
        public List<ActivityPerformDetailDto> Activities { get; set; }

    }
}
