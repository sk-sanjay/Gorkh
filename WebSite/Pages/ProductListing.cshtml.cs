using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages
{
    [Authorize(Roles = "Seller")]
    [IgnoreAntiforgeryToken]
    public class ProductListingModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public ProductListingModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        public string SellerId => DataHelper.GetSellerId(User);
        public SelectList Category { get; set; }
        public SelectList SubCategory { get; set; }
        public SelectList SubSubCategory { get; set; }
        public SelectList State { get; set; }
        public SelectList Country { get; set; }
        public SelectList Condition { get; set; }
        public SelectList Manufacturer { get; set; }
        public SelectList YearofProcs { get; set; }

        [FromRoute] public int? id { get; set; }
        [BindProperty] public ProductsDTO Product { get; set; }
        public bool IsNew => Product == null;
        public List<ProductsVM> ModelVms { get; set; }
        public ProductsLocationsVM ProductsLocationsVM { get; set; }
        public ProductsSpecificationsVM ProductsSpecificationsVM { get; set; }
        public ProductsDescriptionsVM ProductsDescriptionsVM { get; set; }
        public List<ProductsFileUploadsVM> ProductsFileUploadsVM { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            //bind category
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (category != null)
            {
                Category = new SelectList(category, "Id", "Text");
            }
            //bind country
            var CountryList = await _httpClient.GetAsync("Countries/GetDropdown", false);
            var country = !string.IsNullOrEmpty(CountryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(CountryList) : null;
            if (country != null)
            {
                Country = new SelectList(country, "Id", "Text");
            }
            //bind conditions
            var ConditionList = await _httpClient.GetAsync("Conditions/GetDropdown", false);
            var condition = !string.IsNullOrEmpty(ConditionList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(ConditionList) : null;
            if (condition != null)
            {
                Condition = new SelectList(condition, "Id", "Text");
            }
            if (id != null)
            {
                int productid = Convert.ToInt32(id);
                var modelResponse = await _httpClient.GetAsync("Products/Get", false, productid).ConfigureAwait(false);
                Product = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDTO>(modelResponse) : null;
                if (Product != null)
                {
                    var getcategory = "";
                    getcategory = await _httpClient.GetAsync("SubCategories/GetSubcategory", false, Product.CategoryId);
                    if (getcategory == "unauthorized") return null;
                    var catid = !string.IsNullOrEmpty(getcategory) ? JsonConvert.DeserializeObject<List<SubCategoriesVM>>(getcategory) : null;
                    //  SubCategory = new SelectList(catid, "Id", "subCategoryName");
                    // ViewData["SubCategory"] = new SelectList(catid, "Id", "SubCategoryName");
                    SubCategory = new SelectList(catid, "Id", "SubCategoryName");

                    var getsubcategory = "";
                    getsubcategory = await _httpClient.GetAsync("SubSubCategories/GetSubSubCategoryBySubCategory2", false, Product.SubCategoryId);
                    if (getsubcategory == "unauthorized") return null;
                    var subcatid = !string.IsNullOrEmpty(getsubcategory) ? JsonConvert.DeserializeObject<List<SubSubCategoriesVM>>(getsubcategory) : null;

                    // ViewData["SubCategory"] = new SelectList(catid, "Id", "SubCategoryName");
                    SubSubCategory = new SelectList(subcatid, "Id", "SubSubCategoriesName");
                    //
                    var currentYear = DateTime.Now.Year;
                    List<int> YearofProcs11 = new List<int>();
                    for (var i = 1950; i <= currentYear; i++)
                    {
                        //DropDownList1.Items.Add(i.ToString());
                        // YearofProcs.Items.(i);
                        YearofProcs11.Add(i);
                    }
                    if (YearofProcs11 != null)
                    {
                        YearofProcs = new SelectList(YearofProcs11);
                        // ViewData["YearofProcs"] = new SelectList(YearofProcs11);
                    }
                    else
                    {
                        YearofProcs = null;
                    }
                }
                // Get Locations
                var productlocation = await _httpClient.GetAsync("ProductsLocations/GetByProductId", false, id);
                //if (getsubcategory == "unauthorized") return null;
                ProductsLocationsVM = !string.IsNullOrEmpty(productlocation) ? JsonConvert.DeserializeObject<ProductsLocationsVM>(productlocation) : null;
                
                // Get Descrition
                var productdescrition = await _httpClient.GetAsync("ProductsDescriptions/GetByProductId", false, id);
                //if (getsubcategory == "unauthorized") return null;
                ProductsDescriptionsVM = !string.IsNullOrEmpty(productdescrition) ? JsonConvert.DeserializeObject<ProductsDescriptionsVM>(productdescrition) : null;

                // Get ProductFile
                var ProductFile = await _httpClient.GetAsync("ProductsFileUploads/GetProductsFileUploadsByProductId", false, id);
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
        //bind sub category by category
        public async Task<IActionResult> OnGetGetSubCategory(int maincat)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SubCategories/GetSubcategory", false, maincat);
            if (StateResult == "unauthorized") return null;
            var catid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SubCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}
            return new JsonResult(catid);
        }
        //bind sub sub category by sub category
        public async Task<IActionResult> OnGetGetSubSubCategoryBySubCategory2(int subcategoryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SubSubCategories/GetSubSubCategoryBySubCategory2", false, subcategoryid);
            if (StateResult == "unauthorized") return null;
            var catid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SubSubCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}
            return new JsonResult(catid);
        }

        //get specifications mapping data
        public async Task<IActionResult> OnGetGetSpecificationsSSCategoriesjoin(int subsubcategoryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SpecificationsSSCategories/GetSpecificationsSSCategoriesjoin", false, subsubcategoryid);
            if (StateResult == "unauthorized") return null;
            var sdata = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SpecificationsSSCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}

            return new JsonResult(sdata);
        }//get specifications mapping data
        public async Task<IActionResult> OnGetGetSpecificationsSSCategoriesjoin1(int subsubcategoryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SpecificationsSSCategories/GetSpecificationsSSCategoriesjoin", false, subsubcategoryid);
            if (StateResult == "unauthorized") return null;
            var sdata = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SpecificationsSSCategoriesVM>>(StateResult) : null;
            int productid = Convert.ToInt32(id);
            var modelResponse = await _httpClient.GetAsync("Products/Get", false, productid).ConfigureAwait(false);
            Product = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDTO>(modelResponse) : null;
            if (Product != null && Product.ProductsSpecifications.Count > 0)
            {
                for (int i = 0; i < sdata.Count; i++)
                {
                    sdata[i].TextValue = Product.ProductsSpecifications[i].SpecfSSCatField;
                    sdata[i].Productchildid = Product.ProductsSpecifications[i].Id;
                }
            }
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}

            return new JsonResult(sdata);
        }

        public async Task<JsonResult> OnPostManage(string productData)
        {
            var model = JsonConvert.DeserializeObject<ProductsDTO>(productData);
            model.SellerId = Convert.ToInt32(SellerId);
            model.CreatedDate = DateTime.Now;
            model.IsActive = true; //add custome value
            if (model.Id != 0)
            {
                foreach (var item in model.ProductsSpecifications)
                {
                    //item.Id = 1;
                    item.ProductId = model.Id;
                }
            }

            var result = model.Id == 0
         ? await _httpClient.PostAsync("Products/Create", false, model).ConfigureAwait(false)
         : await _httpClient.PutAsync("Products/Edit", false, model.Id, model).ConfigureAwait(false);
            // var result = await _httpClient.PostAsync("Products/Create", false, model).ConfigureAwait(false);
            if (result == "unauthorized") return new JsonResult("unauthorized");
            Product = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ProductsDTO>(result) : null;
            if (Product == null)
                _notyf.Error("Save Failed");
            //  TempData["Message"] = "Save Failed";
            else
                _notyf.Success("Saved Succesfully");
            // TempData["Message"] = "Save Succesfully";
            if (Product != null)
                HttpContext.Session.SetInt32("ProductId", Product.Id); //Session used for send product id to next page

            return new JsonResult(Product);
           // return RedirectToPage("ProductLocation", new { Product.Id });
        }

        //get manufacturers data
        public async Task<IActionResult> OnGetGetDropdown()
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("Manufacturers/GetDropdown", false);
            if (StateResult == "unauthorized") return null;
            var sdata = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<DropdownVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}

            return new JsonResult(sdata);
        }
    }
}
