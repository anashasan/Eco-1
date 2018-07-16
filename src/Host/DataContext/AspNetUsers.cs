using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Host.DataContext
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            ActivityPerform = new HashSet<ActivityPerform>();
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            Company = new HashSet<Company>();
            EmployeeProfileFkInitiatedBy = new HashSet<EmployeeProfile>();
            EmployeeProfileFkUser = new HashSet<EmployeeProfile>();
        }

        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        public bool? Status { get; set; }

        [InverseProperty("FkEmployee")]
        public ICollection<ActivityPerform> ActivityPerform { get; set; }
        [InverseProperty("User")]
        public ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        [InverseProperty("User")]
        public ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        [InverseProperty("User")]
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        [InverseProperty("FkUser")]
        public ICollection<Company> Company { get; set; }
        [InverseProperty("FkInitiatedBy")]
        public ICollection<EmployeeProfile> EmployeeProfileFkInitiatedBy { get; set; }
        [InverseProperty("FkUser")]
        public ICollection<EmployeeProfile> EmployeeProfileFkUser { get; set; }
    }
}
