using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class StationLocation
    {
        [Key]
        public int PkStationLocationId { get; set; }
        public int FkBranchLocationId { get; set; }
        public int FkStationId { get; set; }

        [ForeignKey("FkBranchLocationId")]
        [InverseProperty("StationLocation")]
        public BranchLocation FkBranchLocation { get; set; }
        [ForeignKey("FkStationId")]
        [InverseProperty("StationLocation")]
        public Station FkStation { get; set; }
    }
}
