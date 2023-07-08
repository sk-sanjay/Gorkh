using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ManufacturersRepository : Repository<Manufacturers>, IManufacturersRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ManufacturersRepository(AppDbContext context) : base(context)
        {
        }
        public Task<Manufacturers> CheckDuplicate(Manufacturers model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Manufacturers.FirstOrDefaultAsync(x =>
                      x.MfrName == model.MfrName);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Manufacturers.FirstOrDefaultAsync(x =>
                    x.MfrName == model.MfrName && x.Id != model.Id);
                return duplicateModel;
            }
        }
    }
}
