using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class BranchStationLocationDto
    {
        public int StationId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string StationName { get; set; }
    }
}
