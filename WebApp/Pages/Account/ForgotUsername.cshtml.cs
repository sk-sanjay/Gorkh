using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class ForgotUsernameModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IEmailService _emailService;
        private readonly INotyfService _notyf;
        public ForgotUsernameModel(
            IHttpClientService httpClient,
            IEmailService emailService,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _emailService = emailService;
            _notyf = notyf;
        }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(64, ErrorMessage = "{1} characters max")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Enter a valid email id")]
        [BindProperty] public string Email { get; set; }

        [DisplayName("Captcha Code")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(4)]
        [BindProperty] public string CaptchaCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate Captcha Code
            if (!Captcha.ValidateCaptchaCode(CaptchaCode, HttpContext))
                ModelState.AddModelError("Captcha", "Please enter correct captcha");
            if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
            var result = await _httpClient.PostAsync("Auth/ForgotUsername", false, Email).ConfigureAwait(false);
            var username = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<string>(result) : null;
            if (string.IsNullOrEmpty(username))
            {
                _notyf.Error("Username not found");
                return Page();
            }
            var EmailVm = new EmailVM
            {
                ToAddresses = new List<string> { Email },
                Subject = "Your Username",
                Body = $"Your Username is: <b>{username}</b>"
            };
            //await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
            await _emailService.SendEmailAsync(Email, "Your Username", $"Your Username is: <b>{username}</b>", null, null, null, null).ConfigureAwait(false);
            _notyf.Success("Please check your email for your username");
            return LocalRedirect("/Account/Login");
        }
    }
}