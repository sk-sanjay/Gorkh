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
using System.Threading.Tasks;
using WebSite.Helpers;
using System.Net.Mail;

namespace WebSite.Pages
{
    public class BuyerPaymentsModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        public BuyerPaymentsModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
        }
        public string role => DataHelper.GetUserRole(User);
        public string BuyerId => DataHelper.GetBuyerId(User);
        public string EmailId => DataHelper.GetUserName(User);
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? pid { get; set; }
        [BindProperty] public PaymentsDTO Payment { get; set; }
        public bool IsNew => Payment == null;
        public ProductsDetailsVM ProductsDetail { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, pid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                //_notyf.Information("Please login/register");
                //return RedirectToPage("/Account/Login");
                return null;
            }
            ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;
            if (ProductsDetail == null)
                _notyf.Error("Data not found");

            Payment = new PaymentsDTO
            {
                AmountRp = ProductsDetail.ReservePrice
            };

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (role == "Buyer" && BuyerId != "0")
            {
                if (id == null || IsNew)
                {
                    Payment.ProductId = pid.Value;
                    Payment.BuyerId = Convert.ToInt32(BuyerId); //it means buyerid
                    Payment.CreatedDate = DateTime.Now;
                    response = await _httpClient.PostAsync("Payments/Create", false, Payment).ConfigureAwait(false);
                }
                Payment = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<PaymentsDTO>(response) : null;
                if (Payment == null)
                {
                    _notyf.Warning("You have already paid reserve price.");
                }
                else
                {
                    var modelResponse = await _httpClient.GetAsync("Products/GettProductsDetailsById", false, pid).ConfigureAwait(false);
                    if (modelResponse == "unauthorized")
                    {
                        //_notyf.Information("Please login/register");
                        //return RedirectToPage("/Account/Login");
                        return null;
                    }
                    ProductsDetail = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<ProductsDetailsVM>(modelResponse) : null;

                    MailMessage mail = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    // string ToEmail = SatatEnquiryFormdto.Email;
                    mail.To.Add(EmailId);
                    mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus-Platform");
                    mail.Subject = "Product Payments";
                    mail.Body = "Payments";
                    //string strBody = "<html><body><div>Dear <b>Greetings from Surplus-Platform!</b> <br/><br/>You have been successfully registered as Buyer on Surplus Platform portal https://surplusweb.businesstowork.com <br/><br/>The system generated log-in credentials of your registration are shared below:<br/><br/><br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                    var paymentmode = string.Empty;
                    var ddchbgno = string.Empty;
                    if (Payment.PaymentModeId == 2)
                    {
                        paymentmode = "<p>Payment Mode: Offline</p>" + Environment.NewLine;
                        ddchbgno = "<p>DD / Ch. No.: " + Payment.DdChequeNo + "</p>" + Environment.NewLine;
                    }
                    else
                    {
                        paymentmode = "<p>Payment Mode: Bank Guarantee</p>" + Environment.NewLine;
                        ddchbgno = "<p>BG No.: " + Payment.BgNo + "</p>" + Environment.NewLine;
                    }

                    var strBody = string.Format("<html>" + Environment.NewLine +
                                             "<body>" + Environment.NewLine +
                                             "<h3>Dear Member</h3>" + Environment.NewLine +
                                             "<p>Product Name: <b>{0}</b></p>" + Environment.NewLine +
                                             "<p>Product Code: {1}</p>" + Environment.NewLine +
                                             //"<p>Payment Mode: {2}</p>" + Environment.NewLine +
                                             paymentmode +
                                             //"<p>DD / Ch. No.: {2}</p>" + Environment.NewLine +
                                             //"<p>BG No.: {3}</p>" + Environment.NewLine +
                                             ddchbgno +
                                             "<p>Date: {2}</p>" + Environment.NewLine +
                                             "<p>Drawn On: {3}</p>" + Environment.NewLine +
                                             "<p>Amount - Reserve Price: {4}</p>" + Environment.NewLine +
                                             "<p>Please click here to response https://surplusweb.businesstowork.com</p>" + Environment.NewLine + Environment.NewLine +
                                             "<p>Regards</p>" + Environment.NewLine +
                                             "<p>Team Surplus Platform</p>" + Environment.NewLine +
                                             "</body>" + Environment.NewLine +
                                             "</html>", ProductsDetail.SubSubCategoriesName, ProductsDetail.ProductNumber,  Payment.ChAndBgDate, Payment.DrawnOn, Payment.AmountRp);
                    mail.Body = strBody;
                    mail.IsBodyHtml = true;
                    smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                    smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                    smtp.Port = 587;
                    //Or your Smtp Email ID and Password
                    smtp.EnableSsl = false;

                   // smtp.Send(mail);

                    _notyf.Success("Saved successfully");
                    return RedirectToPage("BuyerPayments", new { pid = pid.Value });
                }
            }
            else
                _notyf.Error("Please Login as a buyer to pay reserve price.");
            //return Page();
            return RedirectToPage("BuyerPayments", new { pid = pid.Value });
        }
    }
}
