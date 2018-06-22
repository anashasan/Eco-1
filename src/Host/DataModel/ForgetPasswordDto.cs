using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Host.DataModel
{
    public class ForgetPasswordDto
    {
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
