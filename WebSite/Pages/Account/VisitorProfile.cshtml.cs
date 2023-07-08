using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Helpers;

namespace WebSite.Pages.Account
{
    [Authorize(Roles = "Visitor")]
    public class VisitorProfileModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public VisitorProfileModel(IHttpClientService httpClient, INotyfService notyf, IEmailService emailService, IFileService fileService)
        {
            _httpClient = httpClient;
            _notyf = notyf;
            _emailService = emailService;
            _fileService = fileService;
        }
        public string uid => DataHelper.GetUserId(User);
        public string username => DataHelper.GetUserName(User);
        [FromRoute] public int? id { get; set; }
        [BindProperty] public VisitorRegistrationsDTO VisitorRegistrationsdto { get; set; }
        [BindProperty] public BuyerUpdateDTO BuyerUpdateDTO { get; set; }


        public async Task<IActionResult> OnGetAsync()

        {
            var result = await _httpClient.GetAsync("VisitorRegistrations/GetbyEmailId", true, username).ConfigureAwait(false);
            if (result == "unauthorized") return RedirectToPage("/Account/Seller-Login");

            VisitorRegistrationsdto = !string.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<VisitorRegistrationsDTO>(result) : null;
            VisitorRegistrationsdto.VeryfyEmail = VisitorRegistrationsdto.Email;
            return Page();
        }
     
    }

}
