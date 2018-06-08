using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class ActivityType
    {
        public ActivityType()
        {
            Activity = new HashSet<Activity>();
        }

        [Key]
        public int PkActivityTypeId { get; set; }
        [StringLength(50)]
        public string Type { get; set; }

        [InverseProperty("FkActivityType")]
        public ICollection<Activity> Activity { get; set; }
    }
}
