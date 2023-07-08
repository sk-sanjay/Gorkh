using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class VoltageFrequenciesRepository : Repository<VoltageFrequencies>, IVoltageFrequenciesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public VoltageFrequenciesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<VoltageFrequencies> CheckDuplicate(VoltageFrequencies model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.VoltageFrequencies.FirstOrDefaultAsync(x =>
                    x.VoltFrequency == model.VoltFrequency);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.VoltageFrequencies.FirstOrDefaultAsync(x =>
                    x.VoltFrequency == model.VoltFrequency && x.Id != model.Id);
                return duplicateModel;
            }
        }
    }
}
