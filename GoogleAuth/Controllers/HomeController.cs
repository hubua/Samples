using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace GoogleAuthSample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Home", new { ReturnUrl = returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, new string[] { provider });
            // Request goes to accounts.google.com/signin/oauth?client_id=496059977833

            // Looks like Challenge executes AuthenticationHttpContextExtensions.SignInAsync
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            // After authenticating on Google request goes to localhost:61850/signin-google and then redirects to /Home/ExternalLoginCallback
            if (remoteError != null)
            {
                ViewBag.Message = remoteError;
                return View("Index");
            }
            
            return View("Index");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public IActionResult UserInfo()
        {
            return View("UserInfo");
        }

    }
}