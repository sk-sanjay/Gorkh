using Domain.Models;
using System.Collections.Generic;

namespace Application.ViewModels
{
    public class CommonVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Heading { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }

        public string Name { get; set; }
        public string Designation { get; set; }

        public string Address { get; set; }
        //public virtual MenuHeading Parent { get; set; }
        public string EnglishHeadingName { get; set; }
        public string HindiHeadingName { get; set; }
        public string EmailId { get; set; }
        public string Banner { get; set; }
        public string HindiBanner { get; set; }
        public string TelephoneNO { get; set; }
        //public virtual ICollection<MenuHeadingVM> Children { get; set; }
    }
}
