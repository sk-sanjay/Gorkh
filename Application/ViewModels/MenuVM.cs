using System.Collections.Generic;
namespace Application.ViewModels
{
    public class MenuVM
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int Sequence { get; set; }
        public string MenuText { get; set; }
        public string IconClass { get; set; }
        public string PageUrl { get; set; }
        public bool IsActive { get; set; }
        public List<MenuVM> Children { get; set; }
    }
}
