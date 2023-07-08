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
    [Authorize(Roles = "Admin")]
    public class ViewProductDetailsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;

        public ViewProductDetailsModel(
           IHttpClientService httpClient,
           INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public ProductsDetailsVM ProductsDetail { get; set; }
        public BuyerSellerRegistrationsVM SellerDetails { get; set; }
        [FromRoute] public int? id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }

            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            if (ProductsDetail == null)
            {
                _notyf.Error("Data not found");
            }
            else
            {
                var modelResponse1 = await _httpClient.GetAsync("BuyerSellerRegistrations/Get", false, ProductsDetail.SellerId).ConfigureAwait(false);

                SellerDetails = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(modelResponse1) : null;

            }
            
          
         


            return Page();
        }
    }
}
