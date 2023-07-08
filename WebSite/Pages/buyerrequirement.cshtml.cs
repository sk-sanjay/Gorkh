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
using System.Net.Mail;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages
{
   // [Authorize(Roles = "Buyer")]
    public class buyerrequirementModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;

        public buyerrequirementModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public string BuyerId => DataHelper.GetBuyerId(User);
        public SelectList Category { get; set; }
        public SelectList Condition { get; set; }
        public CategoriesVM Categories { get; set; }
        public SubSubCategoriesVM SubSubCategories { get; set; }
        [BindProperty] public BuyerSellerRegistrationsVM BuyerSellervm { get; set; }
        [BindProperty] public BuyerRequirementsDTO BuyerRequirementsdto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (category != null)
            {
                Category = new SelectList(category, "Id", "Text");
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
        public async Task<IActionResult> OnGetGetbyEmail(string email)
        {
            var emailResult = await _httpClient.GetAsync("Auth/GetbyEmail", false, email);
            if (emailResult == "unauthorized") return null;
            //var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
            var IfExists = !string.IsNullOrEmpty(emailResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(emailResult) : null;
            return new JsonResult(IfExists);
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

        public async Task<IActionResult> OnPostAsync()
        {

            BuyerRequirementsdto.CreatedDate = DateTime.Now;
            //BuyerRequirementsdto.Files = "abc.jpg";
            BuyerRequirementsdto.IsActive = true;
            if (BuyerRequirementsdto.upload1 != null)
            {
                BuyerRequirementsdto.Files = await _fileService.SaveImageAsync(@"\Files\Enquiry", BuyerRequirementsdto.upload1);
                BuyerRequirementsdto.upload1 = null;
            }



            var Result = await _httpClient.PostAsync("BuyerRequirements/Create", false, BuyerRequirementsdto);
            BuyerRequirementsdto = !string.IsNullOrEmpty(Result) ? JsonConvert.DeserializeObject<BuyerRequirementsDTO>(Result) : null;
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString()); return Page();

            }

            if (BuyerRequirementsdto != null)
            {
                var buyerdata = await _httpClient.GetAsync("Auth/GetbyEmail", false, BuyerRequirementsdto.EmailID);
                var BuyerDetails=!string.IsNullOrEmpty(buyerdata) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(buyerdata) : null;

                var modelResponse = await _httpClient.GetAsync("Categories/Get", false, BuyerRequirementsdto.CategoryId).ConfigureAwait(false);
                Categories = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<CategoriesVM>(modelResponse) : null;

                var result = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyBuyerId", false, BuyerDetails.BuyerId).ConfigureAwait(false);
                //if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");
                BuyerSellervm = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(result) : null;

                var Subsubcategory = await _httpClient.GetAsync("SubSubCategories/Get", false, BuyerRequirementsdto.SubSubCategoryId).ConfigureAwait(false);
                SubSubCategories = !string.IsNullOrEmpty(Subsubcategory) ? JsonConvert.DeserializeObject<SubSubCategoriesVM>(Subsubcategory) : null;

                if (BuyerSellervm != null)
                {
                    //Send SMS to Buyer
                    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Random r = new Random();
                    var name = BuyerSellervm.FirstName + " " + BuyerSellervm.LastName;
                    var Number = BuyerSellervm.Mobile;
                    string Message = "Dear Mr. " + name + ", Greetings! Your requirement has been successfully sent to our genuine sellers. The surplusplatform.com teams immediate fulfill your searching type of product. Best wishes Surplus Platform";
                    string URL = "http://sms.osdigital.in/V2/http-api.php?apikey=uw3USRjocagBmVis&senderid=SRPPLT&number=" + Number + "&message=" + Message + " " + "&format=json";

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();
                    //End Sms send
                    // send mail to buyer
                    string strBody = "<html><body>";
                    strBody = strBody + "<p>Dear Buyer,</p>";
                    strBody = strBody + "<p>Greetings</p>";
                    strBody = strBody + "<p>Thanks indeed for your requirement of wanted Product Name - "+ BuyerRequirementsdto.SubSubCategoryName + ". and Code Number - "+ BuyerSellervm.UniqueCode+".</p>";
                    strBody = strBody + "<p>To start the ‘buy’ process, you are requested to pay our service charges as 2% of above Reserved Price to surplusplatform.com.</p>";
                    strBody = strBody + "<p>For details, please read <b>Service Charges</b> at bottom of our website.</p>";
                    strBody = strBody + "<p>In case of any query, you may write to support@surplusplatform.com or call us on 8287344537.</p>";
                    strBody = strBody + "<p><b>Team Surplus Platform</b></p>";

                    var EmailVm = new EmailVM
                    {
                        ToAddresses = new List<string> { BuyerSellervm.Email },
                        Subject = "Buyer Requirement",
                        Body = strBody
                    };
                    await _emailService.SendEmailAsync(BuyerSellervm.Email, "Buyer Requirement", strBody, null, null, null, null).ConfigureAwait(false);
                    _notyf.Success("Your Requirement has been submitted Successfully !");

                }
                //if (BuyerSellervm != null)
                //{
                //    // send mail to Admin
                //    string strBody = "<html><body>";
                //    strBody = strBody + "<p>Dear Sir,<br/> New Requirement from Buyer has been received successfully !</p>";
                //    strBody = strBody + "<p>Category : " + Categories.Name + "</p>";
                //    strBody = strBody + "<p>Product Description : " + BuyerRequirementsdto.Descrition + "</p>";
                //    strBody = strBody + "<p>Buyer Name : " + BuyerSellervm.FirstName + " " + BuyerSellervm.LastName + " </p>";
                //    strBody = strBody + "<p>Mobile/Phone No. :" + BuyerSellervm.Mobile + "</p>";
                //    strBody = strBody + "<p>Email : " + BuyerSellervm.Email + "</p>";

                //    var EmailVm = new EmailVM
                //    {
                //        ToAddresses = new List<string> { "dharmkmr90@gmail.com" },
                //        Subject = "Wanted equipment",
                //        Body = strBody
                //    };
                //    await _emailService.SendEmailAsync("dharmkmr90@gmail.com", "Wanted equipment", strBody, null, null, null, null).ConfigureAwait(false);
                //    

                //}
            }
            return RedirectToPage("buyerrequirement");
        }
    }

}


