using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IRoleMenuService
    {
        //Common Methods
        Task<List<RoleMenuVM>> Get();
        Task<RoleMenuDTO> Get(int id);
        Task<RoleMenuDTO> Create(RoleMenuDTO argModelDto);
        Task<RoleMenuDTO> Update(RoleMenuDTO argModelDto);
        Task<int> Delete(int id);
        //Custom Methods
        Task<RoleMenuVM> GetAllByRole(string rolename);
        Task<RoleMenuVM> AssignToRole(RoleMenuDTO argModelDto);
        Task<RoleMenuVM> RemoveFromRole(RoleMenuDTO argModelDto);
        Task<int> UpdateMenus(List<RoleMenuDTO> argModelDtos);
    }
}
