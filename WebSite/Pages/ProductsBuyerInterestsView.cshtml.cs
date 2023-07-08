using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages
{
    public class ProductsBuyerInterestsViewModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public ProductsBuyerInterestsViewModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        public string role => DataHelper.GetUserRole(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        //public List<ProductsVM> ModelVms { get; set; }
        public List<ProductsBuyerIntrestsCommonVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            int buyerid = Convert.ToInt32(BuyerId);
            var modelResponse = await _httpClient.GetAsync("ProductsBuyerIntrests/GetProductsBuyerIntrestsByBuyer", false, buyerid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBuyerIntrestsCommonVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
    }
}
