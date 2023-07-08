using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
  public class ProductCountRepository : Repository<ProductCount>, IProductCountRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductCountRepository(AppDbContext context) : base(context)
        {

        }
    }
}
