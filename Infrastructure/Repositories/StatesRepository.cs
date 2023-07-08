using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class StatesRepository : Repository<States>, IStatesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public StatesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<States>> GetStatesByCountry(int countryid)
        {
            return DbContext.States
                .Where(x => x.CountryId == countryid && x.IsActive).OrderBy(x => x.StateName).ToListAsync();
        }

        public Task<List<States>> GetState(int cid)
        {
            return DbContext.States
                .Where(x => x.CountryId == cid && x.IsActive).OrderBy(x => x.StateName).ToListAsync();
        }

        public Task<States> CheckDuplicate(States model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.States.FirstOrDefaultAsync(x =>
                      x.StateName == model.StateName);
                return duplicateModel;
            }
            else
            {

                var duplicateModel = DbContext.States.FirstOrDefaultAsync(x =>
                   x.CountryId == model.CountryId && x.StateName == model.StateName && x.Id != model.Id);
                return duplicateModel;
            }

        }
        public async Task<List<States>> GetStatesByProdcutWise(int countryid)
        {
            var productstate = (from a in DbContext.ProductsLocations
                                where a.CountryId == countryid
                                select new
                                {
                                    StateId = a.StateId
                                }
                                 ).Distinct();
            
            return await (from a in DbContext.States
                          join b in productstate on a.Id equals b.StateId
                          select new States
                          {
                              Id = a.Id,
                              StateName = a.StateName
                          }).ToListAsync();

        }

    }
}
