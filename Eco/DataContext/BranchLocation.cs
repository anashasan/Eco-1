using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class BranchLocation
    {
        public BranchLocation()
        {
            StationLocation = new HashSet<StationLocation>();
        }

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
        [InverseProperty("FkBranchLocation")]
        public ICollection<StationLocation> StationLocation { get; set; }
    }
}
