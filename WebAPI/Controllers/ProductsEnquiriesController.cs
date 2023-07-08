using Application.Dtos;
using Application.Extensions;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductsEnquiriesController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IEmailService _emailService;
        private readonly IRandomService _randomService;
        private readonly IFileService _fileService;
        public ProductsEnquiriesController(
            IDataService dataService,
            IEmailService emailService,
            IRandomService randomService,
            IFileService fileService)
        {
            _dataService = dataService;
            _emailService = emailService;
            _randomService = randomService;
            _fileService = fileService;
        }
        // GET ProductsEnquiries
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var modelVms = await _dataService.ProductsEnquiries.Get().ConfigureAwait(false);
            if (modelVms == null || modelVms.Count <= 0) return NotFound("Products Enquiries not found");
            return Ok(modelVms);
        }
        // GET ProductsEnquiries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var modelDto = await _dataService.ProductsEnquiries.Get(id).ConfigureAwait(false);
            if (modelDto == null) return NotFound("Products Enquiries not found");
            return Ok(modelDto);
        }
        // POST: ProductsEnquiries/Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductsEnquiriesDTO inputModel)
        {
            var modelProductsSellerDetails = await _dataService.Products.GetProductsSellerDetailsById(inputModel.ProductId).ConfigureAwait(false);

            if (inputModel == null) return BadRequest("Input not valid or null");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());
            var modelDto = await _dataService.ProductsEnquiries.Create(inputModel).ConfigureAwait(false);

            ////send the email
            //var body = string.Format("<html>" + Environment.NewLine +
            //                         "<body>" + Environment.NewLine +
            //                         "<h3>Respected User</h3>" + Environment.NewLine +
            //                         "<p>Congratulations, your registration as a <b>{0}</b> with TEMP MIS System has been successful.</p>" + Environment.NewLine +
            //                         "<p>Your one time credentials for login into the system  are:</p>" + Environment.NewLine +
            //                         "<p>Username: <b>{1}</b></p>" + Environment.NewLine +
            //                         "<p>Password: <b>{2}</b></p>" + Environment.NewLine + Environment.NewLine +
            //                         "<p><b>Note</b>: This password is a one time temporary password, you are highly recommended to change your password on your first login.</p>" + Environment.NewLine + Environment.NewLine +
            //                         "<p>Regards</p>" + Environment.NewLine +
            //                         "<p>SURPLUS PLATFORM</p>" + Environment.NewLine +
            //                         "</body>" + Environment.NewLine +
            //                         "</html>",inputModel.Descriptions,inputModel.ProductId,inputModel.CreatedBy);
            //var relativepath = $"\\img\\penquiry\\" + inputModel.EnquiryFile;
            //var NameOrPath = _fileService.GetPhysicalPath(relativepath);
            //Attachment aa = new Attachment(NameOrPath);
            ////SendGrid.Helpers.Mail.Attachment b1 = new SendGrid.Helpers.Mail.Attachment();
            ////b1.Filename = NameOrPath;
            //var EmailVm = new EmailVM
            //{
            //    ToAddresses = new List<string> { "naiemahmad29@gmail.com" },
            //    Subject = "Product Query",
            //    Body = body,
            //    Attachments = new List<Attachment> { aa },
            //    //SendGridAttachments=new List<SendGrid.Helpers.Mail.Attachment> { b1 }
            //};
            //await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
            ////mail send end

            //new code for send email
            if (inputModel.CreatedBy == "Admin")
            {
                var relativepath = $"\\img\\penquiry\\" + inputModel.EnquiryFile;
                var NameOrPath = _fileService.GetPhysicalPath(relativepath);

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                // string ToEmail = SatatEnquiryFormdto.Email;
                mail.To.Add(modelProductsSellerDetails.SellerEmail);
                if (System.IO.File.Exists(NameOrPath))
                {
                    mail.Attachments.Add(new Attachment(NameOrPath));
                }
                //mail.Attachments.Add(new Attachment(NameOrPath));
                mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus-Platform");
                mail.Subject = "Product Query";
                mail.Body = "Seller Registration";
                //string strBody = "<html><body><div>Dear <b>Greetings from Surplus-Platform!</b> <br/><br/>You have been successfully registered as Buyer on Surplus Platform portal https://surplusweb.businesstowork.com <br/><br/>The system generated log-in credentials of your registration are shared below:<br/><br/><br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                var strBody = string.Format("<html>" + Environment.NewLine +
                                         "<body>" + Environment.NewLine +
                                         "<h3>Dear {0}</h3>" + Environment.NewLine +
                                         "<p>Product Name: <b>{1}</b></p>" + Environment.NewLine +
                                         "<p>{2}</p>" + Environment.NewLine +
                                         "<p>Please click here to response https://surplusweb.businesstowork.com</p>" + Environment.NewLine + Environment.NewLine +
                                         "<p>Regards</p>" + Environment.NewLine +
                                         "<p>Team Surplus Platform</p>" + Environment.NewLine +
                                         "</body>" + Environment.NewLine +
                                         "</html>", modelProductsSellerDetails.SellerName, modelProductsSellerDetails.SubSubCategoriesName, inputModel.Descriptions);
                mail.Body = strBody;
                mail.IsBodyHtml = true;
                smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                smtp.Port = 587;
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = false;

                smtp.Send(mail);
            }
            else
            {
                //seller send email to admin
                var relativepath = $"\\img\\penquiry\\" + inputModel.EnquiryFile;
                var NameOrPath = _fileService.GetPhysicalPath(relativepath);

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                // string ToEmail = SatatEnquiryFormdto.Email;
                mail.To.Add("dharmkmr90@gmail.com");
                if (System.IO.File.Exists(NameOrPath))
                {
                    mail.Attachments.Add(new Attachment(NameOrPath));
                }
                //mail.Attachments.Add(new Attachment(NameOrPath));
                mail.From = new MailAddress("helpdesk@surplusplatform.com", "Surplus-Platform");
                mail.Subject = "Product Query";
                mail.Body = "Seller Registration";
                //string strBody = "<html><body><div>Dear <b>Greetings from Surplus-Platform!</b> <br/><br/>You have been successfully registered as Buyer on Surplus Platform portal https://surplusweb.businesstowork.com <br/><br/>The system generated log-in credentials of your registration are shared below:<br/><br/><br/><br/><b>Regards<br/>Team Surplus Platform</b>";
                var strBody = string.Format("<html>" + Environment.NewLine +
                                         "<body>" + Environment.NewLine +
                                         "<h3>Dear Admin</h3>" + Environment.NewLine +
                                         "<p>Seller Name: <b>{0}</b></p>" + Environment.NewLine +
                                         "<p>Product Name: <b>{1}</b></p>" + Environment.NewLine +
                                         "<p>{2}</p>" + Environment.NewLine +
                                         "<p>Please click here to response https://surplusweb.businesstowork.com</p>" + Environment.NewLine + Environment.NewLine +
                                         "<p>Regards</p>" + Environment.NewLine +
                                         "<p>Team Surplus Platform</p>" + Environment.NewLine +
                                         "</body>" + Environment.NewLine +
                                         "</html>", modelProductsSellerDetails.SellerName, modelProductsSellerDetails.SubSubCategoriesName, inputModel.Descriptions);
                mail.Body = strBody;
                mail.IsBodyHtml = true;
                smtp.Host = "mail.surplusplatform.com"; //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential("helpdesk@surplusplatform.com", "SU#rp!L$#321*9"); // ***use valid credentials***
                smtp.Port = 587;
                //Or your Smtp Email ID and Password
                smtp.EnableSsl = false;

                smtp.Send(mail);
            }
            //new code for send email end

            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Create failed");
        }
        // PUT: ProductsEnquiries/Edit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] ProductsEnquiriesDTO inputModel)
        {
            if (inputModel == null) return BadRequest("Input not valid or null");
            if (id != inputModel.Id) return BadRequest("Invalid Id");
            if (!ModelState.IsValid) return BadRequest(ModelState.GetErrorMessages());

            var modelDto = await _dataService.ProductsEnquiries.Update(inputModel).ConfigureAwait(false);
            if (modelDto != null) return Ok(modelDto);
            return BadRequest("Update failed");
        }
        // DELETE: ProductsEnquiries/Delete/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Input not valid or null");
            var rowsChanged = await _dataService.ProductsEnquiries.Delete(id).ConfigureAwait(false);
            if (rowsChanged > 0) return Ok(rowsChanged);
            return BadRequest("Delete failed. There might be active child records.");
        }
        //get products details by id
        [AllowAnonymous]
        [HttpGet("{pid}")]
        public async Task<IActionResult> GetProductsEnquiriesByPid(int pid)
        {
            var modelVms = await _dataService.ProductsEnquiries.GetProductsEnquiriesByPid(pid).ConfigureAwait(false);
            if (modelVms == null) return NotFound("Products not found");
            return Ok(modelVms);
        }
    }
}
