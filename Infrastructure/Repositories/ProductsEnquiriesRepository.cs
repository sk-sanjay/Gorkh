using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsEnquiriesRepository : Repository<ProductsEnquiries>, IProductsEnquiriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsEnquiriesRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductsEnquiries>> GetProductsEnquiriesByPid(int ProductId)
        {
            return await (from a in DbContext.ProductsEnquiries where a.ProductId == ProductId orderby a.Id descending select a).ToListAsync();

        }
    }
}
