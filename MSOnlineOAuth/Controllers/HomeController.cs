using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Graph;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MSOnlineOAuth.Controllers
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

        public IActionResult UserInfo()
        {
            string ObjectIdentifierType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
            string TenantIdType = "http://schemas.microsoft.com/identity/claims/tenantid";

            var identifier = User.FindFirst(ObjectIdentifierType)?.Value;




            return View("UserInfo");
        }

    }

    public class GraphService
    {

        // Get the current user's email address from their profile.
        public async Task<string> GetMyEmailAddress(GraphServiceClient graphClient)
        {

            // Get the current user. 
            // This sample only needs the user's email address, so select the mail and userPrincipalName properties.
            // If the mail property isn't defined, userPrincipalName should map to the email for all account types. 
            User me = await graphClient.Me.Request().Select("mail,userPrincipalName").GetAsync();
            return me.Mail ?? me.UserPrincipalName;
        }
    }

}