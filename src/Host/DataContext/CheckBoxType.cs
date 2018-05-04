using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class CheckBoxType
    {
        [Key]
        public int PkChechBoxTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
    }
}
