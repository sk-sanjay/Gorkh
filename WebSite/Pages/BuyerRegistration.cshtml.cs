using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebSite.Pages
{
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
        public SelectList Category { get; set; }
        public SelectList Organisation { get; set; }
        public SelectList State { get; set; }
        [BindProperty] public RegisterDTO Input { get; set; }
        public SelectList Country { get; set; }
        [BindProperty] public BuyerSellerRegistrationsDTO BuyerSellerdto { get; set; }
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
                string SPECIAL_CHARS = "@#$-=/";
                string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                string numbers = "1234567890";


                string characters = alphabets + SPECIAL_CHARS + small_alphabets + numbers;

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
                BuyerSellerdto.LandlineNo = "";
                BuyerSellerdto.CompanyName = "";
                BuyerSellerdto.CompanyWebsite = "";
                BuyerSellerdto.Descriptionofproduct = "";

                var BuyersdtoResult = await _httpClient.PostAsync("BuyerSellerRegistrations/Create", false, BuyerSellerdto);
                BuyerSellerdto = !string.IsNullOrEmpty(BuyersdtoResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsDTO>(BuyersdtoResult) : null;
                //if (!ModelState.IsValid)
                //{
                //    _notyf.Error(ModelState.GetErrorMessageString()); return Page();

                //}
                if (BuyersdtoResult != null)
                {
                    //-----login Fails - Insert----//
                    Input.Username = BuyerSellerdto.Email;
                    Input.Email = BuyerSellerdto.Email;
                    Input.Password = Password;
                    Input.Role = "Buyer";
                    Input.PhoneNumber = BuyerSellerdto.Mobile;
                    Input.Name = BuyerSellerdto.FirstName;
                    Input.ChangePassword = true;
                    Input.IsActive = true;
                    Input.Approved = true;
                    Input.CaptchaCode = "8g2e";
                    Input.ProfileImage = "default_user100.png";
                    Input.BuyerId = BuyerSellerdto.Id;

                    var result = await _httpClient.PostAsync("Auth/Register", false, Input).ConfigureAwait(false);
                }


                //Buyersdto = !string.IsNullOrEmpty(BuyersdtoResult) ? JsonConvert.DeserializeObject<BuyersDTO>(BuyersdtoResult) : null;
                if (BuyerSellerdto != null)
                {
                    var filename = _fileService.GetPhysicalPath("/assets/pdfs/SP_USER_AGREEMENT.pdf");
                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    // string ToEmail = SatatEnquiryFormdto.Email;
                    mail.To.Add(BuyerSellerdto.Email);
                    mail.Attachments.Add(new Attachment(filename));
                    mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus-Platform");
                    mail.Subject = "Account credentials";
                    mail.Body = "Account credentials";
                    // string strSubject = "Enquiry";
                    string strBody = "<html><body><div>Dear <b>Ms./Mr. " + BuyerSellerdto.FirstName + " " + BuyerSellerdto.LastName + " </b>,<br/> <br/><b>Greetings from Surplus-Platform!</b> <br/><br/>You have been successfully registered as Seller on Surplus Platform portal https://surplusweb.businesstowork.com <br/><br/>The system generated log-in credentials of your registration are shared below:<br/><br/>Username: " + BuyerSellerdto.Email + "<br/><br/>Password: " + Password + "<br/><br/><b>Please note:</b><br/><br/>a) The username is (not changeable) and the password (changeable)<br/><br/>b) This is a one-time registration.<br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                    mail.Body = strBody;
                    mail.IsBodyHtml = true;
                    smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                    smtp.Port = 587;
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = false;

                    smtp.Send(mail);
                    //Response.Write("<script language='javascript' type='text/javascript'>alert('Your Mail has been successfully Submitted!');</script>");

                    TempData["Message"] = "Your Mail has been successfully Submitted!";

                }

                TempData["Message"] = " Thank You for registering with us. Please check your email for further details. !";
                return RedirectToPage("Buyer-Registration-Form");
            }
        }
    }
}
