using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
        }

        public IActionResult Register() => View();

        public IActionResult Login() => View();

        public IActionResult Logout() => RedirectToAction("Index","Home");

        public IActionResult AccessDenied() => View();
    }
}
