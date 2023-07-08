using Application.ServiceInterfaces;
using Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebSite.Pages
{
    public class pagesModel : PageModel
    {
        private readonly IHttpClientService _httpClient;
        public pagesModel(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }
        [FromRoute] public string OtherLinkHeading { get; set; }
        [BindProperty] public CommonVM ModelVM { get; set; }
        public OtherLinkVM Menus { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var Result = await _httpClient.GetAsync("OtherLink/GetMenus", false, OtherLinkHeading.Replace("-", " ").ToLower());

            if (Result != null)
            {
                Menus = !string.IsNullOrEmpty(Result) ? JsonConvert.DeserializeObject<OtherLinkVM>(Result) : null;
                if (HttpContext.Session.GetString("Lang") == null || HttpContext.Session.GetString("Lang") != "English")
                {
                    ModelVM = new CommonVM
                    {
                        Heading = Menus.EnglishHeadingName,
                        Content = Menus.EnglishContentDesc
                    };
                }
                else
                {
                    ModelVM = new CommonVM
                    {
                        Heading = Menus.HindiHeadingName,
                        Content = Menus.HindiContentDesc
                    };
                }
            }
            else
            {
                TempData["Results"] = "The page you are looking for doesn't exist, you may have mistyped the address or the page may have moved.";
            }

            return Page();
        }
    }
}
