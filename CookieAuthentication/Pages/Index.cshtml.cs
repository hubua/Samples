using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CookieAuthentication.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CookieAuthentication.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public Person Person { get; set; }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = Person.Id + "email.com";

            var claimsIdentity = new ClaimsIdentity(new List<Claim> { new Claim("name", Person.Name), new Claim("email", email) }, "CustomCookies"); // authenticationType must be non-null to have User.Identity.IsAuthenticated working
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, Startup.COOKIE_AUTH_SCHEME, claimsPrincipal);

            return RedirectToPage("/Contact");
        }

        public async Task<IActionResult> OnPostSignOutAsync(int id)
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToPage("/Contact");
        }


    }
}
