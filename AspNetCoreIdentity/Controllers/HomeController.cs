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
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipalFactory;
        private readonly SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _signInManager = signInManager;
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
                if (user != null && !await _userManager.IsLockedOutAsync(user))
                {
                    //                    var identity = new ClaimsIdentity("Identity.Application");
                    //                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Id));
                    //                    identity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));
                    //await HttpContext.SignInAsync("Identity.Application", new ClaimsPrincipal(identity));
                    if (await _userManager.CheckPasswordAsync(user, userViewModel.Password))
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "Email is not confirmed");
                            return View();
                        }

                        await _userManager.ResetAccessFailedCountAsync(user);
                        var claimPrinciple = await _claimsPrincipalFactory.CreateAsync(user);
                        await HttpContext.SignInAsync("Identity.Application", claimPrinciple);

                        return RedirectToAction("Index");
                    }

                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "you have been locked for 1 min");
                        // send mail through phone 
                    }
                }

                /**    
                     var loginResult = await 
                         _signInManager.PasswordSignInAsync(userViewModel.UserName, userViewModel.Password, false, false);

                     if (loginResult.Succeeded)
                     {
                         var user = await _userManager.FindByNameAsync(userViewModel.UserName);
                         if (!await _userManager.IsEmailConfirmedAsync(user))
                         {
                             ModelState.AddModelError("","Email is not confirmed");
                             return View();
                         }
                         return RedirectToAction("Index");
                     }

                **/
                ModelState.AddModelError("","Invalid username or password");
            }
            else
            {
                ModelState.AddModelError("password", "Please check password validity");
            }
            return View();
        }
    }
}
