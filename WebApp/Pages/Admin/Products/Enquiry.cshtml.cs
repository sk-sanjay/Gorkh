using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Products
{
    [Authorize(Roles = "Admin")]
    public class EnquiryModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        public EnquiryModel(
            IHttpClientService httpClient,
            INotyfService notyf, IFileService fileService, IEmailService emailService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _fileService = fileService;
            _emailService = emailService;
        }
        [FromRoute] public int? id { get; set; }
        [FromRoute] public int? pid { get; set; }
        [BindProperty] public ProductsEnquiriesDTO ProductsEnquiry { get; set; }
        public bool IsNew => ProductsEnquiry == null;
        public List<ProductsEnquiriesVM> ModelVms { get; set; }
        public ProductsSellerDetailsVM ProductsSellerDetail { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var modelResponse1 = await _httpClient.GetAsync("Products/GetProductsSellerDetailsById", true, (int)pid).ConfigureAwait(false);
            if (modelResponse1 == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ProductsSellerDetail = !string.IsNullOrEmpty(modelResponse1) ? JsonConvert.DeserializeObject<ProductsSellerDetailsVM>(modelResponse1) : null;

            var modelResponse = await _httpClient.GetAsync("ProductsEnquiries/GetProductsEnquiriesByPid", true, (int)pid).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<ProductsEnquiriesVM>>(modelResponse) : null;
            //if (ModelVms == null || ModelVms.Count <= 0)
            //    _notyf.Error("Data not found");
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            var SavedFileName = string.Empty;
            if (id == null)
            {
                var FileUploadDto = new FileUploadDTO
                {
                    ChangeName = true,
                    ReturnValue = "name",
                    UploadedFile = Request.Form.Files.GetFile("UserImage"),
                    FilePath = "\\img\\penquiry",
                    //FileOldName = fileoldname,
                    ChangeDimensions = true,
                    Width = 500,
                    Height = 500
                };
                var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFile", true, FileUploadDto).ConfigureAwait(false);
                if (FileUploadResult == "unauthorized") return new JsonResult("unauthorized");
                SavedFileName = !string.IsNullOrEmpty(FileUploadResult) ? JsonConvert.DeserializeObject<string>(FileUploadResult) : null;

                ProductsEnquiry.EnquiryFile = SavedFileName;
                ProductsEnquiry.ProductId = (int)pid;
                ProductsEnquiry.CreatedDate = DateTime.Now;
                ProductsEnquiry.CreatedBy = "Admin";
                //State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), State);
                //if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PostAsync("ProductsEnquiries/Create", true, ProductsEnquiry).ConfigureAwait(false);
            }
            //else
            //{
            //    State = ModelAuditor<StatesDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), State);
            //    if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
            //    response = await _httpClient.PutAsync("States/Edit", true, State.Id, State).ConfigureAwait(false);
            //}
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ProductsEnquiry = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<ProductsEnquiriesDTO>(response) : null;
            if (ProductsEnquiry == null)
            {
                _notyf.Error("Data already exists");
            }
            else
            {
                //var relativepath = $"\\img\\penquiry\\" + SavedFileName;
                //var filename = await _httpClient.PostAsync("Files/GetPhysicalPath1", true, relativepath).ConfigureAwait(false);
                ////var filename = filepath + SavedFileName;
                //MailMessage mail = new MailMessage();
                //SmtpClient smtp = new SmtpClient();
                //smtp.UseDefaultCredentials = false;
                //// string ToEmail = SatatEnquiryFormdto.Email;
                //mail.To.Add("naiemahmad29@gmail.com");
                //mail.Attachments.Add(new Attachment(filename));
                //mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus-Platform");
                //mail.Subject = "Subject- Account credentials";
                //mail.Body = "Seller Registration";
                //// string strSubject = "Enquiry";
                //string strBody = "<html><body><div>Dear <b>Greetings from Surplus-Platform!</b> <br/><br/>You have been successfully registered as Buyer on Surplus Platform portal https://surplusweb.businesstowork.com <br/><br/>The system generated log-in credentials of your registration are shared below:<br/><br/><br/><br/><b>Please note:</b><br/><br/>a) The username is (not changeable) and the password (changeable)<br/><br/>b) This is a one-time registration.<br/><br/><b>Regards<br/>Team Surplus Platform</b>";

                //mail.Body = strBody;
                //mail.IsBodyHtml = true;
                //smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                //smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                //smtp.Port = 587;
                ////Or your Smtp Email ID and Password
                //smtp.EnableSsl = false;

                //smtp.Send(mail);

                //await _emailService.SendEmailAsync(_config["ContactEmail"], ContactusDto.Subject, emailBody, null, null, null, null);
                //await _emailService.SendEmailAsync("naiemahmad29@gmail.com","hello", strBody, null, null, null, null);
                //var email1 = "naiemahmad29@gmail.com";
                //Attachment aa = new Attachment(filename);
                //var EmailVm = new EmailVM
                //{
                //    ToAddresses = new List<string> { email1 },
                //    Subject = "Account credentials",
                //    Body = strBody,
                //    Attachments=new List<Attachment> { aa }

                //};
                //await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
                _notyf.Success("Saved successfully");
            }
            return RedirectToPage("Enquiry");
        }
    }
}
