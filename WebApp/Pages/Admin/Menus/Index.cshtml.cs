using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Menus
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public IndexModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public List<MenuVM> Menus { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var MenusResult = await _httpClient.GetAsync("Menus/GetWithAll", true).ConfigureAwait(false);
            if (MenusResult == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            Menus = !string.IsNullOrEmpty(MenusResult) ? JsonConvert.DeserializeObject<List<MenuVM>>(MenusResult) : null;
            if (Menus == null || Menus.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
    }
}