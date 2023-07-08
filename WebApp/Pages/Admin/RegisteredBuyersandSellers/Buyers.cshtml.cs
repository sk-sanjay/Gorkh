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

namespace WebApp.Pages.Admin.RegisteredBuyersandSellers
{
    [Authorize(Roles = "Admin")]
    public class BuyersModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;

        public BuyersModel(
           IHttpClientService httpClient,
            INotyfService notyf
            )
        {
            _httpClient = httpClient;
            _notyf = notyf;

        }
        public string uid => DataHelper.GetUserId(User);
        public List<BuyerSellerRegistrations1VM> ModelVms { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO ModelDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("BuyerSellerRegistrations/GetBuyersData", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<BuyerSellerRegistrations1VM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
        public async Task<IActionResult> OnGetActiveDeactiveUser(string modelstr)
        {

            var Result = await _httpClient.GetAsync("Users/ActiveDeactiveUser", true, modelstr).ConfigureAwait(false);
            //var Result= await _httpClient.PutAsync("Users/Edit", true, uid,modelstr).ConfigureAwait(false);
            if (string.IsNullOrEmpty(Result)) return new JsonResult("Product(s) couldn't be update");
            if (Result == "unauthorized") return new JsonResult(Result);
            return Convert.ToInt32(Result) > 0 ? new JsonResult("User Update successfully") : new JsonResult("Product(s) couldn't be updated");
        }
    }
    
    
}
