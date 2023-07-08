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

namespace WebApp.Pages.Admin.Masters.Banners
{
    [Authorize(Roles = "SuperAdmin")]
    public class ManageModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ManageModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        public List<BannersVM> ModelVms { get; set; }
        [BindProperty] public BannersDTO Banner { get; set; }
        public bool IsNew => Banner == null;


        public async Task<IActionResult> OnPost()
        {
            Banner = ModelAuditor<BannersDTO>.SetAudit(User.Identity.Name, Banner.Id == 0 ? "Create" : "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), Banner);
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString());
                return RedirectToPage("Index");
            }

            var FileUploadDto = new FileUploadDTO
            {
                ChangeName = true,
                ReturnValue = "name",
                UploadedFile = Request.Form.Files.GetFile("UserImage"),
                FilePath = "\\img\\banners",
                //FileOldName = fileoldname,
                ChangeDimensions = true,
                Width = 1920,
                Height = 585
            };
            var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFile", true, FileUploadDto).ConfigureAwait(false);
            if (FileUploadResult == "unauthorized") return new JsonResult("unauthorized");
            var SavedFileName = !string.IsNullOrEmpty(FileUploadResult) ? JsonConvert.DeserializeObject<string>(FileUploadResult) : null;

            Banner.BannerImage = SavedFileName;

            var response = Banner.Id == 0
            ? await _httpClient.PostAsync("Banners/Create", true, Banner).ConfigureAwait(false)
            : await _httpClient.PutAsync("Banners/Edit", true, Banner.Id, Banner).ConfigureAwait(false);
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            Banner = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<BannersDTO>(response) : null;
            if (Banner == null)
                _notyf.Error("Data already exists!");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Manage");

        }
    }
}
