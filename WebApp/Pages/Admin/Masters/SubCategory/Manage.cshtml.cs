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
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages.Admin.Masters.SubCategory
{
    [Authorize(Roles = "SuperAdmin")]
    public class ManageModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public ManageModel(IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        [BindProperty] public SubCategoriesDTO SubCategoriesDTO { get; set; }
        public bool IsNew => SubCategoriesDTO == null;
        public async Task<IActionResult> OnGet()
        {
            //get all the countries for dropdown
            var categories = await DataHelper.GetDropdown(_httpClient, "Categories", false).ConfigureAwait(false);
            if (categories == null || categories.Count <= 0)
            {
                _notyf.Error("Data not found");
                return Page();
            }
            ViewData["Categories"] = new SelectList(categories, "Id", "Text");
            if (!id.HasValue) return Page();
            var modelResponse = await _httpClient.GetAsync("SubCategories/Get", true, (int)id).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            SubCategoriesDTO = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<SubCategoriesDTO>(modelResponse) : null;
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var response = string.Empty;
            if (id == null || IsNew)
            {
                SubCategoriesDTO = ModelAuditor<SubCategoriesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), SubCategoriesDTO);
                if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PostAsync("SubCategories/Create", true, SubCategoriesDTO).ConfigureAwait(false);
            }
            else
            {
                SubCategoriesDTO = ModelAuditor<SubCategoriesDTO>.SetAudit(User.Identity.Name, "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), SubCategoriesDTO);
                if (!ModelState.IsValid) { _notyf.Error(ModelState.GetErrorMessageString()); return Page(); }
                response = await _httpClient.PutAsync("SubCategories/Edit", true, SubCategoriesDTO.Id, SubCategoriesDTO).ConfigureAwait(false);
            }
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            SubCategoriesDTO = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<SubCategoriesDTO>(response) : null;
            if (SubCategoriesDTO == null)
                _notyf.Error("Save failed");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDelete(int Id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("States/Delete", true, Id).ConfigureAwait(false);
            if (DeleteResult == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            var RowsChanged = !string.IsNullOrEmpty(DeleteResult) && Convert.ToInt32(DeleteResult) > 0;
            if (RowsChanged)
                _notyf.Success("Deleted successfully");
            else
                _notyf.Error("Delete failed. There might be active child records.");
            return RedirectToPage("Index");
        }



    }
}
