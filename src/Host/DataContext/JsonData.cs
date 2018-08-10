using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class JsonData
    {
        [Key]
        public int PkJsonId { get; set; }
        [Required]
        [Column("JsonData")]
        public string JsonData1 { get; set; }
        public Guid GeneratedCode { get; set; }
    }
}
