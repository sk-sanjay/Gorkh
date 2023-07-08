using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    class CurrentsRepository : Repository<Currents>, ICurrentsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public CurrentsRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Currents> CheckDuplicate(Currents model)
        {
            if(model.Id==0)
            {
                var duplicateModel = DbContext.Currents.FirstOrDefaultAsync(x =>
                    x.CurrentName == model.CurrentName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Currents.FirstOrDefaultAsync(x =>
                    x.CurrentName == model.CurrentName && x.Id != model.Id);
                return duplicateModel;
            }
            
        }
    }
}
