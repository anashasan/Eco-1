using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Station
    {
        public Station()
        {
            ActivityPerform = new HashSet<ActivityPerform>();
            StationActivity = new HashSet<StationActivity>();
            StationLocation = new HashSet<StationLocation>();
        }

        [Key]
        public int PkStationId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreateOn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? UpdatedOn { get; set; }

        [InverseProperty("FkStation")]
        public ICollection<ActivityPerform> ActivityPerform { get; set; }
        [InverseProperty("FkStation")]
        public ICollection<StationActivity> StationActivity { get; set; }
        [InverseProperty("FkStation")]
        public ICollection<StationLocation> StationLocation { get; set; }
    }
}
