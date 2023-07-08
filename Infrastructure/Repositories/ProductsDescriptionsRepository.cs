using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsDescriptionsRepository : Repository<ProductsDescriptions>, IProductsDescriptionsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsDescriptionsRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<ProductsDescriptions> GetProductsDescriptionsByProductId(int productid)
        {
            return await DbContext.ProductsDescriptions
                .Where(x => x.ProductId == productid).FirstOrDefaultAsync();
        }
        public Task<ProductsDescriptions> GetByProductId(int id)
        {
            return DbContext.ProductsDescriptions
                .SingleOrDefaultAsync(x => x.ProductId == id);
        }
    }
}
