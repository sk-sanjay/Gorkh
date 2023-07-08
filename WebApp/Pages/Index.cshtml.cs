using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IConfiguration _config;
        private readonly INotyfService _notyf;
        public IndexModel(
            IHttpClientService httpClient,
            IConfiguration config,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _config = config;
            _notyf = notyf;
        }
        public bool ShowChangePasswordModal => HttpContext.Session.GetString("chn") == "Y";
        public DashboardAlertsVM DashboardAlertVm { get; set; }
        public List<BuyerSellerCountVM> BuyerSellerCountVM { get; set; }
        public List<ProductCountVM> ProductCountVM { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //Set Profile Image to session on login
            var ImageName = DataHelper.GetUserProfileImage(User);
            var ProfileImage = !string.IsNullOrEmpty(ImageName) ? ImageName : _config["DefaultUserImage"];
            HttpContext.Session.SetString("ProfileImage", ProfileImage);
            var alertResponse = await _httpClient.GetAsync("DashboardAlerts/GetActiveAlert", false).ConfigureAwait(false);
            DashboardAlertVm = !string.IsNullOrEmpty(alertResponse) ? JsonConvert.DeserializeObject<DashboardAlertsVM>(alertResponse) : null;
            return Page();
        }
        public async Task<IActionResult> OnGetSetPasswordStatus()
        {
            var uid = DataHelper.GetUserId(User);
            var result = await _httpClient.GetAsync("Users/UpdatePasswordStatus", true, uid, 0).ConfigureAwait(false);
            if (result == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            var success = !string.IsNullOrEmpty(result) && Convert.ToBoolean(result);
            if (!success)
                _notyf.Error("Password status couldn't be updated");
            HttpContext.Session.SetString("chn", "N");
            return Page();
        }
        public IActionResult OnGetChangePasswordLater()
        {
            HttpContext.Session.SetString("chn", "N");
            return Page();
        }

        public async Task<IActionResult> OnGetBuyerandSellerCount()
        {
            //Get Buyer Seller Count
            var modelBandSCount = await _httpClient.GetAsync("Users/GetBuyerandSellerCount", true).ConfigureAwait(false);
            BuyerSellerCountVM = !string.IsNullOrEmpty(modelBandSCount) ? JsonConvert.DeserializeObject<List<BuyerSellerCountVM>>(modelBandSCount) : null;
           
            if (BuyerSellerCountVM == null)
                _notyf.Error("Data not found");
            return new JsonResult(BuyerSellerCountVM);
            //return Page();
        }

        public async Task<IActionResult> OnGetProductCount()
        {
            //Get Buyer Seller Count
            var modelproductCount = await _httpClient.GetAsync("Users/GetProductCount", true).ConfigureAwait(false);
            ProductCountVM = !string.IsNullOrEmpty(modelproductCount) ? JsonConvert.DeserializeObject<List<ProductCountVM>>(modelproductCount) : null;

            if (ProductCountVM == null)
                _notyf.Error("Data not found");
            return new JsonResult(ProductCountVM);
         
        }
        public async Task<IActionResult> OnGetProductCountbyCategory()
        {
            //Get Buyer Seller Count
            var modelproductCount = await _httpClient.GetAsync("Users/GetProductCountbyCategory", true).ConfigureAwait(false);
            ProductCountVM = !string.IsNullOrEmpty(modelproductCount) ? JsonConvert.DeserializeObject<List<ProductCountVM>>(modelproductCount) : null;

            if (ProductCountVM == null)
                _notyf.Error("Data not found");
            var ApprovePendingVm = new ApprovePendingVM
            {
                ApprovedCount = ProductCountVM.Select(x => x.Approve).ToList(),
                PendingCount = ProductCountVM.Select(x => x.Pending).ToList()
            };
            return new JsonResult(ApprovePendingVm);
            //return Page();
        }

        

    }
}
