using Application.ViewModels;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsLocationsRepository : Repository<ProductsLocations>, IProductsLocationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsLocationsRepository(AppDbContext context) : base(context)
        {
        }
        public Task<ProductsLocations> GetByProductId(int id)
        {
            return DbContext.ProductsLocations
                .SingleOrDefaultAsync(x => x.ProductId == id);
        }
    }
}
