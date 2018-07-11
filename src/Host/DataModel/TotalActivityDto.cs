using Host.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class TotalActivityDto
    {
        public String StationName { get; set; }
        public int StationId { get; set; }
        public int ActivityId { get; set; }
        public int StationNO { get; set; }
        public string LocationName { get; set; }
        public string ActivityName { get; set; }
        public string CreatedOn { get; set; }
        public string Perform { get; set; }
        public bool? IsPerform { get; set; }
        public int TotalNumberOfActivity { get; set; }
    }
}
