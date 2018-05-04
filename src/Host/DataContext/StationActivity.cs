using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class StationActivity
    {
        [Key]
        public int PkStationActivityId { get; set; }
        public int FkStationId { get; set; }
        public int FkActivityId { get; set; }
        [Column("QRCode")]
        [StringLength(500)]
        public string Qrcode { get; set; }

        [ForeignKey("FkActivityId")]
        [InverseProperty("StationActivity")]
        public Activity FkActivity { get; set; }
        [ForeignKey("FkStationId")]
        [InverseProperty("StationActivity")]
        public Station FkStation { get; set; }
    }
}
