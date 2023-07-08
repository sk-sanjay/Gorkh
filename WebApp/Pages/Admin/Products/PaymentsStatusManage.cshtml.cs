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

namespace WebApp.Pages.Admin.Products
{
    [Authorize(Roles = "Admin")]
    public class PaymentsStatusManageModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        private readonly INotyfService _notyf;
        public PaymentsStatusManageModel(
            IHttpClientService httpClient,
            INotyfService notyf)
        {
            _httpClient = httpClient;
            _notyf = notyf;
        }
        [FromRoute] public int? id { get; set; }
        public List<PaymentsCommonVM> ModelVms { get; set; }
        [BindProperty] public PaymentsDTO Payment { get; set; }
        public bool IsNew => Payment == null;
        public async Task<IActionResult> OnPostAsync()
        {
            if (id != null)
            {
                var response = string.Empty;
                PaymentsUpdateDTO comma = new PaymentsUpdateDTO();
                comma.Id = id.Value;
                comma.PaymentStatus = Payment.PaymentStatus;
                comma.RecDate = Payment.RecDate;
                response = await _httpClient.PutAsync("Payments/UpdatePaymentStatus", true, comma.Id, comma).ConfigureAwait(false);
                if (response == "unauthorized")
                {
                    _notyf.Information("Please login/register");
                    return RedirectToPage("/Account/Login");
                }
                //Payment = !string.IsNullOrEmpty(response) ? JsonConvert.DeserializeObject<PaymentsDTO>(response) : null;
                var RowsChanged = !string.IsNullOrEmpty(response) && Convert.ToInt32(response) > 0;
                if (RowsChanged)
                {
                    _notyf.Success("Updated successfully");
                }
                else
                {
                    _notyf.Error("Update failed.");
                }
                
            }
            return Page();
        }
    }
}
