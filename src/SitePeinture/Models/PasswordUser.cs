using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SitePeinture.Models
{
    public class PasswordUser
    {
        public string NewPassword { get; set; }

        public string CurrentPassword { get; internal set; }
    }
}
