using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public  class BuyerandSellerCountRepository : Repository<BuyerSellerCount>, IBuyerandSellerCountRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyerandSellerCountRepository(AppDbContext context) : base(context)
        {

        }
    }
}
