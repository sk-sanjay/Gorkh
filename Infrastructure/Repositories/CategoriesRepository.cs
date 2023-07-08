using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class CategoriesRepository : Repository<Categories>, ICategoriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public CategoriesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<Categories> CheckDuplicate(Categories model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Categories.FirstOrDefaultAsync(x =>
                    x.Name == model.Name);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Categories.FirstOrDefaultAsync(x =>
                    x.Name == model.Name && x.Id != model.Id);
                return duplicateModel;
            }
            }
        
            
            public Task<List<Categories>> GetAllCategoryWithChild()
        {
            //return DbContext.Categories
            //.Where(x => x.IsActive && x.SubCategory.Any(y=>y.IsActive)).ToListAsync();

            return DbContext.Categories.Include(x => x.SubCategory.Where(y => y.IsActive)).ToListAsync();
        }

    }
}
