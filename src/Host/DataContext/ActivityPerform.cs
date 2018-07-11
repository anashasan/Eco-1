using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class ActivityPerform
    {
        public ActivityPerform()
        {
            ActivityPerformDetail = new HashSet<ActivityPerformDetail>();
        }

        [Key]
        public int PkActivityPerformId { get; set; }
        [Required]
        [StringLength(450)]
        public string FkEmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }
        public int FkStationLocationId { get; set; }

        [ForeignKey("FkEmployeeId")]
        [InverseProperty("ActivityPerform")]
        public AspNetUsers FkEmployee { get; set; }
        [ForeignKey("FkStationLocationId")]
        [InverseProperty("ActivityPerform")]
        public StationLocation FkStationLocation { get; set; }
        [InverseProperty("FkActivityPerform")]
        public ICollection<ActivityPerformDetail> ActivityPerformDetail { get; set; }
    }
}
