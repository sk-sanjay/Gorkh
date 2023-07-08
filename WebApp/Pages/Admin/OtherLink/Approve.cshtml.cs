using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspNetCoreHero.ToastNotification.Abstractions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.OtherLink
{
   
    [Authorize(Roles = "Admin")]
    public class ApproveModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly IFileService _fileService;
        private readonly INotyfService _notyf;
        public ApproveModel(IHttpClientService httpClient, IFileService fileService, INotyfService notyf)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _notyf = notyf;
        }
        [FromRoute] public int id { get; set; }
        [BindProperty] public TempOtherLinkHeadingDTO TempDTO { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var tempResult = await _httpClient.GetAsync("TempOtherLink/Get", true, id);
            if (tempResult == "unauthorized")
            {
                TempData["Message"] = "info^Please login/register";
                return RedirectToPage("/Account/Login");
            }
            TempDTO = !string.IsNullOrEmpty(tempResult) ? JsonConvert.DeserializeObject<TempOtherLinkHeadingDTO>(tempResult) : null;
            if (TempDTO == null)
            {
                TempData["Message"] = "info^Record not found";
                return RedirectToPage("Pending");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (TempDTO.EnglishFile != null)
            {
                TempDTO.EnglishAttachment = await _fileService.SaveImageAsync(@"\img\UploadedFiles\OtherLink\Files\English\", TempDTO.EnglishFile);
                TempDTO.EnglishFile = null;
            }
            if (TempDTO.HindiFile != null)
            {
                TempDTO.HindiAttachment = await _fileService.SaveImageAsync(@"\img\UploadedFiles\OtherLink\Files\Hindi\", TempDTO.HindiFile);
                TempDTO.HindiFile = null;
            }
            var modelDto = new OtherLinkDTO
            {
                Id = TempDTO.RowId ?? 0,
                ParentId = TempDTO.ParentId,
                HindiHeadingName = TempDTO.HindiHeadingName,
                EnglishHeadingName = TempDTO.EnglishHeadingName,
                EnglishPageLink = TempDTO.EnglishPageLink,
                HindiPageLink = TempDTO.HindiPageLink,
                HindiContentDesc = TempDTO.HindiContentDesc,
                EnglishContentDesc = TempDTO.EnglishContentDesc,
                EnglishAttachment = TempDTO.EnglishAttachment,
                HindiAttachment = TempDTO.HindiAttachment,
                Title = TempDTO.Title,
                Description = TempDTO.Description,
                Keyword = TempDTO.Keyword,
                Priority = TempDTO.Priority,
                Show = TempDTO.Show,
            };

            if (TempDTO.Action == "Create")
            {

                var createResponse = await _httpClient.PostAsync("OtherLink/Create", true, modelDto);
                if (createResponse == "unauthorized")
                {
                    TempData["Message"] = "info^Please login/register";
                    return RedirectToPage("/Account/Login");
                }
                var createResult = !string.IsNullOrEmpty(createResponse) ? JsonConvert.DeserializeObject<OtherLinkDTO>(createResponse) : null;
                if (createResult == null)
                {
                    TempData["Message"] = "danger^Create failed";
                    return RedirectToPage("Pending");
                }
                var tempModeldto = SetAudit(TempDTO.Action, "Approved", createResult);
                return await CreateAudit(tempModeldto);
            }

            if (TempDTO.Action == "Edit")
            {
                var editResponse = await _httpClient.PutAsync("OtherLink/Edit", true, modelDto.Id, modelDto);
                if (editResponse == "unauthorized")
                {
                    TempData["Message"] = "info^Please login/register";
                    return RedirectToPage("/Account/Login");
                }
                var editResult = !string.IsNullOrEmpty(editResponse) ? JsonConvert.DeserializeObject<OtherLinkDTO>(editResponse) : null;
                if (editResult == null)
                {
                    TempData["Message"] = "danger^Edit failed";
                    return RedirectToPage("Pending");
                }
                var tempModelDto = SetAudit(TempDTO.Action, "Approved", editResult);
                return await CreateAudit(tempModelDto);
            }

            if (TempDTO.Action == "Delete")
            {
                var deleteResponse = await _httpClient.DeleteAsync("OtherLink/Delete", true, modelDto.Id);
                if (deleteResponse == "unauthorized")
                {
                    TempData["Message"] = "info^Please login/register";
                    return RedirectToPage("/Account/Login");
                }
                var RowsChanged = !string.IsNullOrEmpty(deleteResponse) && Convert.ToInt32(deleteResponse) > 0;
                if (!RowsChanged)
                {
                    TempData["Message"] = "danger^Delete failed";
                    return RedirectToPage("Pending");
                }
                var tempModelDto = SetAudit(TempDTO.Action, "Approved", modelDto);
                return await CreateAudit(tempModelDto);
            }
            TempData["Message"] = "danger^Action not specified";
            return RedirectToPage("Pending");
        }

        public async Task<IActionResult> OnPostReject()
        {
            var ModelDTO = new OtherLinkDTO
            {
                Id = TempDTO.RowId ?? 0,
                HindiHeadingName = TempDTO.HindiHeadingName,
                EnglishHeadingName = TempDTO.EnglishHeadingName,
                EnglishPageLink = TempDTO.EnglishPageLink,
                HindiPageLink = TempDTO.HindiPageLink,
                HindiContentDesc = TempDTO.HindiContentDesc,
                EnglishContentDesc = TempDTO.EnglishContentDesc,
                EnglishAttachment = TempDTO.EnglishAttachment,
                HindiAttachment = TempDTO.HindiAttachment,
                Title = TempDTO.Title,
                Description = TempDTO.Description,
                Keyword = TempDTO.Keyword,
                Priority = TempDTO.Priority,
            };
            var tempModelDTO = SetAudit(TempDTO.Action, "Rejected", ModelDTO);
            return await CreateAudit(tempModelDTO);
        }

        private TempOtherLinkHeadingDTO SetAudit(string action, string status, OtherLinkDTO model)
        {
            var uname = User.Identity.Name;
            var role = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            return new TempOtherLinkHeadingDTO
            {
                HindiHeadingName = TempDTO.HindiHeadingName,
                EnglishHeadingName = TempDTO.EnglishHeadingName,
                EnglishPageLink = TempDTO.EnglishPageLink,
                HindiPageLink = TempDTO.HindiPageLink,
                HindiContentDesc = TempDTO.HindiContentDesc,
                EnglishContentDesc = TempDTO.EnglishContentDesc,
                EnglishAttachment = TempDTO.EnglishAttachment,
                HindiAttachment = TempDTO.HindiAttachment,
                Title = TempDTO.Title,
                Description = TempDTO.Description,
                Keyword = TempDTO.Keyword,
                Priority = TempDTO.Priority,
                Action = action,
                ActionDate = DateTime.UtcNow,
                UserName = uname,
                RoleName = role,
                Status = status,
                IP = HttpContext.Connection.RemoteIpAddress.ToString(),
                RowId = model.Id == 0 ? (int?)null : model.Id,
                Show = model.Show,
            };
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
