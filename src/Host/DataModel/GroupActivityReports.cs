using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GroupActivityReports
    {
        public string StatioName { get; set; }
        public List<ActivityFilterReportsDto> DailyReports { get; set; }
    }
}
