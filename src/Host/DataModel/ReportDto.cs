using System.Collections.Generic;

namespace Host.DataModel
{
    public class ReportDto
    {
        public List<DailyActivityPerformReportDto> DailyActivityPerformReport { get; set; }
        public string StationName { get; set; }
        public List<string> Activities { get; set; }
    }
}
