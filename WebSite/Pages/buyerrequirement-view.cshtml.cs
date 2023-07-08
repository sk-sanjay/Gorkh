using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Pages
{
        public class buyerrequirement_viewModel : PageModel
        {
            private readonly IHttpClientService _httpClient;
            private readonly INotyfService _notyf;

            public buyerrequirement_viewModel(
               IHttpClientService httpClient,
               INotyfService notyf)
            {
                _httpClient = httpClient;
                _notyf = notyf;
            }
            public BuyerRequirementsVM1 BuyerRequirements { get; set; }
            [FromRoute] public int? id { get; set; }
            public async Task<IActionResult> OnGetAsync()
            {
          
                var modelResponse = await _httpClient.GetAsync("BuyerRequirements/GetBuyerRequirements", false, id).ConfigureAwait(false);
                if (modelResponse == "unauthorized")
                {
                    _notyf.Information("Please login/register");
                    return RedirectToPage("/Account/Login");
                }
                BuyerRequirements = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<BuyerRequirementsVM1>(modelResponse) : null;
                if (BuyerRequirements == null)
                {
                    _notyf.Error("Data not found");
                }



                return Page();
            }
        }
    }
