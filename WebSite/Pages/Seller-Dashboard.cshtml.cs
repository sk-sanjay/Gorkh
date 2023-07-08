using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSite.Helpers;

namespace WebSite.Pages
{
    [Authorize]
    public class Seller_DashboardModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IRandomService _randomService;
        private readonly INotyfService _notyf;
        public Seller_DashboardModel(IHttpClientService httpClient,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration config,
            IRandomService randomService,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _randomService = randomService;
            _notyf = notyf;
        }
        public string role => DataHelper.GetUserRole(User);
        public string uid => DataHelper.GetUserId(User);

        public async Task<IActionResult> OnGetPrivilegeLoginBuyer()
        {
            string Id = uid;
            string Role = "Buyer";
            var result = await _httpClient.GetAsync("Auth/PrivilegeLoginBuyer", true, Id, Role).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result)) return RedirectToPage();
            var tokenVm = JsonConvert.DeserializeObject<TokenVM>(result);
            if (tokenVm == null)
            {
                _notyf.Error("Invalid login attempt");
                return RedirectToPage();
            }
           
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!(tokenHandler.ReadToken(tokenVm.AccessToken) is JwtSecurityToken payload))
            {
                _notyf.Error("Invalid token");
                return RedirectToPage();
            }
            //await UserSignInAsync(payload).ConfigureAwait(false);
            ////Set Profile Image to session on login
            //var ProfileImage = !string.IsNullOrEmpty(payload.Claims.First(c => c.Type == "img").Value) ? payload.Claims.First(c => c.Type == "img").Value : _config["DefaultUserImage"];
            //HttpContext.Session.SetString("ProfileImage", ProfileImage);
            ////Check if login for first time with default password
            //var chn = payload.Claims.First(c => c.Type == "chn").Value.ToString();
            ////if (chn != "Y") return LocalRedirect(Url.Content("~/"));
            //if (chn != "Y") return LocalRedirect("/Seller-Dashboard");
            //HttpContext.Session.SetString("chn", "Y");
            //return LocalRedirect(Url.Content("/Account/ChangePassword"));

            HttpContext.Session.Remove("Glass");
            //Set access token cookie
            SetTokenCookies(tokenVm);
            
            await UserSignInAsync(payload).ConfigureAwait(false);
            
            //Set Profile Image to session on login
            var ProfileImage = !string.IsNullOrEmpty(payload.Claims.First(c => c.Type == "img").Value) ? payload.Claims.First(c => c.Type == "img").Value : _config["DefaultUserImage"];
            HttpContext.Session.SetString("ProfileImage", ProfileImage);
            
            //Check if login for first time with default password
            var chn = payload.Claims.First(c => c.Type == "chn").Value.ToString();
            if (chn != "Y") return LocalRedirect("/Seller-Dashboard");
            HttpContext.Session.SetString("chn", "Y");
            return LocalRedirect(Url.Content("./Account/ChangePassword"));
        }
        public async Task<IActionResult> OnGetPrivilegeLoginSeller()
        {
            string Id = uid;
            string Role = "Seller";
            var result = await _httpClient.GetAsync("Auth/PrivilegeLoginSeller", true, Id, Role).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result)) return RedirectToPage();
            var tokenVm = JsonConvert.DeserializeObject<TokenVM>(result);
            if (tokenVm == null)
            {
                _notyf.Error("Invalid login attempt");
                return RedirectToPage();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!(tokenHandler.ReadToken(tokenVm.AccessToken) is JwtSecurityToken payload))
            {
                _notyf.Error("Invalid token");
                return RedirectToPage();
            }


            // HttpContext.Session.Remove("Glass");
            //Set access token cookie
            SetTokenCookies(tokenVm);
            await UserSignInAsync(payload).ConfigureAwait(false);
            //Set Profile Image to session on login
            var ProfileImage = !string.IsNullOrEmpty(payload.Claims.First(c => c.Type == "img").Value) ? payload.Claims.First(c => c.Type == "img").Value : _config["DefaultUserImage"];
            HttpContext.Session.SetString("ProfileImage", ProfileImage);
            //Check if login for first time with default password
            var chn = payload.Claims.First(c => c.Type == "chn").Value.ToString();
            if (chn != "Y") return LocalRedirect("/Seller-Dashboard");
            HttpContext.Session.SetString("chn", "Y");
            return LocalRedirect(Url.Content("/Account/ChangePassword"));
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
        private void SetTokenCookies(TokenVM tokenVm)
        {
            var cookieOptions = new CookieOptions
            {
                Domain = _config["Domain"],
                Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_config["CookieExpiry"])),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURPS.AuthToken", tokenVm.AccessToken, cookieOptions);
            var cookieOptions2 = new CookieOptions
            {
                Domain = _config["Domain"],
                Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_config["CookieExpiry2"])),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURPS.RefreshToken", tokenVm.RefreshToken, cookieOptions2);
        }
        private async Task UserSignInAsync(JwtSecurityToken payload)
        {
            var claimsIdentity = new ClaimsIdentity(payload.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(Convert.ToInt32(_config["CookieExpiry"]))
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties).ConfigureAwait(false);
        }

    }
}
