using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages.Admin.RegisteredBuyersandSellers
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        
        public DetailsModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            
        }

        [FromRoute] public string? email { get; set; }
        public BuyerSellerRegistrationsVM BuyerSellerRegistrations { get; set; }
        //[BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyEmail", false,email).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            BuyerSellerRegistrations = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(modelResponse) : null;
            if (BuyerSellerRegistrations == null)
            {
                _notyf.Error("Data not found");
            }



            return Page();
        }

    }
}
