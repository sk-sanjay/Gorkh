using Application.Dtos;
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

namespace WebApp.Pages.Admin.Masters.SpecificationsSSCategories
{
    [Authorize(Roles = "SuperAdmin")]
    [IgnoreAntiforgeryToken]
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
        [BindProperty] public SpecificationsSSCategoriesDTO SpecificationsSSCategory { get; set; }
        public bool IsNew => SpecificationsSSCategory == null;
        public List<SpecificationsVM> ModelVms { get; set; }
        public async Task<IActionResult> OnGet()
        {
            //get all the categories for dropdown
            var categories = await DataHelper.GetDropdown(_httpClient, "Categories", false).ConfigureAwait(false);
            if (categories == null || categories.Count <= 0)
            {
                _notyf.Error("Data not found");
                return Page();
            }
            ViewData["Categories"] = new SelectList(categories, "Id", "Text");
            //if (!id.HasValue) return Page();
            //var modelResponse = await _httpClient.GetAsync("States/Get", true, (int)id).ConfigureAwait(false);
            //if (modelResponse == "unauthorized")
            //{
            //    _notyf.Information("Please login/register");
            //    return RedirectToPage("/Account/Login");
            //}
            //State = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<StatesDTO>(modelResponse) : null;
            //return Page();
            var modelResponse = await _httpClient.GetAsync("Specifications/GetByOrder", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<SpecificationsVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
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

        public async Task<IActionResult> OnGetGetSubSubCategoryBySubCategory2(int subcategoryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SubSubCategories/GetSubSubCategoryBySubCategory2", true, subcategoryid);
            if (StateResult == "unauthorized") return null;
            var catid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SubSubCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}
            return new JsonResult(catid);
        }

        [BindProperty] public List<SpecificationsSSCategoriesDTO> SpecificationsSSC { get; set; }

        //-----Get Specification Category By Sub Category Wise

        public async Task<IActionResult> OnGetGetSpecificationsSSCategories(int subsubcategoryid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("SpecificationsSSCategories/GetSpecificationsSSCategories", true, subsubcategoryid);
            if (StateResult == "unauthorized") return null;
            var sdata = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<SpecificationsSSCategoriesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}

            return new JsonResult(sdata);
        }

        public async Task<JsonResult> OnPostManage(string roleMenus)
        {
            var model = JsonConvert.DeserializeObject<List<SpecificationsSSCategoriesDTO>>(roleMenus);

            //SpecificationsSSC = specfdetails;
            var SpecificationsSSCategoriesDTOs = new List<SpecificationsSSCategoriesDTO>();
            foreach (var a in model)
            {
                var SpecificationsSSCategoriesDto = new SpecificationsSSCategoriesDTO
                {
                    CategoryId = a.CategoryId,
                    SubCategoryId = a.SubCategoryId,
                    SubSubCatId = a.SubSubCatId,
                    IsActive = true,
                    SpecfId = a.SpecfId,
                    Sequence = a.Sequence,
                    IsMandatory = a.IsMandatory


                };
                SpecificationsSSCategoriesDto = ModelAuditor<SpecificationsSSCategoriesDTO>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), SpecificationsSSCategoriesDto);
                SpecificationsSSCategoriesDTOs.Add(SpecificationsSSCategoriesDto);
            }
            SpecificationsSSCategoriesDTOs = ModelAuditor<List<SpecificationsSSCategoriesDTO>>.SetAudit(User.Identity.Name, "Create", HttpContext.Connection.RemoteIpAddress.ToString(), SpecificationsSSCategoriesDTOs);
            var result = await _httpClient.PostAsync("SpecificationsSSCategories/UpdateSpecificationsSSCategories", true, SpecificationsSSCategoriesDTOs).ConfigureAwait(false);
            if (result == "unauthorized") return new JsonResult("unauthorized");
            var rowsChanged = !string.IsNullOrEmpty(result) ? Convert.ToInt32(result) : 0;
            return rowsChanged > 0 ? new JsonResult("Saved successfully") : new JsonResult("Save failed");
        }
    }
}
