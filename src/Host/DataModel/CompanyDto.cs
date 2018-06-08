using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class CompanyDto
    {
        public int? CompanyId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(60)]
        [Required]
        public string Type { get; set; }
        [StringLength(60)]
        [Required]
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UserId { get; set; }
    }
}
