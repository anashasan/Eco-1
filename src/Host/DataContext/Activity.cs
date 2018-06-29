using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Activity
    {
        public Activity()
        {
            ActivityPerformDetail = new HashSet<ActivityPerformDetail>();
            StationActivity = new HashSet<StationActivity>();
        }

        [Key]
        public int PkActivityId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreateOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }
        public int FkActivityTypeId { get; set; }

        [ForeignKey("FkActivityTypeId")]
        [InverseProperty("Activity")]
        public ActivityType FkActivityType { get; set; }
        [InverseProperty("FkActivity")]
        public ICollection<ActivityPerformDetail> ActivityPerformDetail { get; set; }
        [InverseProperty("FkActivity")]
        public ICollection<StationActivity> StationActivity { get; set; }
    }
}
