using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class ActivityPerformDetail
    {
        public ActivityPerformDetail()
        {
            ActivityObservation = new HashSet<ActivityObservation>();
        }

        [Key]
        public int PkActivityPerformDetailId { get; set; }
        public int FkActivityPerformId { get; set; }
        public int FkActivityId { get; set; }
        public bool? IsPerform { get; set; }
        [StringLength(250)]
        public string Perform { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("FkActivityId")]
        [InverseProperty("ActivityPerformDetail")]
        public Activity FkActivity { get; set; }
        [ForeignKey("FkActivityPerformId")]
        [InverseProperty("ActivityPerformDetail")]
        public ActivityPerform FkActivityPerform { get; set; }
        [InverseProperty("FkActivityPerformDetail")]
        public ICollection<ActivityObservation> ActivityObservation { get; set; }
    }
}
