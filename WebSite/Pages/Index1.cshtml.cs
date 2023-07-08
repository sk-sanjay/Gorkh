using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Pages
{
    public class Index1Model : PageModel
    {
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            HttpContext.Session.SetInt32("age", 20);
            HttpContext.Session.SetString("username", "abc");
            return RedirectToPage("Index2");
        }
    }
}
