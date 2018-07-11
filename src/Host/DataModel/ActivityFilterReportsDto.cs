using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityFilterReportsDto
    {
        public string StationName { get; set; }
        public int StationNO { get; set; }
        public string LocationName { get; set; }
        public string ActivityName { get; set; }
        public DateTime CreateOn { get; set; }
        public string Perform { get; set; }
        public bool? IsPerform { get; set; }
    }
}
