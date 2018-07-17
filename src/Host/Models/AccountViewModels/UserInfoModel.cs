using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models.AccountViewModels
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        public bool LockedoutEnable { get; set; }
        public string NormalizeEmail { get; set; }
        [Required]
        public string NormalizeUserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfrimedPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnable { get; set; }
        [Required]
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public SelectList Roles { get; set; }
    }
}
