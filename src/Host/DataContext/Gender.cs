using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class Gender
    {
        public Gender()
        {
            EmployeeProfile = new HashSet<EmployeeProfile>();
        }

        [Key]
        public byte PkGenderId { get; set; }
        [Required]
        [Column(TypeName = "nchar(10)")]
        public string Name { get; set; }

        [InverseProperty("FkGender")]
        public ICollection<EmployeeProfile> EmployeeProfile { get; set; }
    }
}
