using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class MenuRepository : Repository<Menus>, IMenuRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public MenuRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<Menus>> GetWithAll()
        {
            return DbContext.Menus
                .Where(x =>
                    x.IsActive &&
                    x.ParentId == null)
                .OrderBy(c => c.Id)
                .ToListAsync();
        }

        //NotInUse
        //public Task<List<Menus>> GetAllByRole(string role)
        //{
        //    return DbContext.Menus
        //        .Where(x =>
        //            x.IsActive &&
        //            x.ParentId == null &&
        //            x.RoleMenus.Any(rm => rm.RoleName == role))
        //        .OrderBy(c => c.Sequence)
        //        .ToListAsync();
        //}
    }
}
