using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
   public class BuyerSellerRegistrations1Repository: Repository<BuyerSellerRegistrations1>, IBuyerSellerRegistrations1Repository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyerSellerRegistrations1Repository(AppDbContext context) : base(context)
        {

        }
    }
}
