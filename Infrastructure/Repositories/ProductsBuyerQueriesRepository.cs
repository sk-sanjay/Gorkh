using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductsBuyerQueriesRepository : Repository<ProductsBuyerQueries>, IProductsBuyerQueriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsBuyerQueriesRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<ProductsBuyerQueriesCommon>> GetProductsByBuyer(int buyerid)
        {
            var ProductsBuyerQuery = (from a in DbContext.ProductsBuyerQueries
                                      where a.BuyerId == buyerid
                                      group a by new
                                      {
                                          a.ProductId,
                                          a.BuyerId
                                      } into gcs
                                      select new
                                      {
                                          productid = gcs.Key.ProductId,
                                          buyerid = gcs.Key.BuyerId
                                      });


            var aa = await (from a in DbContext.Products
                            join b in ProductsBuyerQuery on a.Id equals b.productid
                            select new ProductsBuyerQueriesCommon
                            {
                                Id = a.Id,
                                ProductId=a.Id,
                                ProductNumber = a.ProductNumber,
                                SubSubCategory = a.SubSubCategory
                            }
                    ).ToListAsync();

            //return await (from a in DbContext.Products where a.SellerId == sellerid orderby a.Id descending select a).ToListAsync();
            //var countv = (from a in DbContext.ProductsVisitors
            //              group a by a.ProductId into ag
            //              select new
            //              {
            //                  productid = ag.Key,
            //                  cnt = ag.Count()
            //              });

            //var countInterest = (from a in DbContext.ProductsBuyerIntrests
            //                     group a by a.ProductId into Ints
            //                     select new
            //                     {
            //                         productid = Ints.Key,
            //                         picount = Ints.Count()
            //                     });

            //var aa = await (from a in DbContext.Products
            //                join b in countv on a.Id equals b.productid into cv
            //                from x in cv.DefaultIfEmpty()
            //                join c in countInterest on a.Id equals c.productid into cv1
            //                from c1 in cv1.DefaultIfEmpty()
            //                where a.SellerId == sellerid
            //                orderby a.Id descending
            //                select new ProductsBySeller
            //                {
            //                    Id = a.Id,
            //                    SaleType = a.SellerType,
            //                    ReservePrice = a.ReservePrice,
            //                    IsApprove = a.IsApprove,
            //                    SubSubCategory = a.SubSubCategory,
            //                    totalVisitor = x.cnt,
            //                    //totalVisitor = (x == null ? 0 : x.cnt)
            //                    TotalInterest = c1.picount

            //                }
            //              ).ToListAsync();
            return aa;

        }
        public async Task<List<ProductsBuyerQueriesCommon>> GetProductsBuyerQueriesByPid(int ProductId, int buyerid)
        {
            return await (from a in DbContext.ProductsBuyerQueries
                          where a.ProductId == ProductId && a.BuyerId == buyerid
                          orderby a.Id descending
                          select new ProductsBuyerQueriesCommon
                          {
                              Id = a.Id,
                              ProductId = a.ProductId,
                              Descriptions = a.Descriptions,
                              EnquiryFile = a.EnquiryFile,
                              CreatedDate = a.CreatedDate,
                              CreatedBy = a.CreatedBy

                          }
                          ).ToListAsync();

        }
        public async Task<List<ProductsBuyerQueriesCommon>> GetProductsBuyerQueriesForAdmin()
        {
            var aa = await (from a in DbContext.ProductsBuyerQueries
                            join b in DbContext.Products on a.ProductId equals b.Id
                            join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                            orderby a.CreatedDate descending
                            select new ProductsBuyerQueriesCommon
                            {
                                Id = a.Id,
                                CreatedDate=a.CreatedDate,
                                ProductId = a.ProductId,
                                ProductNumber = b.ProductNumber,
                                SubSubCategory = b.SubSubCategory,
                                BuyerFullName = c.FirstName + ' ' + c.LastName
                            }
                    ).ToListAsync();

            return aa;

        }
        public async Task<ProductsBuyerQueriesCommon> GetProductsBuyerQueriesById(int id)
        {
            return await (from a in DbContext.ProductsBuyerQueries
                          join b in DbContext.Products on a.ProductId equals b.Id
                          join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                          where a.Id == id
                          select new ProductsBuyerQueriesCommon
                          {
                              Id = a.Id,
                              ProductId = a.ProductId,
                              ProductNumber = b.ProductNumber,
                              SubSubCategory = b.SubSubCategory,
                              BuyerFullName = c.FirstName + ' ' + c.LastName

                          }
                          ).FirstOrDefaultAsync();

        }
        public async Task<ProductsBuyerQueriesCommon> GetProductsDetailsById(int ProductId)
        {
            return await (from a in DbContext.Products
                          where a.Id == ProductId
                          select new ProductsBuyerQueriesCommon
                          {
                              ProductNumber = a.ProductNumber,
                              SubSubCategory = a.SubSubCategory

                          }
                          ).FirstOrDefaultAsync();

        }
    }
}
