using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GetStationActivityDto
    {
        public int StationId { get; set; }
        public string StationName { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }
}
