using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsFileUploadsRepository : Repository<ProductsFileUploads>, IProductsFileUploadsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsFileUploadsRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductsFileUploads>> GetProductsFileUploadsByProductId(int productid, int flagimage)
        {
            return await DbContext.ProductsFileUploads
                .Where(x => x.ProductId == productid && x.FlagImage == flagimage).ToListAsync();
        }
        public async Task<List<ProductsFileUploads>> GetProductsFileUploadsByProductId(int productid)
        {
            return await DbContext.ProductsFileUploads
                .Where(x => x.ProductId == productid).ToListAsync();
        }
    }
}
