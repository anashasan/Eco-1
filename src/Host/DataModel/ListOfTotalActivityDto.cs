using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ListOfTotalActivityDto
    {
        public string StationName { get; set; }
        public int StationId { get; set; }
        public List<TotalActivityDto> TotalActivities { get; set; }
    }
}
