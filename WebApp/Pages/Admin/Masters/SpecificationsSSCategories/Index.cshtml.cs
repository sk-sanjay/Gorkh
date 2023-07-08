using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Helpers;


namespace WebApp.Pages.Admin.Masters.SpecificationsSSCategories
{
    [Authorize(Roles = "SuperAdmin")]
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
        public List<SpecificationsSSCategoriesVM> ModelVms { get; set; }
        [BindProperty] public SpecificationsSSCategoriesDTO ModelDto { get; set; }

        public async Task<JsonResult> OnGetCategory()
        {
            var dropDownVms = await DataHelper.GetDropdown(_httpClient, "Categories", false).ConfigureAwait(false);
            return dropDownVms != null && dropDownVms.Count > 0
                ? new JsonResult(dropDownVms)
                : new JsonResult("Category not found");
        }
    }
}
