using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Pages.Admin.OtherLink
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        public IndexModel(
            IHttpClientService httpClient
        )
        {
            _httpClient = httpClient;
        }
        public List<OtherLinkVM> Headings { get; set; }
        public OtherLinkDTO FirstLblMenuDTO { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CategoryId { get; set; }
        public string Status = "";
       // [BindProperty] public OtherLinkDTO otherlinkdto { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var Result = await _httpClient.GetAsync("OtherLink/GetMenuHeadingsWithAll", true);
            Headings = !string.IsNullOrEmpty(Result) ? JsonConvert.DeserializeObject<List<OtherLinkVM>>(Result) : null;
            //if (Result == "unauthorized")
            //{
            //    Status = "Unauthorized";
            //    return new JsonResult(Status);
            //}
            //else
            //{
            //    if (Headings == null)
            //        Status = "Failed";
            //    else
            //        Status = "Success";
            //}
            //return new JsonResult(Headings);
            return Page();
        }
    }
}
