using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Company
    {
        public Company()
        {
            CompanyBranch = new HashSet<CompanyBranch>();
        }

        [Key]
        public int PkCompanyId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(250)]
        public string Url { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }
        [Column("UpdatedON", TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }
        [Required]
        [StringLength(450)]
        public string FkUserId { get; set; }

        [ForeignKey("FkUserId")]
        [InverseProperty("Company")]
        public AspNetUsers FkUser { get; set; }
        [InverseProperty("FkCompany")]
        public ICollection<CompanyBranch> CompanyBranch { get; set; }
    }
}
