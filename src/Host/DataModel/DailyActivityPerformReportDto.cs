using Newtonsoft.Json;
using System.Collections.Generic;

namespace Host.DataModel
{
    public class DailyActivityPerformReportDto
    {
        [JsonIgnore]
        public string StationName { get; set; }
        public int StationNo { get; set; }
        public string LocationName { get; set; }
        [JsonIgnore]
        public string ActivityPerformJson { get; set; }
        public List<DailyActivityPerformDetailDto> ActivityPerform { get; set; }
    }
}
