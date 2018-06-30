using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GraphDto
    {
        public List<int> NumberOfActivities { get; set; }
        public string StationName { get; set; }
        public List<String> ActivityName { get; set; }
        public int Activity { get; set; }
    }
}
