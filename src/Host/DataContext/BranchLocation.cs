using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class BranchLocation
    {
        [Key]
        public int PkBranchLocationId { get; set; }
        public int FkBranchId { get; set; }
        public int FkLocationId { get; set; }

        [ForeignKey("FkBranchId")]
        [InverseProperty("BranchLocation")]
        public Branch FkBranch { get; set; }
        [ForeignKey("FkLocationId")]
        [InverseProperty("BranchLocation")]
        public Location FkLocation { get; set; }
    }
}
