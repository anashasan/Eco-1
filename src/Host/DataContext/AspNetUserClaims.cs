using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class AspNetUserClaims
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AspNetUserClaims")]
        public AspNetUsers User { get; set; }
    }
}
