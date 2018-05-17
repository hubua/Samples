/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MSOnlineOAuth.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                MicrosoftAccountDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            // return SignOut(
            //    new AuthenticationProperties { RedirectUri = callbackUrl },
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    MicrosoftAccountDefaults.AuthenticationScheme);
            // NOTE: No IAuthenticationSignOutHandler is configured to handle sign out for the scheme: Microsoft

            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToAction(nameof(SignedOut));
        }

        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
