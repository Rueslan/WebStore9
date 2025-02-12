﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore9.ViewModels.Identity;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        
        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
        }

        #region Registration

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var user = new User { UserName = Model.UserName };

            var register_result = await _UserManager.CreateAsync(user, Model.Password);
            if (register_result.Succeeded)
            {
                await _SignInManager.SignInAsync(user, false);

                await _UserManager.AddToRoleAsync(user, Role.Users);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in register_result.Errors)
                ModelState.AddModelError("", error.Description);


            return View(Model);
        }

        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl = "/") => View(new LoginViewModel{ ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName, 
                Model.Password, 
                Model.RememberMe, 
                false);

            if (login_result.Succeeded)
            {
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка ввода имени пользователя или пароля");
            return View(Model);
        }

        #endregion

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
