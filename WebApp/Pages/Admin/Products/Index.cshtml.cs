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
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public IndexModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public List<ProductsVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("Products/GetAllProductsForAdmin", false).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
        public async Task<IActionResult> OnGetUpdateSelected(string modelStr)
        {
            //var model = JsonConvert.DeserializeObject<ProductsDTO>(modelStr);
            var Result = await _httpClient.GetAsync("Products/UpdateSelected", true, modelStr).ConfigureAwait(false);
            if (string.IsNullOrEmpty(Result)) return new JsonResult("Product(s) couldn't be update");
            if (Result == "unauthorized") return new JsonResult(Result);
            return Convert.ToInt32(Result) > 0 ? new JsonResult("Product(s) update successfully") : new JsonResult("Product(s) couldn't be updated");
        }

        public async Task<IActionResult> OnGetUpdateasFeatured(string modelstr)
        {
            
            var Result = await _httpClient.GetAsync("Products/UpdateasFeatured", true, modelstr).ConfigureAwait(false);
            if (string.IsNullOrEmpty(Result)) return new JsonResult("Product(s) couldn't be update");
            if (Result == "unauthorized") return new JsonResult(Result);
            return Convert.ToInt32(Result) > 0 ? new JsonResult("Product(s) update successfully") : new JsonResult("Product(s) couldn't be updated");
        }

        
    }
}
