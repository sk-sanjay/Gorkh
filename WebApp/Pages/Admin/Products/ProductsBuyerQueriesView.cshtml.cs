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
    public class ProductsBuyerQueriesViewModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductsBuyerQueriesViewModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public List<ProductsBuyerQueriesCommonVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("ProductsBuyerQueries/GetProductsBuyerQueriesForAdmin", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBuyerQueriesCommonVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
    }
}
