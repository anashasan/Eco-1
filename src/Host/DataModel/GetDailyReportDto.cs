using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GetDailyReportDto
    {
        
        public string PkActitvityPerformDetailId { get; set; }
        public int FkActivityPerformId { get; set; }
        public string FkActivityId { get; set; }   
        public DateTime? CreatedOn  { get; set; }       
        public int Perform { get; set; }
        public bool IsPerform { get; set; }
        public string Name { get; set; }

    }

    

}
