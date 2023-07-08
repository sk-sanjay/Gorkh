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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages.Account
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
        public string usename => DataHelper.GetUserName(User);
        public SelectList Category { get; set; }
        public SelectList Organisation { get; set; }
        public SelectList State { get; set; }
        public SelectList Country { get; set; }
        [BindProperty] public RegisterDTO Input { get; set; }
        [BindProperty] public SellerRegistrationsDTO Sellerdto { get; set; }
        public List<BuyerSellerRegistrationsVM> BuyerSellerRegistrationsVM { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var result1 = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyBuyerId", true, BuyerId).ConfigureAwait(false);
            if (result1 == "unauthorized") return RedirectToPage("/Account/Seller-Login");
            BuyerSellerdto = !string.IsNullOrEmpty(result1) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result1) : null;
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
            var response = string.Empty;
            if (BuyerSellerdto.TermsAndConditions == false)
            {
                TempData["Message"] = "Pls check Terms and condition Box";
                _notyf.Error(ModelState.GetErrorMessageString()); return Page();
            }
            else
            {
                SellerCommonDTO scommon = new SellerCommonDTO();
                scommon.Id = BuyerSellerdto.Id;
                scommon.CompanyName = BuyerSellerdto.CompanyName;
                scommon.CompanyWebsite = BuyerSellerdto.CompanyWebsite;
                scommon.LandlineNo = BuyerSellerdto.LandlineNo;
                scommon.Descriptionofproduct = BuyerSellerdto.Descriptionofproduct;
                response = await _httpClient.PutAsync("BuyerSellerRegistrations/UpdateDetails", true, scommon.Id, scommon).ConfigureAwait(false);



                if (response != "0")
                {
                    CommonRegisterDTO ComReg = new CommonRegisterDTO();
                    ComReg.Emailid = usename;
                    ComReg.SellerId = BuyerSellerdto.Id;
                    //Input.BuyerId = BuyerSellerdto.Id;
                    response = await _httpClient.PutAsync("Auth/UpdateSellerId", false, ComReg.Emailid, ComReg).ConfigureAwait(false);
                }
                if (response != "0")
                {
                    //Send SMS to Seller
                    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Random r = new Random();
                    var name = BuyerSellerdto.FirstName + " " + BuyerSellerdto.LastName;
                    var Number = BuyerSellerdto.Mobile;
                    string Message = "Dear " + name + ", Congratulation! You are now a registered member of Surplusplatform.com; We will make you connect shortly with the genuine buyers, searching for your type of products.";
                    string URL = "http://sms.osdigital.in/V2/http-api.php?apikey=uw3USRjocagBmVis&senderid=SRPPLT&number=" + Number + "&message=" + Message + " " + "&format=json";

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();
                    //End Sms send
                    _notyf.Success("Your have successfully Registered as a Seller!");
                    //TempData["Message"] = "Your have successfully Registered as a Seller!";
                }
                else
                {
                    _notyf.Error("Save Failed!");
                   //TempData["Message"] = "Save Failed!";

                }
                return RedirectToPage("/Seller-Registration");
            }
        }
    }
}
