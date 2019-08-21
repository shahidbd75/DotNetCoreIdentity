using System;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = Url.Action("ResetPassword","Authentication", new {token = token, email = user.Email},Request.Scheme);

                    System.IO.File.WriteAllText("resetLink.txt",resetLink);
                    return View("Success");
                }
                else
                {
                    ModelState.AddModelError("email","Do not have any account with this email");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            return View(new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            });
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, viewModel.Token, viewModel.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var identityError in result.Errors)
                        {
                            ModelState.AddModelError("",identityError.Description);
                        }
                    }

                    return View("Success");
                }
                ModelState.AddModelError("","Invalid Request");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(registerViewModel.UserName);
                if (user == null)
                {
                    user = new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = registerViewModel.UserName,
                        Email = registerViewModel.Email
                    };

                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var emailConfirmedLink = Url.Action("ConfirmEmailAddress","Authentication",
                            new {token = token, email = user.Email}, Request.Scheme);

                        System.IO.File.WriteAllText("emailConfirmedLink.txt",emailConfirmedLink);
                    }
                    else
                    {
                        foreach (var identityError in result.Errors)
                        {
                            ModelState.AddModelError(identityError.Code, identityError.Description);
                        }
                        return View(registerViewModel);
                    }
                }

                return View("Success");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var isConfirmed = await _userManager.ConfirmEmailAsync(user, token);
                if (isConfirmed.Succeeded)
                {
                    return View("SuccessWithMessage","Email is Confirmed. Goto Login");
                }
            }
            return View("ErrorWithMessage", "Invalid User Request");
        }
    }
}