using System;

namespace Host.DataModel
{
    public class GetDailyReportDto
    {
        
        public int PkActivityPerformDetailId { get; set; }
        public int FkActivityPerformId { get; set; }
        public int FkActivityId { get; set; }   
        public DateTime? Date { get; set; }       
        public string Perform { get; set; }
        public bool? IsPerform { get; set; }
        public string Name { get; set; }
        public int StationNo { get; set; }
        public string StationName { get; set; }
        public string LocationName { get; set; }
        public string ActivityName { get; set; }
        public int BranchId { get; set; }
       
    }

    

}
