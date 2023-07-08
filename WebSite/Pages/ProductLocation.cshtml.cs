using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    [Authorize(Roles = "Seller")]
    public class ProductLocationModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public ProductLocationModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }

        //public int? spid { get; set; } //For get seesion product id
        public SelectList State { get; set; }
        public SelectList Country { get; set; }
        public SelectList City { get; set; }
      //  [FromRoute] public int? id { get; set; }
        [FromRoute] public int? ProductId { get; set; }
        [BindProperty] public ProductsLocationsDTO ProductsLocation { get; set; }
        public bool IsNew => ProductsLocation == null;
        public List<ProductsLocationsVM> ModelVms { get; set; }
        public ProductsSpecificationsVM ProductsSpecificationsVM { get; set; }
        public ProductsDescriptionsVM ProductsDescriptionsVM { get; set; }
        public List<ProductsFileUploadsVM> ProductsFileUploadsVM { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //spid = HttpContext.Session.GetInt32("ProductId");
            //bind country
            var CountryList = await _httpClient.GetAsync("Countries/GetDropdown", false);
            var country = !string.IsNullOrEmpty(CountryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(CountryList) : null;
            if (country != null)
            {
                Country = new SelectList(country, "Id", "Text");
            }
            //get data from productid
            if (ProductId != null)
            {
                var productlocationdetails = await _httpClient.GetAsync("ProductsLocations/GetByProductId", false, ProductId).ConfigureAwait(false);
                ProductsLocation = !string.IsNullOrEmpty(productlocationdetails) ? JsonConvert.DeserializeObject<ProductsLocationsDTO>(productlocationdetails) : null;
                if (ProductsLocation != null)
                {
                    //get state
                    var StateResult = await _httpClient.GetAsync("States/GetState", false, ProductsLocation.CountryId);
                    if (StateResult == "unauthorized") return null;
                    var states = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateResult) : null;
                    State = new SelectList(states, "Id", "StateName");

                    //get City
                    var CityResult = await _httpClient.GetAsync("Cities/GetCitybystate", false, ProductsLocation.StateId);
                    if (CityResult == "unauthorized") return null;
                    var Citys = !string.IsNullOrEmpty(CityResult) ? JsonConvert.DeserializeObject<List<CitiesVM>>(CityResult) : null;
                    City = new SelectList(Citys, "Id", "CityName");
                }
                

                // Get Descrition
                var productdescrition = await _httpClient.GetAsync("ProductsDescriptions/GetByProductId", false, ProductId);
                //if (getsubcategory == "unauthorized") return null;
                ProductsDescriptionsVM = !string.IsNullOrEmpty(productdescrition) ? JsonConvert.DeserializeObject<ProductsDescriptionsVM>(productdescrition) : null;

                // Get ProductFile
                var ProductFile = await _httpClient.GetAsync("ProductsFileUploads/GetProductsFileUploadsByProductId", false, ProductId);
                //if (getsubcategory == "unauthorized") return null;
                ProductsFileUploadsVM = !string.IsNullOrEmpty(ProductFile) ? JsonConvert.DeserializeObject<List<ProductsFileUploadsVM>>(ProductFile) : null;
            }
            return Page();
        }
        //bind state by country
        public async Task<IActionResult> OnGetGetStatebyCountryid(int countryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("States/GetState", false, countryid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateResult) : null;
            return new JsonResult(stid);
        }
        //bind city by state
        public async Task<IActionResult> OnGetGetCitybystateid(int stateid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("Cities/GetCitybystate", false, stateid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<CitiesVM>>(StateResult) : null;
            return new JsonResult(stid);
        }

        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (ProductsLocation.Id == 0 || IsNew)
            {
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                // ProductsLocation.ProductId = Convert.ToInt32(HttpContext.Session.GetInt32("ProductId"));  //geting product id from session
                ProductsLocation.ProductId = (int)ProductId;
                response = await _httpClient.PostAsync("ProductsLocations/Create", false, ProductsLocation).ConfigureAwait(false);
            }
            else
            {
                ProductsLocation.ProductId = (int)ProductId; //geting product id from session
                response = await _httpClient.PutAsync("ProductsLocations/Edit", false, ProductsLocation.Id, ProductsLocation).ConfigureAwait(false);
            }
            //if (response == "unauthorized")
            //{
            //    _notyf.Information("Please login/register");
            //    return RedirectToPage("/Account/Login");
            //}
            ProductsLocation = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<ProductsLocationsDTO>(response) : null;
            if (ProductsLocation == null)
                _notyf.Error("Save failed");
                //TempData["Message1"] = "Save failed";
            else
                _notyf.Success("Saved successfully");
               // TempData["Message1"] = "Save successfully";
            //return RedirectToPage("ProductLocation");
            //   return RedirectToPage("ProductDescription");
            return RedirectToPage("ProductDescription", new { ProductId });
        }
    }
}
