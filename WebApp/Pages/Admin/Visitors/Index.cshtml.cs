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

namespace WebApp.Pages.Admin.Visitors
{
    [Authorize(Roles = "SuperAdmin,Admin")]
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

        public List<VisitorRegistrationsVM> ModelVms { get; set; }
        [BindProperty] public VisitorRegistrationsDTO ModelDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("VisitorRegistrations/Get", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<VisitorRegistrationsVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }

       
      
    }
}
