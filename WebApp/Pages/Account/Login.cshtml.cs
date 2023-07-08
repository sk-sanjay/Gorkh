using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IRandomService _randomService;
        private readonly INotyfService _notyf;
        public LoginModel(
            IHttpClientService httpClient,
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
        [BindProperty] public LoginDTO Input { get; set; }
        public string ReturnUrl { get; set; }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
            if (User.Identity != null && User.Identity.IsAuthenticated) return LocalRedirect(Url.Content(ReturnUrl));
            HttpContext.Session.SetString("Glass", await _randomService.RandomPassword().ConfigureAwait(false));
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            // Validate Captcha Code
            if (!Captcha.ValidateCaptchaCode(Input.CaptchaCode, HttpContext))
                ModelState.AddModelError("Captcha", "Please enter correct captcha");
          
            //Validate model
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString());
                return Page();
            }
            Input.Username = "U$erName";
            Input.EncUsername = Input.EncUsername.Substring(8);
            var emptyGlass = HttpContext.Session.GetString("Glass");
            var filledGlass = Input.EncPassword.Substring(0, 8);
            if (emptyGlass != filledGlass)
            {
                _notyf.Error("Invalid login attempt");
                HttpContext.Session.Remove("Glass");
                return RedirectToPage();
            }
            var result = await _httpClient.PostAsync("Auth/Login", false, Input).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result))
            {
                HttpContext.Session.Remove("Glass");
                _notyf.Error("Invalid Login Attempt.<br/>Possible reasons:<br/>1. User not found, not Approved, or not Active.<br/>2. Password Incorrect.<br/>3. Account Locked Out.");
                return RedirectToPage();
            }
            var tokenVm = JsonConvert.DeserializeObject<TokenVM>(result);
            if (tokenVm == null)
            {
                _notyf.Error("Invalid login attempt");
                HttpContext.Session.Remove("Glass");
                return RedirectToPage();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!(tokenHandler.ReadToken(tokenVm.AccessToken) is JwtSecurityToken payload))
            {
                _notyf.Error("Invalid token");
                HttpContext.Session.Remove("Glass");
                return RedirectToPage();
            }
            HttpContext.Session.Remove("Glass");
            //Set access token cookie
            SetTokenCookies(tokenVm);
            await UserSignInAsync(payload).ConfigureAwait(false);
            //Set Profile Image to session on login
            var ProfileImage = !string.IsNullOrEmpty(payload.Claims.First(c => c.Type == "img").Value) ? payload.Claims.First(c => c.Type == "img").Value : _config["DefaultUserImage"];
            HttpContext.Session.SetString("ProfileImage", ProfileImage);
            //Check if login for first time with default password
            var chn = payload.Claims.First(c => c.Type == "chn").Value.ToString();
            if (chn != "Y") return LocalRedirect(ReturnUrl);
            HttpContext.Session.SetString("chn", "Y");
            return LocalRedirect(Url.Content("~/"));
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
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURP.AuthToken", tokenVm.AccessToken, cookieOptions);
            var cookieOptions2 = new CookieOptions
            {
                Domain = _config["Domain"],
                Expires = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_config["CookieExpiry2"])),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                IsEssential = true
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(".SURP.RefreshToken", tokenVm.RefreshToken, cookieOptions2);
        }
        private async Task UserSignInAsync(JwtSecurityToken payload)
        {
            var claimsIdentity = new ClaimsIdentity(payload.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt32(_config["CookieExpiry"]))
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties).ConfigureAwait(false);
        }
    }
}
