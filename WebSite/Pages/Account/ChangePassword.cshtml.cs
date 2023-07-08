using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace WebSite.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;

        public ChangePasswordModel(
           IHttpClientService httpClient,
           INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public string ReturnUrl { get; set; }
        [BindProperty] public ChangePasswordDTO Input { get; set; }
        public void OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("/Seller-Dashboard");
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("/Seller-Dashboard");
            if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
            var result = await _httpClient.PostAsync("Auth/ChangePassword", true, Input).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result)) return Page();
            if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");
            _notyf.Information(result);
            HttpContext.Session.SetString("chn", "N");
            return LocalRedirect("/Seller-Dashboard");
        }
    }
}
