using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class BranchEmployee
    {
        [Key]
        public int PkBranchEmployeeId { get; set; }
        [Required]
        [StringLength(250)]
        public string EmployeeName { get; set; }
        [StringLength(250)]
        public string Designation { get; set; }
        [StringLength(250)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        public int FkBranchId { get; set; }

        [ForeignKey("FkBranchId")]
        [InverseProperty("BranchEmployee")]
        public Branch FkBranch { get; set; }
    }
}
