using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsVisitorsRepository : Repository<ProductsVisitors>, IProductsVisitorsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsVisitorsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
