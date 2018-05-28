using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GetStationDto
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Activities { get; set; }
    }
}
