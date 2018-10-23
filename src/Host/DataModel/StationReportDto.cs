using System.Collections.Generic;

namespace Host.DataModel
{
    public class StationReportDto
    {
        public string Month { get; set; }
        public string Year { get; set; }
        public int Perform { get; set; }
        //public int isperform { get; set; }
        public string ActivityName { get; set; }
        public string LocationName { get; set; }
        public string ActivityType { get; set; }
        public string StationName { get; set; }
        
    }

    public class MonthlyPerform
    {
        public string Month { get; set; }
        public int Perform { get; set; }
    }

    public class Activities
    {
        public string ActivityName { get; set; }
        public List<MonthlyPerform> MonthlyPerform { get; set; }
    }


    public class GraphActivityPerform
    {
        public string StationName { get; set; }
        public List<Activities>Activity {get;set ;}
        
        
    }

    public class Graph
    {
        public string LocationName { get; set; }
        public List<GraphActivityPerform> Stations { get; set; }
    }

    
    //public class StationActivityDto
    //{
    //    public string Station { get; set; }
    //    public List<GraphActivityPerform> graphActivityPerforms { get; set; }
    //}
}
