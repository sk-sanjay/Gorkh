using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Products
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "Admin")]
    public class BuyerOffersModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public BuyerOffersModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        public List<BuyerOffersVM> ModelVms { get; set; }
        [BindProperty] public BuyerOffersDTO BuyerOffersdto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("BuyerOffers/GetBuyerOffersForAdmin", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<BuyerOffersVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateOffer(int id)
        {
            var modelResponse = await _httpClient.PutAsync("BuyerOffers/UpdateBuyerofers", true, id, BuyerOffersdto).ConfigureAwait(false);
           // var modelResponse = await _httpClient.GetAsync("BuyerOffers/UpdateBuyerofers", true,ProductNumber).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            
            return RedirectToPage("BuyerOffers");
        }
    }
}
