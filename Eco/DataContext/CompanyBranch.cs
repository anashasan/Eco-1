using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class CompanyBranch
    {
        [Key]
        public int PkCompanyBranchId { get; set; }
        public int FkCompanyId { get; set; }
        public int FkBranchId { get; set; }

        [ForeignKey("FkBranchId")]
        [InverseProperty("CompanyBranch")]
        public Branch FkBranch { get; set; }
        [ForeignKey("FkCompanyId")]
        [InverseProperty("CompanyBranch")]
        public Company FkCompany { get; set; }
    }
}
