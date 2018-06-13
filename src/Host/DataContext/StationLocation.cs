﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class StationLocation
    {
        [Key]
        public int PkStationLocationId { get; set; }
        public int FkLocationId { get; set; }
        public int FkStationId { get; set; }
        [Required]
        [StringLength(250)]
        public string Code { get; set; }

        [ForeignKey("FkLocationId")]
        [InverseProperty("StationLocation")]
        public Location FkLocation { get; set; }
        [ForeignKey("FkStationId")]
        [InverseProperty("StationLocation")]
        public Station FkStation { get; set; }
    }
}
