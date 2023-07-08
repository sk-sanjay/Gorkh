using Application.Dtos;
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
    [Authorize(Roles = "Admin")]
    public class ProductsBuyerQueriesModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        public ProductsBuyerQueriesModel(
            IHttpClientService httpClient,
            INotyfService notyf, IFileService fileService, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _fileService = fileService;
            _emailService = emailService;
        }
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? pid { get; set; } //Product Id
        [BindProperty] public ProductsBuyerQueriesDTO ProductsBuyerQuery { get; set; }
        public bool IsNew => ProductsBuyerQuery == null;
        //public List<ProductsBuyerQueriesVM> ModelVms { get; set; }
        public List<ProductsBuyerQueriesCommonVM> ModelVms { get; set; }
        public ProductsBuyerQueriesCommonVM ModelVms1 { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var modelResponse = await _httpClient.GetAsync("ProductsBuyerQueries/Get", true, (int)id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ProductsBuyerQuery = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsBuyerQueriesDTO>(modelResponse) : null;
            ProductsBuyerQuery.Descriptions = "";
            //return Page();

            //Get Product and Buyer Details
            var modelRes = await _httpClient.GetAsync("ProductsBuyerQueries/GetProductsBuyerQueriesById", true, (int)id).ConfigureAwait(false);
            if (modelRes == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms1 = !string.IsNullOrEmpty(modelRes) ? JsonConvert.DeserializeObject<ProductsBuyerQueriesCommonVM>(modelRes) : null;

            //Get Buyer Query by Product Id & Buyer Id wise
            int productid= Convert.ToInt32(ProductsBuyerQuery.ProductId);
            int buyerid = Convert.ToInt32(ProductsBuyerQuery.BuyerId);
            var modelResponse1 = await _httpClient.GetAsync("ProductsBuyerQueries/GetProductsBuyerQueriesByPid", true, (int)productid, buyerid).ConfigureAwait(false);
            if (modelResponse1 == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse1) ? JsonConvert.DeserializeObject<List<ProductsBuyerQueriesCommonVM>>(modelResponse1) : null;
            //if (ModelVms == null || ModelVms.Count <= 0)
            //    _notyf.Error("Data not found");
            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (id != null)
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
                //ProductsBuyerQuery.ProductId = (int)pid;
                ProductsBuyerQuery.Id = 0;
                ProductsBuyerQuery.CreatedDate = DateTime.Now;
                ProductsBuyerQuery.CreatedBy = "Admin";
                //ProductsBuyerQuery.BuyerId = Convert.ToInt32(BuyerId);
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
