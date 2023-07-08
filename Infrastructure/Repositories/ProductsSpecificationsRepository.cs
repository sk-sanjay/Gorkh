using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsSpecificationsRepository : Repository<ProductsSpecifications>, IProductsSpecificationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsSpecificationsRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductsSpecifications>> GetByProductId(int productid)
        {
            return DbContext.ProductsSpecifications.Where(x => x.ProductId == productid).ToList();
        }
    }
}
