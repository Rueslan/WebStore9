using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore9.ViewModels.Identity;
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

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost]
        public IActionResult Register(RegisterUserViewModel Model)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View();

        public IActionResult Logout() => RedirectToAction("Index","Home");

        public IActionResult AccessDenied() => View();
    }
}
