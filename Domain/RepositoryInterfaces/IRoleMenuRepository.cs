using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IRoleMenuRepository : IRepository<RoleMenus>
    {
        Task<List<RoleMenus>> GetAllByRole(string role);
        Task<RoleMenus> GetByRoleMenu(string roleName, int menuId);
    }
}
