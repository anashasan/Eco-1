using System.Collections.Generic;
using Newtonsoft.Json;

namespace Host.DataModel
{
    public class DailyActivityPerformReportDto
    {
        [JsonIgnore]
        public string StationName { get; set; }
        public int StationNo { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        [JsonIgnore]
        public string ActivityName { get; set; }
        [JsonIgnore]
        public int Perform { get; set; }

        public List<ActivityPerformance> ActivityPerform { get; set; }
    }

    public class ActivityPerformance
    {
        public string ActivityName { get; set; }
        public int Perform { get; set; }
    }
}
