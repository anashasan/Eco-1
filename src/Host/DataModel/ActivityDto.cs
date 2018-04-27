using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ActivityDto
    {
        public int ActivityId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
