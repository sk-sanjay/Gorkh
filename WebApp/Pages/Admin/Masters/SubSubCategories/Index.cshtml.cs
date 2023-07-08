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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages.Admin.Masters.SubSubCategories
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public IndexModel(
           IHttpClientService httpClient,
            INotyfService notyf
            )
        {
            _httpClient = httpClient;
            _notyf = notyf;

        }
        public List<SubSubCategoriesVM> ModelVms { get; set; }
        [BindProperty] public SubSubCategoriesDTO ModelDto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("SubSubCategories/Get", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<SubSubCategoriesVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }

        public async Task<JsonResult> OnGetModel(int id)
        {
            // get sub category
            var response = await _httpClient.GetAsync("SubSubCategories/Get", true, id).ConfigureAwait(false);
            if (response == "unauthorized")
                return new JsonResult("unauthorized");
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<SubSubCategoriesDTO>(response) : null;

            // Get SubCategories by Category Id
            var subCategoriesResponse = await _httpClient.GetAsync("SubCategories/GetSubcategory", true, ModelDto.CategoryId).ConfigureAwait(false);
            if (subCategoriesResponse == "unauthorized")
                return new JsonResult("unauthorized");

            var subCategoriesData = !string.IsNullOrEmpty(subCategoriesResponse) ? JsonConvert.DeserializeObject<List<SubCategoriesVM>>(subCategoriesResponse) : null;
            var subCategoriesDropDown = new SelectList(subCategoriesData, "Id", "SubCategoryName");

            return new JsonResult(new { ModelDto, subCategoriesDropDown });
        }
        public async Task<IActionResult> OnPostManage()
        {
            ModelDto = ModelAuditor<SubSubCategoriesDTO>.SetAudit(User.Identity.Name, ModelDto.Id == 0 ? "Create" : "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), ModelDto);
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString());
                return RedirectToPage("Index");
            }
            var response = ModelDto.Id == 0
            ? await _httpClient.PostAsync("SubSubCategories/Create", true, ModelDto).ConfigureAwait(false)
            : await _httpClient.PutAsync("SubSubCategories/Edit", true, ModelDto.Id, ModelDto)
            .ConfigureAwait(false);
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<SubSubCategoriesDTO>(response) : null;
            if (ModelDto == null)
                _notyf.Error("Data already exists!");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("SubSubCategories/Delete", true, id).ConfigureAwait(false);
            if (DeleteResult == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return new JsonResult("unauthorized");
            }
            var RowsChanged = !string.IsNullOrEmpty(DeleteResult) && Convert.ToInt32(DeleteResult) > 0;
            if (RowsChanged)
            {
                _notyf.Success("Deleted successfully");
                return new JsonResult("success");
            }
            _notyf.Error("Delete failed. There might be active child records.");
            return new JsonResult("fail");
        }

        public async Task<JsonResult> OnGetCategory()
        {
            var dropDownVms = await DataHelper.GetDropdown(_httpClient, "Categories", false).ConfigureAwait(false);
            return dropDownVms != null && dropDownVms.Count > 0
                ? new JsonResult(dropDownVms)
                : new JsonResult("Category not found");
        }
        public async Task<IActionResult> OnGetGetSubCategory(int maincat)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SubCategories/GetSubcategory", true, maincat);
            if (StateResult == "unauthorized") return null;
            var catid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SubCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}
            return new JsonResult(catid);
        }
    }
}