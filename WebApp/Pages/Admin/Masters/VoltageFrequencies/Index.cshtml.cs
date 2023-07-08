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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.Masters.VoltageFrequencies
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public IndexModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        public List<VoltageFrequenciesVM> ModelVms { get; set; }
        [BindProperty] public VoltageFrequenciesDTO ModelDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var modelResponse = await _httpClient.GetAsync("VoltageFrequencies/Get", true).ConfigureAwait(false);
            if (modelResponse == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelVms = !string.IsNullOrEmpty(modelResponse) ? JsonConvert.DeserializeObject<List<VoltageFrequenciesVM>>(modelResponse) : null;
            if (ModelVms == null || ModelVms.Count <= 0)
                _notyf.Error("Data not found");
            return Page();
        }
        public async Task<JsonResult> OnGetModel(int id)
        {
            var response = await _httpClient.GetAsync("VoltageFrequencies/Get", true, id).ConfigureAwait(false);
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<VoltageFrequenciesDTO>(response) : null;
            if (ModelDto == null)
                _notyf.Error("Data not found");
            if (ModelDto == null)
                _notyf.Error("Data not found");
            return new JsonResult(ModelDto);
        }
        public async Task<IActionResult> OnPostManage()
        {
            ModelDto = ModelAuditor<VoltageFrequenciesDTO>.SetAudit(User.Identity.Name, ModelDto.Id == 0 ? "Create" : "Edit", HttpContext.Connection.RemoteIpAddress.ToString(), ModelDto);
            if (!ModelState.IsValid)
            {
                _notyf.Error(ModelState.GetErrorMessageString());
                return RedirectToPage("Index");
            }
            var response = ModelDto.Id == 0
            ? await _httpClient.PostAsync("VoltageFrequencies/Create", true, ModelDto).ConfigureAwait(false)
            : await _httpClient.PutAsync("VoltageFrequencies/Edit", true, ModelDto.Id, ModelDto)
            .ConfigureAwait(false);
            if (response == "unauthorized")
            {
                _notyf.Information("Please login/register");
                return RedirectToPage("/Account/Login");
            }
            ModelDto = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<VoltageFrequenciesDTO>(response) : null;
            if (ModelDto == null)
                _notyf.Error("Data already exists!");
            else
                _notyf.Success("Saved successfully");
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnGetDelete(int id)
        {
            var DeleteResult = await _httpClient.DeleteAsync("VoltageFrequencies/Delete", true, id).ConfigureAwait(false);
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
    }
}
