using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eco.DataContext
{
    public partial class AspNetRoleClaims
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        [Required]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("AspNetRoleClaims")]
        public AspNetRoles Role { get; set; }
    }
}
