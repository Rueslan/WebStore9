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
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region Registration

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            using (_logger.BeginScope("Регистрация пользователя {0}", model.UserName))
            {
                var user = new User { UserName = model.UserName };

                _logger.LogInformation("Регистрация пользователя {0}", user.UserName);

                var registerResult = await _userManager.CreateAsync(user, model.Password);
                if (registerResult.Succeeded)
                {
                    _logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);

                    await _userManager.AddToRoleAsync(user, Role.Users);

                    _logger.LogInformation("Пользователю {0} назначена роль {1}", user.UserName, Role.Users);

                    await _signInManager.SignInAsync(user, false);

                    _logger.LogInformation("Пользователь {0} вошёл в систему после регистрирации", user.UserName);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in registerResult.Errors)
                    ModelState.AddModelError("", error.Description);

                _logger.LogWarning("Ошибка регистрации пользователя {0}: {1}", user.UserName, string.Join(", ", registerResult.Errors.Select(e => e.Description)));
            }

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
            if (!ModelState.IsValid) 
                return View(model);

            var loginResult = await _signInManager.PasswordSignInAsync(
                model.UserName, 
                model.Password, 
                model.RememberMe, 
                false);

            if (loginResult.Succeeded)
            {
                _logger.LogInformation("Пользователь {0} успешно вошёл в систему", model.UserName);
                return LocalRedirect(model.ReturnUrl);
            }

            ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");

            _logger.LogWarning("Ошибка ввода имени пользователя или пароля при входе {0}", model.UserName);

            return View(model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity!.Name;

            await _signInManager.SignOutAsync();

            _logger.LogInformation("Пользователь {0} вышел из системы", userName);

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogWarning("Отказано в доступе {0} к uri: {1}", User.Identity!.Name, HttpContext.Request.Path);
            return View();
        }
    }
}
