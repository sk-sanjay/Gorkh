using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebSite.Pages
{
    public class PartialDemoModel : PageModel
    {
        public void OnGet()
        {
        }
        public PartialViewResult OnGetAboutPartial()
        {
            //List<string> countries = new List<string>();
            //countries.Add("USA");
            //countries.Add("UK");
            //countries.Add("India");
            return Partial("_AboutPartial");
        }
        public PartialViewResult OnGetContactPartial()
        {
            //List<string> countries = new List<string>();
            //countries.Add("USA");
            //countries.Add("UK");
            //countries.Add("India");
            return Partial("_ContactPartial");
        }
    }
}
