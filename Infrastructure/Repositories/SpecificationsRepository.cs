using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    class SpecificationsRepository : Repository<Specifications>, ISpecificationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public SpecificationsRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Specifications> CheckDuplicate(Specifications model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Specifications.FirstOrDefaultAsync(x =>
                    x.SpecfName == model.SpecfName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Specifications.FirstOrDefaultAsync(x =>
                    x.SpecfName == model.SpecfName && x.Id != model.Id);
                return duplicateModel;
            }
        }
        public Task<List<Specifications>> GetByOrder()
        {
            return DbContext.Specifications
                .OrderByDescending(x => x.IsCommon)
                .ThenBy(x => x.SpecfName)
                .ToListAsync();
        }
    }
}
