using Application.Dtos;
using Application.ServiceInterfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages
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
        public string BuyerId => DataHelper.GetBuyerId(User);



        [BindProperty] public UserProfileDTO UserProfileDto { get; set; }
        [BindProperty] public SellerRegistrationsDTO SellerDto { get; set; }
        [BindProperty] public BuyersDTO BuyerDto { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }

        public async Task<IActionResult> OnGet()

        {

            if (SelleId == "0")
            {
                var result = await _httpClient.GetAsync("BuyerSellerRegistrations/GetdropdownbySellerId", true, BuyerId).ConfigureAwait(false);
                if (result == "unauthorized") return RedirectToPage("/account/seller-login");
                BuyerSellerdto = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result) : null;
                if (BuyerSellerdto != null) return Page();

                _notyf.Error("User not found");
                return RedirectToPage("/Index");
            }
            else
            {
                var result1 = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbySellerId", true, SelleId).ConfigureAwait(false);
                if (result1 == "unauthorized") return RedirectToPage("/Account/Seller-Login");
                BuyerSellerdto = !string.IsNullOrEmpty(result1) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result1) : null;
                if (BuyerSellerdto != null) return Page();

                _notyf.Error("User not found");
                return RedirectToPage("/Index");
            }
        }
    }
}
