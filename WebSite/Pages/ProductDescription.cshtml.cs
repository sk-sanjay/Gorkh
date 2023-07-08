using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace WebSite.Pages
{
    [Authorize(Roles = "Seller")]
    public class ProductDescriptionModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public ProductDescriptionModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? ProductId { get; set; }
        [BindProperty] public ProductsDescriptionsDTO ProductsDescription { get; set; }
        public bool IsNew => ProductsDescription == null;
        public List<ProductsDescriptionsVM> ModelVms { get; set; }
        public ProductsLocationsVM ProductsLocationsVM { get; set; }
        public ProductsDescriptionsVM ProductsDescriptionsVM { get; set; }
        public List<ProductsFileUploadsVM> ProductsFileUploadsVM { get; set; }

        public async Task<IActionResult> OnGet()
        {
            //id=21;
            // if (!id.HasValue) return Page();
            var modelResponse = "";
            if (id != null)
            {
                modelResponse = await _httpClient.GetAsync("ProductsDescriptions/Get", false, (int)id).ConfigureAwait(false);
                if (modelResponse == "unauthorized")
                {
                    //_notyf.Information("Please login/register");
                    return RedirectToPage("/Account/Login");
                }
            }
            if(ProductId != null)
            {
                modelResponse = await _httpClient.GetAsync("ProductsDescriptions/GetByProductId", false, (int)ProductId).ConfigureAwait(false);
                ProductsDescription = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDescriptionsDTO>(modelResponse) : null;

                // Get Locations
                var productlocation = await _httpClient.GetAsync("ProductsLocations/GetByProductId", false, ProductId);
                //if (getsubcategory == "unauthorized") return null;
                ProductsLocationsVM = !string.IsNullOrEmpty(productlocation) ? JsonConvert.DeserializeObject<ProductsLocationsVM>(productlocation) : null;

                
                // Get ProductFile
                var ProductFile = await _httpClient.GetAsync("ProductsFileUploads/GetProductsFileUploadsByProductId", false, ProductId);
                //if (getsubcategory == "unauthorized") return null;
                ProductsFileUploadsVM = !string.IsNullOrEmpty(ProductFile) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsVM>>(ProductFile) : null;
            }
            

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if ((id == null || IsNew) && ProductsDescription.Id == 0)
            {
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }

                //  ProductsDescription.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));  //geting product id from session
                ProductsDescription.ProductId = (int)ProductId;
                //ProductsDescription.ProductId = 61;
                response = await _httpClient.PostAsync("ProductsDescriptions/Create", false, ProductsDescription).ConfigureAwait(false);
            }
            else
            {
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                //ProductsDescription.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));
                ProductsDescription.ProductId = (int)ProductId;
                //ProductsDescription.ProductId = 61;
                response = await _httpClient.PutAsync("ProductsDescriptions/Edit", false, ProductsDescription.Id, ProductsDescription).ConfigureAwait(false);
            }
            if (response == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ProductsDescription = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<ProductsDescriptionsDTO>(response) : null;
            if (ProductsDescription == null)
                _notyf.Error("Save failed");
            else
                _notyf.Success("Saved successfully");
               // TempData["Message2"] = "Save successfully";
            //return RedirectToPage("ProductDescription");
            //return RedirectToPage("ProductFile");
            return RedirectToPage("ProductFile", new { ProductId });
        }
    }
}
