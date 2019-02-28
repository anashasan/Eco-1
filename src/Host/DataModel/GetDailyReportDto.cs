using System;

namespace Host.DataModel
{
    public class GetDailyReportDto
    {
        
        public int PkActitvityPerformDetailId { get; set; }
        public int FkActivityPerformId { get; set; }
        public int FkActivityId { get; set; }   
        public DateTime? CreatedOn  { get; set; }       
        public int Perform { get; set; }
        public bool IsPerform { get; set; }
        public string Name { get; set; }

    }

    

}
