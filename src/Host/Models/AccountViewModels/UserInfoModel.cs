using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Models.AccountViewModels
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockedoutEnable { get; set; }
        public string NormalizeEmail { get; set; }
        public string NormalizeUserName { get; set; }
        public string Password { get; set; }
        public string ConfrimedPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnable { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public SelectList Roles { get; set; }
    }
}
