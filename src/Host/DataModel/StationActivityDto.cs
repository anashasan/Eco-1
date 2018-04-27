using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class StationActivityDto
    {
        public int StationId { get; set; }
        public List<ActivityDto> Activities { get; set; }
    }
}
