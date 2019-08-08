using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                    var result = await _userManager.CreateAsync(new IdentityUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = registerViewModel.UserName
                    },registerViewModel.Password);

                    if (!result.Succeeded)
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userViewModel.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user,userViewModel.Password))
                {
                    var identity = new ClaimsIdentity("cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("","Invalid username or password");
            }
            return View();
        }
    }
}
