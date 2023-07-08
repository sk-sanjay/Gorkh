using System.Collections.Generic;
namespace Application.ViewModels
{
    public class RoleMenuVM
    {
        public string roleName { get; set; }
        public int menuId { get; set; }
        public List<DropdownVM> assignedMenus { get; set; }
        public List<DropdownVM> remainingMenus { get; set; }
    }
}