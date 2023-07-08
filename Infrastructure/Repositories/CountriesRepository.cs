using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class CountriesRepository : Repository<Countries>, ICountriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public CountriesRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Countries> CheckDuplicate(Countries model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Countries.FirstOrDefaultAsync(x =>
                       x.Name == model.Name);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Countries.FirstOrDefaultAsync(x =>
                    x.Name == model.Name && x.Id != model.Id);
                return duplicateModel;

            }

        }

        //public Task<List<Countries>> GetCountrybystateid(int stateid)
        //{
        //    return DbContext.Countries.Where(x => x.Id == stateid && x.IsActive).ToListAsync();
        //}
        public async Task<List<Countries>> GetCountryByProdcutWise()
        {
            var productcountry = (from a in DbContext.ProductsLocations
                                  select new
                                  {
                                      CountryId = a.CountryId
                                  }
                                 ).Distinct();
            return await (from a in DbContext.Countries
                          join b in productcountry on a.Id equals b.CountryId
                          select new Countries
                          {
                              Id = a.Id,
                              Name = a.Name
                          }).ToListAsync();

        }
    }
}
