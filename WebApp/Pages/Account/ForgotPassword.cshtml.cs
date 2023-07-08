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
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApp.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IEmailService _emailService;
        private readonly INotyfService _notyf;
        public ForgotPasswordModel(
            IHttpClientService httpClient,
            IEmailService emailService,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _emailService = emailService;
            _notyf = notyf;
        }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(32, ErrorMessage = "{1} characters max")]
        [BindProperty] public string Username { get; set; }

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
            var result = await _httpClient.PostAsync("Auth/ForgotPassword", false, Username).ConfigureAwait(false);
            var forgotPasswordVm = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<ForgotPasswordVM>(result) : null;
            if (forgotPasswordVm?.Code == null || forgotPasswordVm.Email == null)
            {
                _notyf.Error("Password reset link could not be generated");
                return Page();
            }
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { forgotPasswordVm.Code, forgotPasswordVm.Id },
                protocol: Request.Scheme);
            var Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
            //var EmailVm = new EmailVM
            //{
            //    ToAddresses = new List<string> { forgotPasswordVm.Email },
            //    Subject = "Password Reset Link",
            //    Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            //};
            // await _emailService.SendEmailAsync(EmailVm).ConfigureAwait(false);
            await _emailService.SendEmailAsync(forgotPasswordVm.Email, "Password Reset Link",Body, null, null, null).ConfigureAwait(false);
            _notyf.Success("Please check your email to reset your password");
            return LocalRedirect("/Account/Login");
        }
    }
}