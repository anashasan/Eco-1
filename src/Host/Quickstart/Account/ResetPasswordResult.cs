using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.Quickstart.Account
{
    public class ResetPasswordResult
    {
        public ResetPasswordResult(
            IdentityResult identityResult
            )
        {
            IdentityResult = identityResult;
        }
        
        public IdentityResult IdentityResult { get; }
    }
}
