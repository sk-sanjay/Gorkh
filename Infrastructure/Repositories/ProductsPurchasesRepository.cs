using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    class ProductsPurchasesRepository : Repository<ProductsPurchases>, IProductsPurchasesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsPurchasesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<ProductsPurchases> CheckDuplicate(ProductsPurchases model)
        {
            var duplicateModel = DbContext.ProductsPurchases.FirstOrDefaultAsync(x =>
                  x.ProductId == model.ProductId && x.BuyerId == model.BuyerId);
            return duplicateModel;
        }
        public async Task<List<ProductsPurchasesCommon>> GetProductsPurchasesByBuyer(int buyerid)
        {
            return await (from a in DbContext.ProductsPurchases
                          join b in DbContext.Products on a.ProductId equals b.Id
                          //join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                          where a.BuyerId== buyerid
                          select new ProductsPurchasesCommon
                          { 
                              Id=a.Id,
                              ProductId=a.ProductId,
                              BuyerId=a.BuyerId,
                              CreatedDate=a.CreatedDate,
                              ProductNumber=b.ProductNumber,
                              ReservePrice=b.ReservePrice,
                              SubSubCategory=b.SubSubCategory
                          }
                          ).ToListAsync();

        }
        public async Task<List<ProductsPurchasesCommon>> GetProductsPurchasesForAdmin()
        {
            var aa = await (from a in DbContext.ProductsPurchases
                            join b in DbContext.Products on a.ProductId equals b.Id
                            join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                            orderby a.CreatedDate descending
                            select new ProductsPurchasesCommon
                            {
                                Id = a.Id,
                                ProductId = a.ProductId,
                                BuyerId = a.BuyerId,
                                CreatedDate = a.CreatedDate,
                                ProductNumber = b.ProductNumber,
                                ReservePrice = b.ReservePrice,
                                SubSubCategory = b.SubSubCategory,
                                BuyerFullName = c.FirstName + ' ' + c.LastName
                            }
                    ).ToListAsync();

            return aa;

        }
        public async Task<ProductsPurchases> GetProductsPurchasesByBuyerandPid(int buyerid, int productid)
        {
            var result = await DbContext.ProductsPurchases.FirstOrDefaultAsync(x =>
                x.BuyerId == buyerid && x.ProductId == productid);
            return result;
        }
    }
}
