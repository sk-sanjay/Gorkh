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
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public IndexModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public List<CategoriesVM> ModelVms { get; set; }
        public List<Products1VM> Modelvfm { get; set; }
        public List<Products1VM> modellatestproduct { get; set; }
        //  public List<Products1VM> modelcatimg { get; set; }
        public List<CategoryimgCommonVM> modelcatimg { get; set; }

        public List<OurCustomersVM> OurCustomers { get; set; }
        public List<TestimonialsVM> Testimonials { get; set; }
        public List<BuyerRequirementsVM1> BuyerRequirements { get; set; }
        public BuyerRequirementsVM1 BuyerRequirements1 { get; set; }
        public string role => DataHelper.GetUserRole(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        [FromRoute] public int? id { get; set; }
        [BindProperty] public ProductsBuyerFavoritesDTO ProductsBuyerFavorites { get; set; }

        public List<BannersVM> ModelBannerVms { get; set; }
        public List<SubSubCategoriesVM> SubSubCategory { get; set; }
        public ProductsDetailsVM ProductsDetail { get; set; }
        //public int? categoryid { get; set; }
        //public int? sub_categoryid { get; set; }
        //public string keyword { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
           

            //Get Products as Featured Machine
            var modelfm = await _httpClient.GetAsync("Products/GetProductsasfeaturedmachine", false).ConfigureAwait(false);
            Modelvfm = !string.IsNullOrEmpty(modelfm) ? JsonConvert.DeserializeObject<List<Products1VM>>(modelfm) : null;
            if (Modelvfm == null || Modelvfm.Count <= 0)
                _notyf.Error("Data not found");
            //Get Latest Products
            var latestproduct = await _httpClient.GetAsync("Products/GetLatestProducts", false).ConfigureAwait(false);
            modellatestproduct = !string.IsNullOrEmpty(latestproduct) ? JsonConvert.DeserializeObject<List<Products1VM>>(latestproduct) : null;
            if (modellatestproduct == null || modellatestproduct.Count <= 0)
                _notyf.Error("Data not found");
            

            var modelcatimage = await _httpClient.GetAsync("Products/GetCategoryImage", false).ConfigureAwait(false);
            modelcatimg = !string.IsNullOrEmpty(modelcatimage) ? JsonConvert.DeserializeObject<List<CategoryimgCommonVM>>(modelcatimage) : null;
            if (modelcatimg == null || modelcatimg.Count <= 0)
                _notyf.Error("Data not found");
            // our happy customers
            var OurCustomersmodel = await _httpClient.GetAsync("OurCustomers/GetOurCustomersHomePage", false).ConfigureAwait(false);
            OurCustomers = !string.IsNullOrEmpty(OurCustomersmodel) ? JsonConvert.DeserializeObject<List<OurCustomersVM>>(OurCustomersmodel) : null;
            if (OurCustomers == null || OurCustomers.Count <= 0)
                _notyf.Error("Data not found");

            // Get Testimonials
            var Testimonialsmodel = await _httpClient.GetAsync("Testimonials/GetTestimonials", false).ConfigureAwait(false);
            Testimonials = !string.IsNullOrEmpty(Testimonialsmodel) ? JsonConvert.DeserializeObject<List<TestimonialsVM>>(Testimonialsmodel) : null;
            if (Testimonials == null || Testimonials.Count <= 0)
                _notyf.Error("Data not found");


            var modelResponse = await _httpClient.GetAsync("Categories/GetAllCategoryWithChild", false).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<CategoriesVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");

            var modelBanners = await _httpClient.GetAsync("Banners/GetBannersForHomeSlider", false).ConfigureAwait(false);
            if (modelBanners == "unauthorized")
            {
                return null;
            }
            ModelBannerVms = !string.IsNullOrEmpty(modelBanners) ? JsonConvert.DeserializeObject<List<BannersVM>>(modelBanners) : null;
            if (ModelBannerVms == null || ModelBannerVms.Count <= 0)
                _notyf.Error("Banner not found");

            var modelBR = await _httpClient.GetAsync("BuyerRequirements/GetBuyerRequirementsforWebsite", false).ConfigureAwait(false);
            BuyerRequirements = !string.IsNullOrEmpty(modelBR) ? JsonConvert.DeserializeObject<List<BuyerRequirementsVM1>>(modelBR) : null;
            if (BuyerRequirements == null || BuyerRequirements.Count <= 0)
                _notyf.Error("Data not found");

            return Page();
        }

        public async Task<JsonResult> OnPostFavorites(int id)
        {
            var model = new ProductsBuyerFavoritesDTO();
            model.ProductId = id;
            if (role == "Buyer" && BuyerId != "0")
            {
                model.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                model.CreatedDate = DateTime.Now;
                var result = await _httpClient.PostAsync("ProductsBuyerFavorites/Create", true, model).ConfigureAwait(false);
                ProductsBuyerFavorites = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ProductsBuyerFavoritesDTO>(result) : null;

                if (ProductsBuyerFavorites == null)
                    //_notyf.Warning("You have already mark this product as Favorites.");
                    // return new JsonResult(ProductsBuyerFavorites);
                    return new JsonResult("You have already mark this product as Favorites.");
                else
                    // _notyf.Success("You have  mark this product as Favorites");
                    return new JsonResult("You have  mark this product as Favorites");
            }
            else
               // _notyf.Error("Please Login as a buyer to mark this product as Favorites.!");
            return new JsonResult("Please Login as a buyer to mark this product as Favorites.!");
            //return RedirectToPage("Index");

        }
        public async Task<JsonResult> OnPostAutoComplete(string prefix)
        {
            var response = await _httpClient.GetAsync("SubSubCategories/SearchSubSubCategory", false, prefix).ConfigureAwait(false);
            SubSubCategory = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<List<SubSubCategoriesVM>>(response) : null;
            //if (SubSubCategory == null)
            //    _notyf.Error("Data not found");
            return new JsonResult(SubSubCategory);
        }
        //public async Task<PartialViewResult> OnGetBuyerRequiremenDataAsync(int id)
        //{
        //    var modelResponse = await _httpClient.GetAsync("BuyerRequirements/GetBuyerRequirements", false, id).ConfigureAwait(false);
        //    BuyerRequirements1 = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<BuyerRequirementsVM1>(modelResponse) : null;

        //    return Partial("_BuyerRequirmentPartial", BuyerRequirements1);
        //}
        public async Task<PartialViewResult> OnGetProductAsync(int id)
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, id).ConfigureAwait(false);
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            return Partial("_ProductsPartial", ProductsDetail);
        }

        public async Task<JsonResult> OnGetBuyerRequiremenDataAsync(int id)
        {
            var modelResponse = await _httpClient.GetAsync("BuyerRequirements/GetBuyerRequirements", false, id).ConfigureAwait(false);
            BuyerRequirements1 = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<BuyerRequirementsVM1>(modelResponse) : null;

            return new JsonResult(BuyerRequirements1);
        }




    }
}
