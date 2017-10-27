using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GoogleAuthSample.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace GoogleAuthSample.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Home", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
            // Request goes to accounts.google.com/signin/oauth?client_id=496059977833
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            // After authenticating on Google request goes to localhost:61850/signin-google and then redirects to /Home/ExternalLoginCallback
            if (remoteError != null)
            {
                ViewBag.Message = remoteError;
                return View("Index");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ViewBag.Message = "You are NOT logged in";
                return View("Index");
            }


            HttpContext.Session.SetString("email", info.Principal.FindFirstValue(ClaimTypes.Email));

            var claimsIdentity = new ClaimsIdentity(new List<Claim> { new Claim("name", "Rob") }, "CustomCookies"); // authenticationType must be non-null to have User.Identity.IsAuthenticated working
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            


            await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, "MyCookieAuthenticationScheme1", claimsPrincipal);
            return View("Index");

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email);

                ViewBag.Message = "You are logged in as " + email.Value;
                ViewBag.Claims = info.Principal.Claims;
                return View("Index");
            }
            else
            {
                ViewBag.Message = "not succedded";
                return View("Index");
            }
           
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.Clear();
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            await _userManager.CreateAsync(new AppUser() { UserName = "Mark" });
            

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> UserInfo()
        {
            ViewBag.Message = $"User:{User.FindFirstValue(ClaimTypes.Email)}, Session:{HttpContext.Session.GetString("email")},";
            return View("UserInfo");
        }

    }
}