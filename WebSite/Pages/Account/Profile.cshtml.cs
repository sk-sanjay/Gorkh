using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;
namespace WebSite.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProfileModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public string uid => DataHelper.GetUserId(User);
        public string role => DataHelper.GetUserRole(User);

        public string SelleId => DataHelper.GetSellerId(User);
        [BindProperty] public UserProfileDTO UserProfileDto { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }

        public async Task<IActionResult> OnGet()

        {      var result = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbySellerId", true, SelleId).ConfigureAwait(false);
                if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");
                BuyerSellerdto = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result) : null;
                return Page();

          
        }
    }
}
