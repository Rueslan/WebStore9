using Microsoft.AspNetCore.Identity;

namespace WebStore9Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";

        public const string DefaultAdminPassword = "AdPass_123";
    }
}
