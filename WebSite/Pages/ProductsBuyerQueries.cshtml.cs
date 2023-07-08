using Application.Dtos;
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
    public class ProductsBuyerQueriesModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductsBuyerQueriesModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public string role => DataHelper.GetUserRole(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? pid { get; set; }
        [BindProperty] public ProductsBuyerQueriesDTO ProductsBuyerQuery { get; set; }
        public bool IsNew => ProductsBuyerQuery == null;
        public List<ProductsBuyerQueriesCommonVM> ModelVms { get; set; }
        public ProductsBuyerQueriesCommonVM ModelVms1 { get; set; }
        public async Task<IActionResult> OnGet()
        {
            //Get Product Details
            var modelRes = await _httpClient.GetAsync("ProductsBuyerQueries/GetProductsDetailsById", true, (int)pid).ConfigureAwait(false);
            if (modelRes == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms1 = !string.IsNullOrEmpty(modelRes) ? JsonConvert.DeserializeObject<ProductsBuyerQueriesCommonVM>(modelRes) : null;


            int buyerid = Convert.ToInt32(BuyerId);
            var modelResponse = await _httpClient.GetAsync("ProductsBuyerQueries/GetProductsBuyerQueriesByPid", true, (int)pid, buyerid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBuyerQueriesCommonVM>>(modelResponse) : null;
            //if (ModelVms == null || ModelVms.Count <= 0)
            //    _notyf.Error("Data not found");
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (id == null)
            {
                var FileUploadDto = new FileUploadDTO
                {
                    ChangeName = true,
                    ReturnValue = "name",
                    UploadedFile = Request.Form.Files.GetFile("UserImage"),
                    FilePath = "\\img\\penquiry",
                    //FileOldName = fileoldname,
                    ChangeDimensions = true,
                    Width = 500,
                    Height = 500
                };
                var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFile", true, FileUploadDto).ConfigureAwait(false);
                if (FileUploadResult == "unauthorized") return new JsonResult("unauthorized");
                var SavedFileName = !string.IsNullOrEmpty(FileUploadResult) ? JsonConvert.DeserializeObject<string>(FileUploadResult) : null;

                ProductsBuyerQuery.EnquiryFile = SavedFileName;
                ProductsBuyerQuery.ProductId = (int)pid;
                ProductsBuyerQuery.CreatedDate = DateTime.Now;
                ProductsBuyerQuery.CreatedBy = role;
                ProductsBuyerQuery.BuyerId = Convert.ToInt32(BuyerId);
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PostAsync("ProductsBuyerQueries/Create", true, ProductsBuyerQuery).ConfigureAwait(false);
            }
            //else
            //{
            //    State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), State);
            //    if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
            //    response = await _httpClient.PutAsync("States/Edit", true, State.Id, State).ConfigureAwait(false);
            //}
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ProductsBuyerQuery = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<ProductsBuyerQueriesDTO>(response) : null;
            if (ProductsBuyerQuery == null)
                _notyf.Error("Data already exists");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("ProductsBuyerQueries");
        }
    }
}
