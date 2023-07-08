using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsBuyerIntrestsRepository : Repository<ProductsBuyerIntrests>, IProductsBuyerIntrestsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsBuyerIntrestsRepository(AppDbContext context) : base(context)
        {
        }
        public Task<ProductsBuyerIntrests> CheckDuplicate(ProductsBuyerIntrests model)
        {
            var duplicateModel = DbContext.ProductsBuyerIntrests.FirstOrDefaultAsync(x =>
                  x.ProductId == model.ProductId && x.BuyerId == model.BuyerId);
            return duplicateModel;
        }
        public async Task<List<ProductsBuyerIntrestsCommon>> GetProductsBuyerIntrestsByBuyer (int buyerid)
        {
            return await (from a in DbContext.ProductsBuyerIntrests
                          join b in DbContext.Products on a.ProductId equals b.Id
                          join c in DbContext.SubSubCategories on b.SubSubCatId equals c.Id
                          where a.BuyerId == buyerid
                          orderby a.CreatedDate descending
                          select new ProductsBuyerIntrestsCommon
                          {
                              Id = a.Id,
                              ProductId = a.ProductId,
                              BuyerId = a.BuyerId,
                              CreatedDate = a.CreatedDate,
                              ProductNumber = b.ProductNumber,
                              ReservePrice = b.ReservePrice,
                              SubSubCategoriesName = c.SubSubCategoriesName
                          }
                         ).ToListAsync();
        }
        public async Task<List<ProductsBuyerIntrestsCommon>> GetProductsBuyerIntrestsForAdmin()
        {
            return await (from a in DbContext.ProductsBuyerIntrests
                          join b in DbContext.Products on a.ProductId equals b.Id
                          join c in DbContext.SubSubCategories on b.SubSubCatId equals c.Id
                          join d in DbContext.BuyerSellerRegistrations on a.BuyerId equals d.Id
                          orderby a.CreatedDate descending
                          select new ProductsBuyerIntrestsCommon
                          {
                              Id = a.Id,
                              ProductId = a.ProductId,
                              BuyerId = a.BuyerId,
                              CreatedDate = a.CreatedDate,
                              ProductNumber = b.ProductNumber,
                              ReservePrice = b.ReservePrice,
                              SubSubCategoriesName = c.SubSubCategoriesName,
                              BuyerFullName = d.FirstName + " " + d.LastName
                          }
                         ).ToListAsync();
        }
        public async Task<ProductsBuyerIntrests> GetProductsBuyerIntrestsByBuyerandPid(int buyerid, int productid)
        {
            var result = await DbContext.ProductsBuyerIntrests.FirstOrDefaultAsync(x =>
                x.BuyerId == buyerid && x.ProductId == productid);
            return result;
        }
    }
}
