using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityPerformDetailDto
    {
        public int ActivityId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public int StationId { get; set; }
        public int ActivityTypeId { get; set; }
        public string Type { get; set; }
        public int StationActivityId { get; set; }
        public List<string> Observations { get; set; }
        public bool IsPerform { get; set; }
        public string Perform { get; set; }
    }
}
