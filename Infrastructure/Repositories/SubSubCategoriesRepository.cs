using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SubSubCategoriesRepository : Repository<SubSubCategories>, ISubSubCategoriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public SubSubCategoriesRepository(AppDbContext contex) : base(contex)
        {

        }
        public Task<List<SubSubCategories>> GetSubSubCategoryBySubCategory(int subcategoryid)
        {
            return DbContext.SubSubCategories
                .Where(x => x.SubCategoryId == subcategoryid && x.IsActive).ToListAsync();
        }
        public Task<List<SubSubCategories>> GetSubcategory(int maincat)
        {
            return DbContext.SubSubCategories
                .Where(x => x.CategoryId == maincat && x.IsActive).ToListAsync();
        }
        public Task<List<SubSubCategories>> GetSubSubCategoryBySubCategory2(int subcategoryid)
        {
            return DbContext.SubSubCategories
                .Where(x => x.SubCategoryId == subcategoryid && x.IsActive).ToListAsync();
        }


        public Task<SubSubCategories> CheckDuplicate(SubSubCategories model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.SubSubCategories.FirstOrDefaultAsync(x =>
                 x.SubCategoryId == model.SubCategoryId && x.SubSubCategoriesName == model.SubSubCategoriesName);
                return duplicateModel;
            }

            else
            {
                var duplicateModel = DbContext.SubSubCategories.FirstOrDefaultAsync(x =>
                    x.SubSubCategoriesName == model.SubSubCategoriesName && x.SubCategoryId == model.SubCategoryId && x.Id != model.Id);

                return duplicateModel;
            }
        }
        public Task<List<SubSubCategories>> SearchSubSubCategory(string prefix)
        {
            return (from a in DbContext.SubSubCategories
                    where a.SubSubCategoriesName.StartsWith(prefix)
                    select new SubSubCategories
                    {
                        SubSubCategoriesName = a.SubSubCategoriesName
                    }).ToListAsync();
        }
    }
}
