using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class shippingWeightsRepository : Repository<ShippingWeights>, IShippingWeightsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public shippingWeightsRepository(AppDbContext context) : base(context)
        {
        }
        public Task<ShippingWeights> CheckDuplicate(ShippingWeights model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.shippingWeights.FirstOrDefaultAsync(x =>
                    x.UnitName == model.UnitName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.shippingWeights.FirstOrDefaultAsync(x =>
                    x.UnitName == model.UnitName && x.Id != model.Id);
                return duplicateModel;
            }
        }
    }
}
