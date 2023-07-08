using Application.Dtos;
using Application.ServiceInterfaces;
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

namespace WebSite.Pages
{
    public class ProductsBySubCatModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ProductsBySubCatModel(IHttpClientService httpClient, INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        //public List<ProductsVM> ModelVms { get; set; }
        public List<Products1VM> ModelVms { get; set; }
        public ProductsDetailsVM ProductsDetail { get; set; }
        public SelectList Category { get; set; }
        public SelectList Country { get; set; }
        public SelectList Condition { get; set; }
        public List<ProductsBreadCrumbsVM> ProductsBreadCrumb { get; set; }
        public int? categoryid { get; set; }
        public int? sub_categoryid { get; set; }
        public string keyword { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //get query string
            categoryid = !string.IsNullOrEmpty(HttpContext.Request.Query["category_id"].ToString()) ? int.Parse(HttpContext.Request.Query["category_id"].ToString()) : null;
            //categoryid = int.Parse(HttpContext.Request.Query["category_id"].ToString());
            sub_categoryid = !string.IsNullOrEmpty(HttpContext.Request.Query["sub_category"].ToString()) ? int.Parse(HttpContext.Request.Query["sub_category"].ToString()) : null;
            keyword = !string.IsNullOrEmpty(HttpContext.Request.Query["keyword"].ToString()) ? HttpContext.Request.Query["keyword"].ToString() : "0";
            //bind category
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (category != null)
            {
                Category = new SelectList(category, "Id", "Text");
            }
            
            var CountryList = await _httpClient.GetAsync("Countries/GetCountryByProdcutWise", false);
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
            
            return Page();
        }
        public async Task<JsonResult> OnGetModel(int id)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            return new JsonResult(ProductsDetail);
        }

        public async Task<PartialViewResult> OnGetProductAsync(int id)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            return Partial("_ProductsPartial", ProductsDetail);
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
        //bind state by country
        public async Task<IActionResult> OnGetGetStatebyCountryid(int countryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("States/GetStatesByProdcutWise", false, countryid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<DropdownVM>>(StateResult) : null;
            //var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            //if (category != null)
            //{
            //    Category = new SelectList(category, "Id", "Text");
            //}
            return new JsonResult(stid);
        }
        //Search data
        public async Task<PartialViewResult> OnGetSearch(int catId, int subcatId, int countryId, int stateId, string saleType, int conditionId, string keyword)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsBySubCatId1", false, catId, subcatId, countryId, stateId, saleType, conditionId, keyword).ConfigureAwait(false);
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<Products1VM>>(modelResponse) : null;
            return Partial("_SearchProductPartial", ModelVms);
        }
        //Bread Crumb data
        public async Task<PartialViewResult> OnGetProductsBreadCrumbs(int catId, int subcatId)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GetProductsBreadCrumbs", false, catId, subcatId).ConfigureAwait(false);
            ProductsBreadCrumb = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsBreadCrumbsVM>>(modelResponse) : null;
            return Partial("_SearchProductBreadCrumbsPartial", ProductsBreadCrumb);
        }

    }
}
