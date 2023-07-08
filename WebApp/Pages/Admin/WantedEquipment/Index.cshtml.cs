using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
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
using WebApp.Helpers;

namespace WebApp.Pages.Admin.WantedEquipment
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;

        public IndexModel(
           IHttpClientService httpClient,
            INotyfService notyf
            )
        {
            _httpClient = httpClient;
            _notyf = notyf;

        }
        public string uid => DataHelper.GetUserId(User);
        //public List<BuyerSellerRegistrations1VM> ModelVms { get; set; }
        public List<BuyerRequirementsVM1> ModelVms { get; set; }
        
        [BindProperty] public BuyerSellerRegistrationsDTO ModelDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("BuyerRequirements/GetBuyerRequirements", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<BuyerRequirementsVM1>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
       
    }
}

