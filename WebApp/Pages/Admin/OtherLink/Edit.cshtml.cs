using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.OtherLink
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;
        public EditModel(
            IHttpClientService httpClient, IFileService fileService, INotyfService notyf
        )
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _notyf = notyf;

        }
        [FromRoute] public int id { get; set; }
        [BindProperty] public OtherLinkDTO otherlinkdto { get; set; }

        public async Task<IActionResult> OnGet( int id)
        {
            var CategoryResult = await _httpClient.GetAsync("OtherLink/Get", true, id);
            if (CategoryResult == "unauthorized")
            {
                TempData["Message"] = "info^Please login/register";
                return RedirectToPage("/Account/Login");
            }
            otherlinkdto = !string.IsNullOrEmpty(CategoryResult) ? JsonConvert.DeserializeObject<OtherLinkDTO>(CategoryResult) : null;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
           
                otherlinkdto = ModelAuditor<OtherLinkDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), otherlinkdto);
                if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PutAsync("OtherLink/Edit", true, otherlinkdto.Id, otherlinkdto).ConfigureAwait(false);
            
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            otherlinkdto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<OtherLinkDTO>(response) : null;
            if (otherlinkdto != null)
                
                _notyf.Success("Data Updated successfully");
            else
                _notyf.Success("Something went wrong please try again! ");
            return RedirectToPage("Index");
        }
        private async Task<IActionResult> CreateAudit(TempOtherLinkHeadingDTO modelDto)
        {
            if (id != 0)
            {
                modelDto.Id = id;
                modelDto.Show = false;
                var example = await _httpClient.PutAsync("TempOtherLink/Edit", true, id, modelDto);
            }
            modelDto.Show = true;
            modelDto.Id = 0;
            var tempResponse = await _httpClient.PostAsync("TempOtherLink/CreateAudit", true, modelDto);
            if (tempResponse == "unauthorized")
            {
                TempData["Message"] = "info^Please login/register";
                return RedirectToPage("/Account/Login");
            }
            var tempResult = !string.IsNullOrEmpty(tempResponse) ? JsonConvert.DeserializeObject<TempOtherLinkHeadingDTO>(tempResponse) : null;
            TempData["Message"] = tempResult == null ? "danger^Operation failed" : "success^Operation successfull";
            return RedirectToPage("Pending");
        }


    }
}
