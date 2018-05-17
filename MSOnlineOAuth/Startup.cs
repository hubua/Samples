using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using static Microsoft.Graph.Constants;

namespace MSOnlineOAuth
{
    public class Startup
    {
        // https://apps.dev.microsoft.com/#/appList
        public const string CLIENT_ID = "46b0681d-0ca3-4928-b10a-09dc90552bbb";
        public const string CLIENT_SECRET = "dbgkWEEE3155;kdnRDH9%%#";


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/microsoft-logins?view=aspnetcore-2.1&tabs=aspnetcore2x
            services.AddAuthentication(auth =>
            {
                auth.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
                auth.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                auth.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.Cookie.Name = "AuthCookie";
            })
            .AddMicrosoftAccount(options =>
            {
                options.ClientId = CLIENT_ID;
                options.ClientSecret = CLIENT_SECRET;
            });

            // Add framework services.
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // env.IsDevelopment()
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseAuthentication(); // Must come before UseMvc

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureAuth(IApplicationBuilder app)
        {
            throw new NotImplementedException();
        }
    }
}
