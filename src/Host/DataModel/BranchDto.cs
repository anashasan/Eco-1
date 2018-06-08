using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class BranchDto
    {
        public int BranchId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int CompanyId { get; set; }
        public SelectList Companies { get; set; }
    }
}
