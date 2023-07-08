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
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public ProductsModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        public string SellerId => DataHelper.GetSellerId(User);
        //public List<ProductsVM> ModelVms { get; set; }
        public List<ProductsBySellerVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            int sellerid = Convert.ToInt32(SellerId);
            var modelResponse = await _httpClient.GetAsync("Products/GetProductsBySeller", false, sellerid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBySellerVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("Products/Delete", false, id).ConfigureAwait(false);
            if (DeleteResult == "unauthorized") return new JsonResult("unauthorized");
            var RowsChanged = !string.IsNullOrEmpty(DeleteResult) && Convert.ToInt32(DeleteResult) > 0;
           // return RowsChanged ? new JsonResult("success") : new JsonResult("fail");
            if (RowsChanged)
            {
                _notyf.Success("success");
                return new JsonResult("success");
            }
            _notyf.Error("fail");
            return new JsonResult("fail");
        }
    }
}
