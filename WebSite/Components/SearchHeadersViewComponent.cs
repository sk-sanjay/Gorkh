using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebSite.Components
{
    public class SearchHeadersViewComponent : ViewComponent
    {
        private readonly IHttpClientService _httpClient;
        public SearchHeadersViewComponent(
            IHttpClientService httpClient
            )
        {
            _httpClient = httpClient;
        }
        public SelectList Category { get; set; }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryList = await _httpClient.GetAsync("Categories/GetDropdown", false);
            var categoryVms = !string.IsNullOrEmpty(categoryList) ? JsonConvert.DeserializeObject<List<DropdownVM>>(categoryList) : null;
            if (categoryVms != null)
            {
                Category = new SelectList(categoryVms, "Id", "Text");
            }
            return View(categoryVms);
        }
    }
}
