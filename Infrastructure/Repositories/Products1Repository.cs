using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Infrastructure.Repositories
{
    public class Products1Repository : Repository<Products1>, IProducts1Repository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public Products1Repository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Products1>> GettProductsBySubCatId1(int catid, int subcategoryid)
        {
            return await (from a in DbContext.Products1 where a.CategoryId == catid && a.SubCategoryId == subcategoryid select a).ToListAsync();
            //return await DbContext.Products.Where(x => x.SubCategoryId == subcategoryid).ToListAsync();
        }

        public async Task<List<Products1>> GetProductsasfeaturedmachine()
        {
            return await (from a in DbContext.Products1 select a).ToListAsync();
            //return await DbContext.Products.Where(x => x.SubCategoryId == subcategoryid).ToListAsync();
        }

       
    }
}
