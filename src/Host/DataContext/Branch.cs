using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Branch
    {
        public Branch()
        {
            BranchEmployee = new HashSet<BranchEmployee>();
            BranchLocation = new HashSet<BranchLocation>();
            CompanyBranch = new HashSet<CompanyBranch>();
        }

        [Key]
        public int PkBranchId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Location { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [StringLength(250)]
        public string Phone { get; set; }
        [StringLength(250)]
        public string Email { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedON", TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }

        [InverseProperty("FkBranch")]
        public ICollection<BranchEmployee> BranchEmployee { get; set; }
        [InverseProperty("FkBranch")]
        public ICollection<BranchLocation> BranchLocation { get; set; }
        [InverseProperty("FkBranch")]
        public ICollection<CompanyBranch> CompanyBranch { get; set; }
    }
}
