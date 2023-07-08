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
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp.Pages.Admin.Masters.Cities
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
        
        public List<CitiesVM> ModelVms { get; set; }
        [BindProperty] public CitiesDTO ModelDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("Cities/Get", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<CitiesVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }

        public async Task<JsonResult> OnGetModel(int id)
        {
            // get sub category
            var response = await _httpClient.GetAsync("Cities/Get", true, id).ConfigureAwait(false);
            if (response == "unauthorized")
                return new JsonResult("unauthorized");
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<CitiesDTO>(response) : null;

            // Get State by Country Id
            var StateDataResponse = await _httpClient.GetAsync("States/GetState", true, ModelDto.CountryId).ConfigureAwait(false);
            if (StateDataResponse == "unauthorized")
                return new JsonResult("unauthorized");

            var StateData = !string.IsNullOrEmpty(StateDataResponse) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateDataResponse) : null;
            var StateDropDown = new SelectList(StateData, "Id", "StateName");

            return new JsonResult(new { ModelDto, StateDropDown });
        }
        public async Task<IActionResult> OnPostManage()
        {
            ModelDto = ModelAuditor<CitiesDTO>.SetAudit(User.Identity.Name, ModelDto.Id == 0 ? "Create" : "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), ModelDto);
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString());
                return RedirectToPage("Index");
            }
            var response = ModelDto.Id == 0
            ? await _httpClient.PostAsync("Cities/Create", true, ModelDto).ConfigureAwait(false)
            : await _httpClient.PutAsync("Cities/Edit", true, ModelDto.Id, ModelDto)
            .ConfigureAwait(false);
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<CitiesDTO>(response) : null;
            if (ModelDto == null)
                _notyf.Error("Save Failed!");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("Cities/Delete", true, id).ConfigureAwait(false);
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

        public async Task<JsonResult> OnGetCountry()
        {
            var dropDownVms = await DataHelper.GetDropdown(_httpClient, "Countries", false).ConfigureAwait(false);
            return dropDownVms != null && dropDownVms.Count > 0
                ? new JsonResult(dropDownVms)
                : new JsonResult("Countries not found");
        }
        public async Task<IActionResult> OnGetGetState(int cid)
        {
            var StateResult = "";
            StateResult = await _httpClient.GetAsync("States/GetState", true, cid);
            if (StateResult == "unauthorized") return null;
            var catid = !string.IsNullOrEmpty(StateResult) ? JsonConvert.DeserializeObject<List<StatesVM>>(StateResult) : null;
            //if(catid ! = null)
            //{
            //    ViewData["SubCategories"] = new SelectList(catid, "Id", "EnglishName");
            //}
            return new JsonResult(catid);
        }
    }
}
