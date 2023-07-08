using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SubCategoriesRepository : Repository<SubCategories>, ISubCategoriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public SubCategoriesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<SubCategories>> GetSubCategoryByCategory(int categoryid)
        {
            return DbContext.SubCategories
                .Where(x => x.CategoryId == categoryid && x.IsActive).ToListAsync();
        }
        public Task<List<SubCategories>> GetSubcategory(int maincat)
        {
            return DbContext.SubCategories
                .Where(x => x.CategoryId == maincat && x.IsActive).ToListAsync();
        }

        public Task<SubCategories> CheckDuplicate(SubCategories model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.SubCategories.FirstOrDefaultAsync(x =>
                     x.CategoryId==model.CategoryId && x.SubCategoryName == model.SubCategoryName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.SubCategories.FirstOrDefaultAsync(x =>
                x.CategoryId == model.CategoryId && x.SubCategoryName == model.SubCategoryName && x.Id != model.Id);
                return duplicateModel;

            }

           
        }

        //public Task<List<SubCategories>> GetCategory()
        //{
        //    return (from x in DbContext.SubCategories join 
        //            dm in DbContext.Categories on x.CategoryId equals dm.Id

        //            select new SubCategories
        //            {
        //                CategoryId = x.CategoryId,
        //                EnglishName = dm.EnglishName
        //            }).Distinct().ToListAsync();
        //}
    }
}
