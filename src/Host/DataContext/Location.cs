using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Location
    {
        public Location()
        {
            BranchLocation = new HashSet<BranchLocation>();
        }

        [Key]
        public int PkLocationId { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [InverseProperty("FkLocation")]
        public ICollection<BranchLocation> BranchLocation { get; set; }
    }
}
