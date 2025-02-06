using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebStore9Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";

        public const string DefaultAdminPassword = "AdPass_123";
    }
}
