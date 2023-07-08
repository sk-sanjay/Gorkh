using Application.Dtos;
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
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Masters.OurCustomers
{
    [Authorize(Roles = "SuperAdmin")]
    public class OurCustomersEditModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public OurCustomersEditModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        public List<OurCustomersVM> ModelVms { get; set; }
        [BindProperty] public OurCustomersDTO OurCustomers { get; set; }
        public bool IsNew => OurCustomers == null;
        public async Task<IActionResult> OnGet()
        {
            var modelResponse = await _httpClient.GetAsync("OurCustomers/Get", true, (int)id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            OurCustomers = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<OurCustomersDTO>(modelResponse) : null;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (id != null)
            {
                var fileoldname = string.IsNullOrEmpty(OurCustomers.Logo)
                                    ? string.Empty : OurCustomers.Logo;

                var FileUploadDto = new FileUploadDTO
                {
                    ChangeName = true,
                    ReturnValue = "name",
                    UploadedFile = Request.Form.Files.GetFile("UserImage"),
                    FilePath = "\\img\\OurCustomersLogo",
                    FileOldName = fileoldname,
                    ChangeDimensions = true,
                    Width = 294,
                    Height = 165
                };

                if (FileUploadDto.UploadedFile != null)
                {
                    //upload new image and delete old image
                    var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFile", true, FileUploadDto).ConfigureAwait(false);
                    if (FileUploadResult == "unauthorized") return new JsonResult("unauthorized");
                    var SavedFileName = !string.IsNullOrEmpty(FileUploadResult) ? JsonConvert.DeserializeObject<string>(FileUploadResult) : null;

                    OurCustomers.Logo = SavedFileName;
                }

                OurCustomers = ModelAuditor<OurCustomersDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), OurCustomers);
                if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PutAsync("OurCustomers/Edit", true, OurCustomers.Id, OurCustomers).ConfigureAwait(false);
            }
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            OurCustomers = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<OurCustomersDTO>(response) : null;
            if (OurCustomers == null)
                _notyf.Error("Data already exists");
            else
                _notyf.Success("Updated successfully");
            return RedirectToPage("Index");
        }
    }
}
