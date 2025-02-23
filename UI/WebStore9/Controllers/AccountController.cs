using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.ViewModels.Identity;

namespace WebStore9.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Registration

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User { UserName = model.UserName };

            var registerResult = await _userManager.CreateAsync(user, model.Password);
            if (registerResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                await _userManager.AddToRoleAsync(user, Role.Users);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registerResult.Errors)
                ModelState.AddModelError("", error.Description);


            return View(model);
        }

        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/") => View(new LoginViewModel{ ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName, 
                model.Password, 
                model.RememberMe, 
                false);

            if (loginResult.Succeeded)
                return LocalRedirect(model.ReturnUrl);

            ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");
            return View(model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
