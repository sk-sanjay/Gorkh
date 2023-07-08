using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CitiesRepository : Repository<Cities>, ICitiesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public CitiesRepository(AppDbContext contex) : base(contex)
        {

        }
        public Task<List<Cities>> GetCitybystate(int sid)
        {
            return DbContext.Cities
                .Where(x => x.StateId == sid && x.IsActive).ToListAsync();
        }


        //public Task<List<SubSubCategories>> GetSubSubCategoryBySubCategory(int subcategoryid)
        //{
        //    return DbContext.SubSubCategories
        //        .Where(x => x.SubCategoryId == subcategoryid && x.IsActive).ToListAsync();
        //}
        //public Task<List<SubSubCategories>> GetSubcategory(int maincat)
        //{
        //    return DbContext.SubSubCategories
        //        .Where(x => x.CategoryId == maincat && x.IsActive).ToListAsync();
        //}

        public Task<Cities> CheckDuplicate(Cities model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Cities.FirstOrDefaultAsync(x =>
                    x.CityName == model.CityName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Cities.FirstOrDefaultAsync(x =>
                    x.CityName == model.CityName && x.Id != model.Id);
                return duplicateModel;
            }
        }
    }
}
