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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    public class VisitorRegistrationsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IRandomService _randomService;
        public VisitorRegistrationsModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IRandomService randomService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _randomService = randomService;
        }


        [BindProperty] public RegisterDTO Input { get; set; }
        [BindProperty] public VisitorRegistrationsDTO VisitorRegistrationsdto { get; set; }
        public async Task<IActionResult> OnGetGetbyEmail(string email)
        {
            var emailResult = await _httpClient.GetAsync("Auth/GetbyEmail", false, email);
            if (emailResult == "unauthorized") return null;
            //var IfExists = !string.IsNullOrEmpty(emailResult) && Convert.ToBoolean(emailResult);
            var IfExists = !string.IsNullOrEmpty(emailResult) ? JsonConvert.DeserializeObject<BuyerSellerRegistrationsVM>(emailResult) : null;
            return new JsonResult(IfExists);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            VisitorRegistrationsdto.Status = true;
            VisitorRegistrationsdto.CreatedDate = DateTime.Now;
            VisitorRegistrationsdto.Password= await _randomService.RandomPassword().ConfigureAwait(false);
            var SellerdtoResult = await _httpClient.PostAsync("VisitorRegistrations/Create", false, VisitorRegistrationsdto);
            VisitorRegistrationsdto = !string.IsNullOrEmpty(SellerdtoResult) ? JsonConvert.DeserializeObject<VisitorRegistrationsDTO>(SellerdtoResult) : null;
            var result = string.Empty;
            if (VisitorRegistrationsdto != null)
            {


                //-----login Fails - Insert----//
                Input.Username = VisitorRegistrationsdto.Email;
                Input.Email = VisitorRegistrationsdto.Email;
                Input.Password = VisitorRegistrationsdto.Password;
                Input.Role = "Visitor";
                Input.PhoneNumber = VisitorRegistrationsdto.ContactNo;
                Input.Name = $"{ VisitorRegistrationsdto.FirstName} { VisitorRegistrationsdto.LastName}";
                Input.ChangePassword = true;
                Input.IsActive = true;
                Input.Approved = true;
                Input.CaptchaCode = "8g2e";
                Input.ProfileImage = "default_user100.png";

                result = await _httpClient.PostAsync("Auth/Register", false, Input).ConfigureAwait(false);
                
                if (result != null)
                {
                    //Send SMS to Seller
                    var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                    Random r = new Random();
                    var name = VisitorRegistrationsdto.FirstName + " " + VisitorRegistrationsdto.LastName;
                    var Number = VisitorRegistrationsdto.ContactNo;
                    var username = VisitorRegistrationsdto.Email;
                    var password = VisitorRegistrationsdto.Password;
                    string Message = "Dear Visitor, Congratulation! You are now a registered member of Surplusplatform.com; Username: " + username + " Password: " + password + "";
                    string URL = "http://sms.osdigital.in/V2/http-api.php?apikey=uw3USRjocagBmVis&senderid=SRPPLT&number=" + Number + "&message=" + Message + " " + "&format=json";

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd();
                    sr.Close();
                    //End Sms send
                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;

                    mail.To.Add(VisitorRegistrationsdto.Email);
                    mail.Bcc.Add("surplusplatforms@gmail.com");

                    mail.From = new MailAddress("helpdesk@surplusplatform.com");
                    mail.Subject = "Account Credentials";
                    mail.Body = "Visitor Registration";
                    string strBody = "<html><body><div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Subject- Successful registration as Visitor</b><br>Dear Sir,<br/>Congratulations! You have successfully signed up.<br/>Surplus Platform always provides quality services to Seller/Buyer for surplus materials & assets.<br/>The dedicated modules are offered for the equitable redistribution, sale and disposal process.<br/><b>Counter Sale:</b> Retail functionality for selling surplus materials to qualified buyers.<br/><b>Online Auction:</b> Functionality for selling surplus materials through onsite auction platform.<br/><b>Web Surplus:</b> Enables eligible organizations to view contents of the surplus warehouse and submit waitlist requests online.<br/><br/>The system generated log in credentials of your registration are shared below:<br/>Username : " + VisitorRegistrationsdto.Email + "<br/>Password : " + VisitorRegistrationsdto.Password + "<br/><br/><b>Please note:</b><br/>a) The username is (not changeable) and the password (changeable)<br/>b) This is one-time registration.<br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                    mail.Body = strBody;
                    mail.IsBodyHtml = true;
                    smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                    smtp.Port = 587;
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = false;

                    smtp.Send(mail);
                }

                if (result != null)
                {

                    // --------mail sent to admin
                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    mail.To.Add("support@surplusplatform.com");
                    //mail.To.Add("dharmkmr90@gmail.com");
                  // mail.Bcc.Add("surplusplatforms@gmail.com");
                    mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus Platform");
                    mail.Subject = "Visitor Registration Details";
                    mail.Body = "Visitor Registration";
                    string strBody = "<html><body>";
                    strBody = strBody + "<p>Dear Sir,<br/> New Visitor has been registered successfully:</p>";
                    strBody = strBody + "<p>Name : Mr. " + VisitorRegistrationsdto.FirstName + "  " + VisitorRegistrationsdto.LastName + "</p>";
                    strBody = strBody + "<p>Mobile/Phone No. :" + VisitorRegistrationsdto.ContactNo + "</p>";
                    strBody = strBody + "<p>Email : " + VisitorRegistrationsdto.Email + "</p>";
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
                _notyf.Error("Somethig Went Wrong Please Try Again !");
            }


            return RedirectToPage("VisitorRegistrations");
        }
    }
}
