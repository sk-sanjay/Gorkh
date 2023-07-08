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

namespace WebSite.Pages
{
    public class ProductEnquiryModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductEnquiryModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? pid { get; set; }
        [BindProperty] public ProductsEnquiriesDTO ProductsEnquiry { get; set; }
        public bool IsNew => ProductsEnquiry == null;
        public List<ProductsEnquiriesVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var modelResponse = await _httpClient.GetAsync("ProductsEnquiries/GetProductsEnquiriesByPid", true, (int)pid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsEnquiriesVM>>(modelResponse) : null;
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

                ProductsEnquiry.EnquiryFile = SavedFileName;
                ProductsEnquiry.ProductId = (int)pid;
                ProductsEnquiry.CreatedDate = DateTime.Now;
                ProductsEnquiry.CreatedBy = "Seller";
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PostAsync("ProductsEnquiries/Create", true, ProductsEnquiry).ConfigureAwait(false);
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
            ProductsEnquiry = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<ProductsEnquiriesDTO>(response) : null;
            if (ProductsEnquiry == null)
                _notyf.Error("Data already exists");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("ProductEnquiry");
        }
    }
}
