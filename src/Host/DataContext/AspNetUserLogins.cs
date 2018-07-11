using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class AspNetUserLogins
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AspNetUserLogins")]
        public AspNetUsers User { get; set; }
    }
}
