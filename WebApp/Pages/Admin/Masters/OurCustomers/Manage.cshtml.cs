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
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Masters.OurCustomers
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
        public List<OurCustomersVM> ModelVms { get; set; }
        [BindProperty] public OurCustomersDTO OurCustomers { get; set; }
        public bool IsNew => OurCustomers == null;


        public async Task<IActionResult> OnPost()
        {
            OurCustomers = ModelAuditor<OurCustomersDTO>.SetAudit(User.Identity.Name, OurCustomers.Id == 0 ? "Create" : "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), OurCustomers);
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
                FilePath = "\\img\\OurCustomersLogo",
                //FileOldName = fileoldname,
                ChangeDimensions = true,
                Width = 294,
                Height = 165
            };
            var FileUploadResult = await _httpClient.PostMultipartAsync("Files/UploadFile", true, FileUploadDto).ConfigureAwait(false);
            if (FileUploadResult == "unauthorized") return new JsonResult("unauthorized");
            var SavedFileName = !string.IsNullOrEmpty(FileUploadResult) ? JsonConvert.DeserializeObject<string>(FileUploadResult) : null;

            OurCustomers.Logo = SavedFileName;
    //        var stream = new MemoryStream();
    //       // Add watermark
    //var watermarkedStream = new MemoryStream();
    //        using (var img = Image.FromStream(stream))
    //        {
    //            using (var graphic = Graphics.FromImage(img))
    //            {
    //                var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
    //                var color = Color.FromArgb(128, 255, 255, 255);
    //                var brush = new SolidBrush(color);
    //                var point = new Point(img.Width - 120, img.Height - 30);

    //                graphic.DrawString("cnblogs.com/zaranet", font, brush, point);
    //                img.Save(watermarkedStream, ImageFormat.Png);



                    var response = OurCustomers.Id == 0
            ? await _httpClient.PostAsync("OurCustomers/Create", true, OurCustomers).ConfigureAwait(false)
            : await _httpClient.PutAsync("OurCustomers/Edit", true, OurCustomers.Id, OurCustomers).ConfigureAwait(false);
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            OurCustomers = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<OurCustomersDTO>(response) : null;
            if (OurCustomers == null)
                _notyf.Error("Data already exists!");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Index");

        }
    }
}
