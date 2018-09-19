using System.Collections.Generic;

namespace Host.DataModel
{
    public class StationReportDto
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public int Perform { get; set; }
        public int isperform { get; set; }
        public string station { get; set; }
        public string Activity { get; set; }
        public string location { get; set; }
        public string ActivityType { get; set; }
    }

    public class MonthlyPerform
    {
        public string Month { get; set; }
        public int Value { get; set; }
    }

    public class GraphActivityPerform
    {
        public string Activity { get; set; }
        public List<MonthlyPerform> MonthlyPerform { get; set; }
    }
}
