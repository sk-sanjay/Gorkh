using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class SellerRegistrationModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public SellerRegistrationModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public string uid => DataHelper.GetUserId(User);
        public string role => DataHelper.GetUserRole(User);
        public string SelleId => DataHelper.GetSellerId(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        public SelectList Category { get; set; }
        public SelectList Organisation { get; set; }
        public SelectList State { get; set; }
        public SelectList Country { get; set; }
        [BindProperty] public RegisterDTO Input { get; set; }
        [BindProperty] public SellerRegistrationsDTO Sellerdto { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            //var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            //var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            //if (category != null)
            //{
            //    Category = new SelectList(category, "Id", "Text");
            //}

            //var OrganisationTypesList = await _httpClient.GetAsync("OrganisationTypes/GetDropdown", false);
            //var OrganisationTypes = !string.IsNullOrEmpty(OrganisationTypesList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(OrganisationTypesList) : null;
            //if (OrganisationTypes != null)
            //{
            //    Organisation = new SelectList(OrganisationTypes, "Id", "Text");
            //}

            //var CountryList = await _httpClient.GetAsync("Countries/GetDropdown", false);
            //int? countryId = null;
            //var country = !string.IsNullOrEmpty(CountryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(CountryList) : null;
            //if (country != null)
            //{
            //    countryId = country.FirstOrDefault(x => x.Text == "India")?.Id;
            //    if (countryId.HasValue)
            //        Country = new SelectList(country, "Id", "Text", countryId.Value);
            //    else
            //        Country = new SelectList(country, "Id", "Text");
            //}


            //if (countryId.HasValue)
            //{
            //    var StateList = await _httpClient.GetAsync("States/GetDropdownByCountry", false, countryId.Value);
            //    var state = !string.IsNullOrEmpty(StateList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(StateList) : null;
            //    if (state != null)
            //    {
            //        State = new SelectList(state, "Id", "Text");
            //    }
            //}


            var result = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyBuyerId", true, BuyerId).ConfigureAwait(false);
            if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");
            BuyerSellerdto = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result) : null;
            if (BuyerSellerdto != null) return Page();

            _notyf.Error("User not found");
            return RedirectToPage("/Index");
        }







        public async Task<IActionResult> OnGetGetStatebyCountryid(int countryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("States/GetState", false, countryid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateResult) : null;
            return new JsonResult(stid);

        }

        public async Task<IActionResult> OnGetGetCitybystateid(int stateid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("Cities/GetCitybystate", false, stateid);
            if (StateResult == "unauthorized") return null;
            var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<CitiesVM>>(StateResult) : null;
            return new JsonResult(stid);
        }
        public async Task<IActionResult> OnGetCheckEmail(string name)
        {
            var emailResult = await _httpClient.GetAsync("BuyerSellerRegistrations/CheckEmail", false, name);
            var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
            return new JsonResult(IfExists);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (BuyerSellerdto.TermsAndConditions == false)
            {
                TempData["Message"] = "Pls check Terms and condition Box";
                _notyf.Error(ModelState.GetErrorMessageString()); return Page();
            }
            else
            {
                string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";
                string SPECIAL_CHARS = "!@#$%^&*()";

                string characters = numbers;
                characters += alphabets + small_alphabets + numbers + SPECIAL_CHARS;

                int length = 10;
                string Password = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    string character = string.Empty;
                    do
                    {
                        int index = new Random().Next(0, characters.Length);
                        character = characters.ToCharArray()[index].ToString();
                    } while (Password.IndexOf(character) != -1);
                    Password += character;
                }

                BuyerSellerdto.Password = Password;
                BuyerSellerdto.IsActive = true;
                BuyerSellerdto.CreatedDate = DateTime.Now;
                var SellerdtoResult = await _httpClient.PostAsync("BuyerSellerRegistrations/Create", false, BuyerSellerdto);
                BuyerSellerdto = !string.IsNullOrEmpty(SellerdtoResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(SellerdtoResult) : null;
                if (SellerdtoResult != null)
                {
                    //-----login Fails - Insert----//
                    Input.Username = BuyerSellerdto.Email;
                    Input.Email = BuyerSellerdto.Email;
                    Input.Password = Password;
                    Input.Role = "Seller";
                    Input.PhoneNumber = BuyerSellerdto.Mobile;
                    Input.Name = BuyerSellerdto.FirstName;
                    Input.ChangePassword = true;
                    Input.IsActive = true;
                    Input.Approved = true;
                    Input.CaptchaCode = "8g2e";
                    Input.ProfileImage = "default_user100.png";
                    Input.SellerId = BuyerSellerdto.Id;

                    var result = await _httpClient.PostAsync("Auth/Register", false, Input).ConfigureAwait(false);
                }
                //if (!ModelState.IsValid)
                //{
                //    _notyf.Error(ModelState.GetErrorMessageString()); return Page();

                //}
                //Sellerdto = !string.IsNullOrEmpty(SellerdtoResult) ? JsonConvert.DeserializeObject<SellerRegistrationsDTO>(SellerdtoResult) : null;
                TempData["Message"] = " Thank You for registering with us. !";
                return RedirectToPage("seller-registration");
            }
        }
    }
}
