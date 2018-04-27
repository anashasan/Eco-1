using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class Activity
    {
        public Activity()
        {
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

        [InverseProperty("FkActivity")]
        public ICollection<StationActivity> StationActivity { get; set; }
    }
}
