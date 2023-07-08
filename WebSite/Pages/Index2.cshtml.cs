using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Pages
{
    public class Index2Model : PageModel
    {
        public int? age { get; set; }

        public string username { get; set; }

        public void OnGet()
        {
            age = HttpContext.Session.GetInt32("age");
            username = HttpContext.Session.GetString("username");
        }
    }
}
