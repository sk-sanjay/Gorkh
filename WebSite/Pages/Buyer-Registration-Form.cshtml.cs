using Application.Dtos;
using Application.Extensions;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    public class Buyer_Registration_FormModel : PageModel
    {
        private static Random random = new Random();
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public Buyer_Registration_FormModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public SelectList Category { get; set; }
        public SelectList Organisation { get; set; }
        public SelectList State { get; set; }
        [BindProperty] public RegisterDTO Input { get; set; }
        public SelectList Country { get; set; }
        [BindProperty] public BuyersDTO Buyersdto { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO ucode { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var category = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (category != null)
            {
                Category = new SelectList(category, "Id", "Text");
            }

            var OrganisationTypesList = await _httpClient.GetAsync("OrganisationTypes/GetDropdown", false);
            var OrganisationTypes = !string.IsNullOrEmpty(OrganisationTypesList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(OrganisationTypesList) : null;
            if (OrganisationTypes != null)
            {
                Organisation = new SelectList(OrganisationTypes, "Id", "Text");
            }

            var CountryList = await _httpClient.GetAsync("Countries/GetDropdown", false);
            int? countryId = null;
            var country = !string.IsNullOrEmpty(CountryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(CountryList) : null;
            if (country != null)
            {
                countryId = country.FirstOrDefault(x => x.Text == "India")?.Id;
                if (countryId.HasValue)
                    Country = new SelectList(country, "Id", "Text", countryId.Value);
                else
                    Country = new SelectList(country, "Id", "Text");
            }


            if (countryId.HasValue)
            {
                var StateList = await _httpClient.GetAsync("States/GetDropdownByCountry", false, countryId.Value);
                var state = !string.IsNullOrEmpty(StateList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(StateList) : null;
                if (state != null)
                {
                    State = new SelectList(state, "Id", "Text");
                }
            }

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

        //public async Task<IActionResult> OnGetCheckEmail(string name)
        //{
        //    var emailResult = await _httpClient.GetAsync("BuyerSellerRegistrations/CheckEmail", false, name);
        //    if (emailResult == "unauthorized") return null;
        //    //var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
        //    var IfExists = !string.IsNullOrEmpty(emailResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(emailResult) : null;
        //    return new JsonResult(IfExists);
        //}

        public async Task<IActionResult> OnGetGetbyEmail(string email)
        {   
            var emailResult = await _httpClient.GetAsync("Auth/GetbyEmail", false, email);
            if (emailResult == "unauthorized") return null;
            //var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
            var IfExists = !string.IsNullOrEmpty(emailResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(emailResult) : null;
            return new JsonResult(IfExists);
        }
        public async Task<IActionResult> OnGetGetbyMobile(string mobile)
         {
            var mobileResult = await _httpClient.GetAsync("BuyerSellerRegistrations/GetbyMobile", false, mobile);
            if (mobileResult == "unauthorized") return null;
            //var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
            var IfExists = !string.IsNullOrEmpty(mobileResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(mobileResult) : null;
            return new JsonResult(IfExists);
        }
        

        //public async Task<IActionResult> OnGetCheckEmail(string name)
        //{
        //    var emailResult = await _httpClient.GetAsync("BuyerSellerRegistrations/CheckEmail", false, name);
        //    var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
        //    return new JsonResult(IfExists);
        //}


        //public async Task<IActionResult> OnGetGetCountrybystateid(int stateid)
        //{
        //    var StateResult = "";
        //    StateResult = await _httpClient.GetAsync("States/GetCountry", false, stateid);
        //    if (StateResult == "unauthorized") return null;
        //    var stid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<StatesVM>(StateResult) : null;
        //    if (stid != null)
        //    {
        //        var CityResult = await _httpClient.GetAsync("Cities/GetCitybystate", false, stateid);
        //        if (CityResult == "unauthorized") return null;
        //        var cities = !string.IsNullOrEmpty(CityResult) ? JsonConvert.DeserializeObject<List<CitiesVM>>(CityResult) : null;
        //        stid.Cities = cities;

        //    }
        //    return new JsonResult(stid);
        //}

        public static string RandomString(int length)
        {
            const string lower = "abcdefghijklmnopqrstuvxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string num = "0123456789";
            const string special = "!@#$%&*/";

            string pwd = new string(Enumerable.Repeat(special, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            pwd += new string(Enumerable.Repeat(lower, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            pwd += new string(Enumerable.Repeat(upper, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            pwd += new string(Enumerable.Repeat(num, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            pwd += new string(Enumerable.Repeat(special, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            return pwd;
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

                BuyerSellerdto.Password = RandomString(8);
                BuyerSellerdto.IsActive = true;
                BuyerSellerdto.CreatedDate = DateTime.Now;
                BuyerSellerdto.LandlineNo = "";
                //BuyerSellerdto.CompanyName = "";
                //BuyerSellerdto.CompanyWebsite = "";
                BuyerSellerdto.Descriptionofproduct = "";

                var BuyersdtoResult = await _httpClient.PostAsync("BuyerSellerRegistrations/Create", false, BuyerSellerdto);
                BuyerSellerdto = !string.IsNullOrEmpty(BuyersdtoResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(BuyersdtoResult) : null;
                var result = string.Empty;

                if (BuyerSellerdto != null)
                {
                    //-----login to buyer/seller table
                    Input.Username = BuyerSellerdto.Email;
                    Input.Email = BuyerSellerdto.Email;
                    Input.Password = BuyerSellerdto.Password;
                    Input.Role = "Buyer";
                    Input.PhoneNumber = BuyerSellerdto.Mobile;
                    Input.Name = $"{ BuyerSellerdto.FirstName}{ BuyerSellerdto.FirstName}";
                    Input.ChangePassword = true;
                    Input.IsActive = true;
                    Input.Approved = true;
                    Input.CaptchaCode = "8g2e";
                    Input.ProfileImage = "default_user100.png";
                    Input.BuyerId = BuyerSellerdto.Id;

                    //Insert to user table
                    result = await _httpClient.PostAsync("Auth/Register", false, Input).ConfigureAwait(false);
                    var Uniquecode = await _httpClient.GetAsync("BuyerSellerRegistrations/Get", false, BuyerSellerdto.Id);
                    ucode = !string.IsNullOrEmpty(Uniquecode) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(Uniquecode) : null;
                    if (result != null)
                    {
                        //Send SMS to Buyer
                        var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        Random r = new Random();
                        //var name = BuyerSellerdto.FirstName + " " + BuyerSellerdto.LastName;
                        var Number = BuyerSellerdto.Mobile;
                        var uniquecode = ucode.UniqueCode;
                        var username = BuyerSellerdto.Email;
                        var password = BuyerSellerdto.Password;

                        string Message = "Dear Buyer, Congratulation! You are now a registered member of Surplusplatform.com; We will make you connect shortly with the genuine seller that matches your search criteria. Unique Code: " + uniquecode + ". Username: " + username + " Password: " + password + "";
                        //string Message = "Dear " + name + ", Congratulation! You are now a registered member of Surplusplatform.com; We will make you connect shortly with the genuine seller that matches your search criteria.";
                        string URL = "http://sms.osdigital.in/V2/http-api.php?apikey=uw3USRjocagBmVis&senderid=SRPPLT&number=" + Number + "&message=" + Message + " " + "&format=json";

                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                        StreamReader sr = new StreamReader(resp.GetResponseStream());
                        string results = sr.ReadToEnd();
                        sr.Close();
                        //End Sms send
                        // Send mail to user
                        var filename = _fileService.GetPhysicalPath("/assets/pdfs/SP_USER_AGREEMENT.pdf");
                        MailMessage mail = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        mail.To.Add(BuyerSellerdto.Email);
                        mail.Bcc.Add("surplusplatforms@gmail.com");
                        mail.Attachments.Add(new Attachment(filename));
                        mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus Platform");
                        mail.Subject = "Account Credentials";
                        mail.Body = "Account credentials";
                        string strBody = "<html><body><div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Subject- Successful registration as buyer</b><br>Dear Sir,<br/>Congratulations! You have successfully signed up.<br/>Surplus Platform always provides quality services to Seller/Buyer for surplus materials & assets.<br/>The dedicated modules are offered for the equitable redistribution, sale and disposal process.<br/><b>Counter Sale:</b> Retail functionality for selling surplus materials to qualified buyers.<br/><b>Online Auction:</b> Functionality for selling surplus materials through onsite auction platform.<br/><b>Web Surplus:</b> Enables eligible organizations to view contents of the surplus warehouse and submit waitlist requests online.<br/><br/>The system generated log-in credentials of your registration are shared below:<br/>Unique Code : " + ucode.UniqueCode + "<br/>Username : " + BuyerSellerdto.Email + "<br/>Password : " + BuyerSellerdto.Password + "<br/><br/><b>Please note:</b><br/>a) The username is (not changeable) and the password (changeable)<br/>b) This is one time registration.<br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                        mail.Body = strBody;
                        mail.IsBodyHtml = true;
                        smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                        smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                        smtp.Port = 587;
                        //Or your Smtp Email ID and Password
                        smtp.EnableSsl = false;

                       // smtp.Send(mail);
                    }

                    if (result != null)
                    {
                        // --------mail sent to admin
                        MailMessage mail = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = false;
                        mail.To.Add("support@surplusplatform.com");
                        //mail.To.Add("dharmkmr90@gmail.com");
                        //mail.Bcc.Add("surplusplatforms@gmail.com");
                        mail.From = new MailAddress("helpdesk@surplusplatform.com");
                        mail.Subject = "Account Details";
                        mail.Body = "Buyer Registration";
                        string strBody = "<html><body>";
                        strBody = strBody + "<p>Dear Sir,<br/> New Buyer has been registered successfully:</p>";
                        strBody = strBody + "<p>Unique Code : " + ucode.UniqueCode + "</p>";
                        strBody = strBody + "<p>Name : Mr. " + BuyerSellerdto.FirstName + " " + BuyerSellerdto.LastName + "</p>";
                        strBody = strBody + "<p>Mobile/Phone No. :" + BuyerSellerdto.Mobile + "</p>";
                        strBody = strBody + "<p>Email : " + BuyerSellerdto.Email + "</p>";

                        //strBody = strBody + "<p>Location:" + BuyerSellerdto.Cities.CityName+ "</p>";
                        mail.Body = strBody;
                        mail.IsBodyHtml = true;
                        smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                        smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                        smtp.Port = 587;
                        //Or your Smtp Email ID and Password
                        smtp.EnableSsl = false;

                        smtp.Send(mail);
                        _notyf.Success("Thank You for registering with us. Please check your email for further details. !");
                    }
                }

                else
                {
                    _notyf.Error("Email Id Already Exist. Please try with Different Email Id!");


                }


                return RedirectToPage("Buyer-Registration-Form");
            }
        }
    }



}
