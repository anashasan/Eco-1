using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class ActivityObservation
    {
        [Key]
        public int PkActivityObservationId { get; set; }
        public int FkActivityPerformDetailId { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public string ClientReview { get; set; }
        public byte[] ObservationImage { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [Column("CLinetReviewDate", TypeName = "date")]
        public DateTime? ClinetReviewDate { get; set; }

        [ForeignKey("FkActivityPerformDetailId")]
        [InverseProperty("ActivityObservation")]
        public ActivityPerformDetail FkActivityPerformDetail { get; set; }
    }
}
