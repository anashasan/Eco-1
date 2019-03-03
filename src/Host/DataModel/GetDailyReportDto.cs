using System;

namespace Host.DataModel
{
    public class GetDailyReportDto
    {
        
        public int PkActivityPerformDetailId { get; set; }
        public int FkActivityPerformId { get; set; }
        public int FkActivityId { get; set; }   
        public DateTime? CreatedOn  { get; set; }       
        public string Perform { get; set; }
        public bool IsPerform { get; set; }
        public string Name { get; set; }

    }

    

}
