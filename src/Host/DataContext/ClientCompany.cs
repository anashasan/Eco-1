using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class ClientCompany
    {
        [Key]
        public Guid PkClientCompanyId { get; set; }
        public int FkCompanyId { get; set; }
        [Required]
        [StringLength(450)]
        public string FkEmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("FkCompanyId")]
        [InverseProperty("ClientCompany")]
        public Company FkCompany { get; set; }
        [ForeignKey("FkEmployeeId")]
        [InverseProperty("ClientCompany")]
        public AspNetUsers FkEmployee { get; set; }
    }
}
