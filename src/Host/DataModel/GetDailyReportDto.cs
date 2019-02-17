using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class GetDailyReportDto
    {
        public int ActivityPerformDetailId { get; set; }
        public string Activity { get; set; }
        public string Perform { get; set; }
        public bool? IsPerform { get; set; }
        public DateTime? ActivityDate { get; set; }
       
    }

    

}
