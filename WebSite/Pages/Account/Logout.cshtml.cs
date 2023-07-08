using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace WebSite.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public LogoutModel(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public async Task<IActionResult> OnGet()
        {
            //sign out of cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            //Clear the cookies
            ClearSessionCookies();
            //redirect to home page
            return RedirectToPage("/Account/Seller-Login");
        }
        private void ClearSessionCookies()
        {
            //clear the session data
            _httpContextAccessor.HttpContext.Session.Clear();
            //delete the session cookie from client  (browser)
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURPS.Session");
            //remove the token cookie from response
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURPS.AuthToken");
            //remove the refresh token cookie from response
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(".SURPS.RefreshToken");
            //delete the token cookie from client (browser)
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                Domain = _config["Domain"]
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURPS.AuthToken", "token", options);
        }
    }
}
