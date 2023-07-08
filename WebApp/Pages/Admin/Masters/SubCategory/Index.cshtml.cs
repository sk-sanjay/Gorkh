using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Masters.SubCategory
{
    [Authorize(Roles = "SuperAdmin")]
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
        public List<SubCategoriesVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("SubCategories/Get", true).ConfigureAwait(false);
            //var modelResponse = await _httpClient.GetAsync("SubCategories/GetCategory", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<SubCategoriesVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }


    }
}
