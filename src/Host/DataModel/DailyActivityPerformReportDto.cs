using Newtonsoft.Json;

namespace Host.DataModel
{
    public class DailyActivityPerformReportDto
    {
        [JsonIgnore]
        public string StationName { get; set; }
        public int StationNo { get; set; }
        public string LocationName { get; set; }
        public string ActivityName { get; set; }
        public int Perform { get; set; }
    }
}
