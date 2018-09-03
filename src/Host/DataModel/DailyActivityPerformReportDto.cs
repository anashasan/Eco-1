
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class DailyActivityPerformReportDto
    {
        private List<DailyActivityPerformDetailDto> _activityPerform;

        public string StationName { get; set; }
        public int StationNo { get; set; }
        public string LocationName { get; set; }
        [JsonIgnore]
        public string ActivityPerformJson { get; set; }
        public List<DailyActivityPerformDetailDto> ActivityPerform { get; set; }
    }
}
