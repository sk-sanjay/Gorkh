using Application.Dtos;
using Application.ServiceInterfaces;
using Application.Helpers;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace WebSite.Pages
{
    [Authorize(Roles = "Seller")]
    public class ProductPreviewModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductPreviewModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public string role => DataHelper.GetUserRole(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        [FromRoute] public int? ProductId { get; set; }
        public ProductsDetailsVM ProductsDetail { get; set; }

        [BindProperty] public ProductsBuyerIntrestsDTO ProductsBuyerIntrests { get; set; }
        [BindProperty] public PaymentsDTO Payment { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, ProductId).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                return null;
            }
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            if (ProductsDetail == null)
            {
                _notyf.Error("Data not found");
            }
            else
            {
                var model = new ProductsVisitorsDTO();
                model.ProductId = ProductsDetail.Id;
                if (role == "Buyer" && BuyerId != "0")
                {
                    model.UserId = Convert.ToInt32(BuyerId); //it means buyerid
                    model.UserType = "B"; //Buyer
                }
                else
                {
                    model.UserId = 0;
                    model.UserType = "A"; //Anonymus
                }
                model.CreatedDate = DateTime.Now;
               
            }
          
            int buyerid = Convert.ToInt32(BuyerId);
            int productid = Convert.ToInt32(ProductId);
            var modelResponse2 = await _httpClient.GetAsync("Payments/GetProductsPaymentsByBuyerandPid", true, buyerid, productid).ConfigureAwait(false);
            if (modelResponse2 == "unauthorized")
            {
                return null;
            }
            Payment = !string.IsNullOrEmpty(modelResponse2) ? JsonConvert.DeserializeObject<PaymentsDTO>(modelResponse2) : null;
            if (Payment == null)
                return null;
            var modelResponse1 = await _httpClient.GetAsync("ProductsBuyerIntrests/GetProductsBuyerIntrestsByBuyerandPid", true, buyerid, productid).ConfigureAwait(false);
            if (modelResponse1 == "unauthorized")
            {
                return null;
            }
            ProductsBuyerIntrests = !string.IsNullOrEmpty(modelResponse1) ? JsonConvert.DeserializeObject<ProductsBuyerIntrestsDTO>(modelResponse1) : null;
            if (ProductsBuyerIntrests == null)
                return null;
            return Page();
        }
        public async Task<IActionResult> OnGetFinalSubmit(int id)
        {

            var argModelDto = new CommonDTO
            {
                ID = Convert.ToInt32(id),
                Status = "",
                Remarks = ""
            };

            var FinalSubmitResult = await _httpClient.PostAsync("Products/FinalSubmit", false, argModelDto).ConfigureAwait(false);
            if (FinalSubmitResult == "unauthorized")
            {
                return new JsonResult("unauthorized");
            }
            var RowsChanged = !string.IsNullOrEmpty(FinalSubmitResult) && Convert.ToInt32(FinalSubmitResult) > 0;
            if (RowsChanged)
            {
                
                _notyf.Success("Save Succesfully");
                return new JsonResult("success");
            }
            _notyf.Error("Save Failed");
            return new JsonResult("fail");
        }
    }
}
