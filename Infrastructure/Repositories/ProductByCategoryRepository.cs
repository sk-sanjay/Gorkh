using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
        class ProductByCategoryRepository : Repository<CategoryimgCommon>, IProductByCategoryRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductByCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
