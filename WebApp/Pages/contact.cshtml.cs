using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages
{
    [Authorize]
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        public ContactModel(
            IEmailService emailService,
            IConfiguration config)
        {
            _emailService = emailService;
            _config = config;
        }
        [BindProperty] public ContactusDTO ContactusDto { get; set; }
        public IActionResult OnGet()
        {
            ContactusDto = new ContactusDTO();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var emailBody = string.Format("<html>" + Environment.NewLine +
                                          "<body>" + Environment.NewLine +
                                          "<p>{0}</p>" + Environment.NewLine +
                                          "<p>From: {1}</p>" + Environment.NewLine +
                                          "<p>Email: {2}</p>" + Environment.NewLine +
                                          "<p>Contact: {3}</p>" + Environment.NewLine +
                                          "</body>" + Environment.NewLine +
                                          "</html>",
                ContactusDto.Message, (ContactusDto.FirstName + " " + ContactusDto.LastName), ContactusDto.EmailId, ContactusDto.PhoneNo);
            var EmailVm = new EmailVM
            {
                ToAddresses = new List<string> { _config["ErrorEmail"] },
                Subject = ContactusDto.Subject,
                Body = emailBody
            };
          //  await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
            ContactusDto.info = "Message sent succsfully.";
            return Page();
        }
    }
}
