using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages.Account
{
    [Authorize]
    public class SellerProfileModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public SellerProfileModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public string uid => DataHelper.GetUserId(User);
        public string usename => DataHelper.GetUserName(User);
        public string SellerId => DataHelper.GetSellerId(User);
        public SelectList Category { get; set; }
        public SelectList Organisation { get; set; }
        public SelectList Country { get; set; }
        public SelectList State { get; set; }
        public SelectList City { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        [BindProperty] public BuyerUpdateDTO BuyerUpdateDTO { get; set; }
        public async Task<IActionResult> OnGetAsync()

        {
            // Bind Seller Details for updation
            var result = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbySellerId", true, SellerId).ConfigureAwait(false);
            if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");
            BuyerSellerdto = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result) : null;
            BuyerSellerdto.VeryfyEmail = BuyerSellerdto.Email;

            // Bind Categories
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (category != null)
            {
                Category = new SelectList(category, "Id", "Text");
            }
            ViewData["Category"] = new SelectList(category, "Id", "Text");

            // Bind OrganisationTypes
            var OrganisationTypesList = await _httpClient.GetAsync("OrganisationTypes/GetDropdown", false);
            var OrganisationTypes = !string.IsNullOrEmpty(OrganisationTypesList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(OrganisationTypesList) : null;
            if (OrganisationTypes != null)
            {
                Organisation = new SelectList(OrganisationTypes, "Id", "Text");
            }
            ViewData["OrganisationType"] = new SelectList(OrganisationTypes, "Id", "Text");

            // Bind Cuntry
            var CountryList = await _httpClient.GetAsync("Countries/GetDropdown", false);

            var country = !string.IsNullOrEmpty(CountryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(CountryList) : null;
            if (country != null)
            {

                Country = new SelectList(country, "Id", "Text");
            }
            ViewData["Countries"] = new SelectList(country, "Id", "Text");

            // Bind State by countryid
            var StateList = await _httpClient.GetAsync("States/GetDropdownByCountry", false, BuyerSellerdto.Country);
            var state = !string.IsNullOrEmpty(StateList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(StateList) : null;
            if (state != null)
            {
                State = new SelectList(state, "Id", "Text");
            }
            ViewData["States"] = new SelectList(state, "Id", "Text");

            // Bind City by stateid
            var Citylist = await _httpClient.GetAsync("Cities/GetCitybystate", false, BuyerSellerdto.State);
            if (Citylist == "unauthorized") return null;
            var cities = !string.IsNullOrEmpty(Citylist) ? JsonConvert.DeserializeObject<List<CitiesVM>>(Citylist) : null;
            if (cities != null)
            {
                City = new SelectList(cities, "Id", "CityName");
            }
            ViewData["City"] = new SelectList(cities, "Id", "CityName");
            return Page();
        }

        public async Task<IActionResult> OnGetGetStatebyCountryid(int countryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("States/GetState", false, countryid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateResult) : null;
            return new JsonResult(stid);
        }

        // Bind City by stateid
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
            {
                BuyerSellerdto = ModelAuditor<BuyerSellerRegistrationsDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), BuyerSellerdto);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PutAsync("BuyerSellerRegistrations/UpdateSellerProfile", true, BuyerSellerdto.Id, BuyerSellerdto).ConfigureAwait(false);
                BuyerSellerdto.ModifiedDate = System.DateTime.Now;
            }
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            BuyerSellerdto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(response) : null;
            CommonRegisterDTO ComReg = new CommonRegisterDTO();
            ComReg.Emailid = usename;
            ComReg.Name = BuyerSellerdto.FirstName;
            //Input.BuyerId = BuyerSellerdto.Id;
            response = await _httpClient.PutAsync("Auth/UpdateBuyerName", true, ComReg.Emailid, ComReg).ConfigureAwait(false);
            if (BuyerSellerdto == null)
                _notyf.Error("Save Failed");
            else
                _notyf.Success("Profile updated successfully.");
            return RedirectToPage("/Account/SellerProfile");
        }
    }
}
