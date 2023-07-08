using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    public class Product_ListingModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public Product_ListingModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        public SelectList Category { get; set; }
        public SelectList State { get; set; }
        public SelectList Country { get; set; }
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
            StateResult = await _httpClient.GetAsync("SubCategories/GetSubcategory", true, maincat);
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
            StateResult = await _httpClient.GetAsync("SubSubCategories/GetSubSubCategoryBySubCategory2", true, subcategoryid);
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
            StateResult = await _httpClient.GetAsync("SpecificationsSSCategories/GetSpecificationsSSCategoriesjoin", true, subsubcategoryid);
            if (StateResult == "unauthorized") return null;
            var sdata = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SpecificationsSSCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}

            return new JsonResult(sdata);
        }
    }
}
