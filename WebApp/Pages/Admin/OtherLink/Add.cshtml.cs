using Application.Dtos;
using Application.Extensions;
using Application.Helpers;
using Application.ServiceInterfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages.Admin.OtherLink
{
    // [SessionManage]
    [Authorize(Roles = "Admin")]
    public class AddModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        private readonly IFileService _fileService;
        
        //private readonly ICommon _common;
        public AddModel(IHttpClientService httpClient, IFileService fileService, INotyfService notyf)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _notyf = notyf;

        }
        [FromRoute] public int? pid { get; set; }
        [BindProperty] public OtherLinkDTO otherlinkdto { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if (pid != null)
            {
                otherlinkdto.ParentId = pid;
            }
            if (otherlinkdto.EnglishFile != null)
            {  //...........Check Valid File ...................
               
                otherlinkdto.EnglishAttachment = await _fileService.SaveImageAsync(@"\img\UploadedFiles\OtherLink\Files\English\", otherlinkdto.EnglishFile);
                otherlinkdto.EnglishFile = null;
            }
            if (otherlinkdto.HindiFile != null)
            {  //...........Check Valid File ...................
               
                otherlinkdto.HindiAttachment = await _fileService.SaveImageAsync(@"\img\UploadedFiles\OtherLink\Files\Hindi\", otherlinkdto.HindiFile);
                otherlinkdto.HindiFile = null;
            }
            return await CreateAudit(SetAudit("Create", "Pending", otherlinkdto));
        }


        private TempOtherLinkHeadingDTO SetAudit(string action, string status, OtherLinkDTO model)
        {
            var uname = User.Identity.Name;
            var role = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            return new TempOtherLinkHeadingDTO
            {
                EnglishAttachment = model.EnglishAttachment,
                HindiAttachment = model.HindiAttachment,
                EnglishContentDesc = model.EnglishContentDesc,
                HindiContentDesc = model.HindiContentDesc,
                HindiPageLink = model.HindiPageLink,
                EnglishPageLink = model.EnglishPageLink,
                HindiHeadingName = model.HindiHeadingName,
                EnglishHeadingName = model.EnglishHeadingName,
                Title = model.Title,
                Description = model.Description,
                Keyword = model.Keyword,
                ParentId = model.ParentId,
                Priority = model.Priority,
                Action = action,
                Show = model.Show,
                ActionDate = DateTime.UtcNow,
                UserName = uname,
                RoleName = role,
                Status = status,
                IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                RowId = null
            };
        }

        private async Task<IActionResult> CreateAudit(TempOtherLinkHeadingDTO modelDto)
        {
            var x = ModelState.IsValid;
            var Result = await _httpClient.PostAsync("TempOtherLink/CreateAudit", true, modelDto);
            if (Result == "unauthorized")
            {
                TempData["Message"] = "info^Please login/register";
                return RedirectToPage("/Account/Login");
            }
            var TempDTO = !string.IsNullOrEmpty(Result) ? JsonConvert.DeserializeObject<TempOtherLinkHeadingDTO>(Result) : null;
            TempData["Message"] = TempDTO == null ? "danger^Save failed" : "success^Saved for approval";
            return RedirectToPage("Index");
        }
    }
}
