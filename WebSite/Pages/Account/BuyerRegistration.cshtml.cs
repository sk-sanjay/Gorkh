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
    public class BuyerRegistrationModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public BuyerRegistrationModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public string uid => DataHelper.GetUserId(User);
        public string role => DataHelper.GetUserRole(User);
        public string usename => DataHelper.GetUserName(User);
        public string SelleId => DataHelper.GetSellerId(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        [BindProperty] public RegisterDTO Input { get; set; }
        public SelectList Organisation { get; set; }


        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        [BindProperty] public BuyerCommonDTO BuyerCommondto { get; set; }
        //[BindProperty] public CommonRegisterDTO ComReg { get; set; }
        public async Task<IActionResult> OnGetAsync()

        {
            var OrganisationTypesList = await _httpClient.GetAsync("OrganisationTypes/GetDropdown", false);
            var OrganisationTypes = !string.IsNullOrEmpty(OrganisationTypesList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(OrganisationTypesList) : null;
            if (OrganisationTypes != null)
            {
                Organisation = new SelectList(OrganisationTypes, "Id", "Text");
            }

            var result1 = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbySellerId", true, SelleId).ConfigureAwait(false);
            if (result1 == "unauthorized") return RedirectToPage("/Account/Seller-Login");
            BuyerSellerdto = !string.IsNullOrEmpty(result1) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(result1) : null;
            return Page();
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
                // var  BuyersdtoResult = ModelAuditor<BuyerCommonDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), BuyerSellerdto);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                BuyerCommonDTO comma = new BuyerCommonDTO();
                comma.Id = BuyerSellerdto.Id;
                comma.OrganisationType = BuyerSellerdto.OrganisationType;
                response = await _httpClient.PutAsync("BuyerSellerRegistrations/UpdateOrganisationType", true, comma.Id, comma).ConfigureAwait(false);
                //if (BuyersdtoResult != null)
                //{
                //    Input.BuyerId = BuyerSellerdto.Id;

                //    var result = await _httpClient.PostAsync("Auth/Register", false, Input).ConfigureAwait(false);
                //}


                //Buyersdto = !string.IsNullOrEmpty(BuyersdtoResult) ? JsonConvert.DeserializeObject<BuyersDTO>(BuyersdtoResult) : null;
                if (response != "0")
                {
                    CommonRegisterDTO ComReg = new CommonRegisterDTO();
                    ComReg.Emailid = usename;
                    ComReg.BuyerId = BuyerSellerdto.Id;
                    //Input.BuyerId = BuyerSellerdto.Id;
                    response = await _httpClient.PutAsync("Auth/UpdateBuyerId", false, ComReg.Emailid, ComReg).ConfigureAwait(false);
                }
                if (response != "0")
                {
                    //Send SMS to Buyer
                    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Random r = new Random();
                    var name = BuyerSellerdto.FirstName + " " + BuyerSellerdto.LastName;
                    var Number = BuyerSellerdto.Mobile;
                    string Message = "Dear " + name + ", Congratulation! You are now a registered member of Surplusplatform.com; We will make you connect shortly with the genuine seller that matches your search criteria.";
                    string URL = "http://sms.osdigital.in/V2/http-api.php?apikey=uw3USRjocagBmVis&senderid=SRPPLT&number=" + Number + "&message=" + Message + " " + "&format=json";

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();
                    //End Sms send
                    _notyf.Success("Your have successfully Registered as a Buyer!");
                    //TempData["Message"] = "Your have successfully Registered as a Buyer!";
                }
                else
                {
                    _notyf.Error("Save Failed!");
                    // TempData["Message"] = "Save Failed!";

                }
                return RedirectToPage("/Buyer-Registration-Form");
            }
        }
    }


}
