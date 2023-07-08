using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    class ProductsBuyerFavoritesRepository : Repository<ProductsBuyerFavorites>, IProductsBuyerFavoritesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsBuyerFavoritesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<ProductsBuyerFavorites> CheckDuplicate(ProductsBuyerFavorites model)
        {
            var duplicateModel = DbContext.ProductsBuyerFavorites.FirstOrDefaultAsync(x =>
                  x.ProductId == model.ProductId && x.BuyerId == model.BuyerId);
            return duplicateModel;
        }
        public async Task<List<BuyerFavouriteProductsCommon>> GetFavoutiteProductsbybuyerid(int buyerid)
        {
            return await (from a in DbContext.ProductsBuyerFavorites
                          join b in DbContext.Products on a.ProductId equals b.Id
                          join c in DbContext.SubSubCategories on b.SubSubCatId equals c.Id
                          where a.BuyerId == buyerid
                          orderby a.CreatedDate descending
                          select new BuyerFavouriteProductsCommon
                          {
                              Id = a.Id,
                              ProductId = a.ProductId,
                              BuyerId = a.BuyerId,
                              CreatedDate = System.DateTime.Now,
                              ProductNumber = b.ProductNumber,
                              ReservePrice = b.ReservePrice,
                              SubSubCategoriesName = c.SubSubCategoriesName
                          }
                         ).ToListAsync();
        }
       

    }
}
