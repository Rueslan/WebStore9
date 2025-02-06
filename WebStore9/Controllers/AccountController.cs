using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register() => View();

        public IActionResult Login() => View();

        public IActionResult Logout() => RedirectToAction("Index","Home");

        public IActionResult AccessDenied() => View();
    }
}
