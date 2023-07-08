using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public class BuyerOffersRepository : Repository<BuyerOffers>, IBuyerOffersRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyerOffersRepository(AppDbContext context) : base(context)
        {
        }
        public Task<BuyerOffers> CheckDuplicate(BuyerOffers model)
        {
            var duplicateModel = DbContext.BuyerOffers.FirstOrDefaultAsync(x =>
                  x.ProductNumber == model.ProductNumber && x.BuyerId == model.BuyerId);
            return duplicateModel;
        }
        public async Task<List<BuyerOffersCommon>> GetBuyerOffersForAdmin()
        {
            var aa = await (from a in DbContext.BuyerOffers
                            join b in DbContext.BuyerSellerRegistrations on a.BuyerId equals b.Id
                            orderby a.CreatedDate descending
                            select new BuyerOffersCommon
                            {
                                Id = a.Id,
                                BuyerName = b.FirstName + ' ' + b.LastName,
                                BuyerEmailId = b.Email,
                                ProductNumber = a.ProductNumber,
                                EstimatePrice = a.EstimatePrice,
                                OfferdPrice = a.OfferdPrice,
                                CreatedDate = a.CreatedDate,
                                ModifiedDate = a.ModifiedDate,
                                Status = a.Status,
                                BuyerId = a.BuyerId,
                                IsSoled= a.IsSoled



                            }
                    ).ToListAsync();

            return aa;

        }

        public async Task<List<BuyerOffers>> GetBuyerOffersByProductNumber(string productnumber)
        {
            var aa = await (from a in DbContext.BuyerOffers
                            join b in DbContext.BuyerSellerRegistrations on a.BuyerId equals b.Id
                            where a.ProductNumber == productnumber                            
                            select new BuyerOffers
                            {
                                Id = a.Id,
                                ProductNumber = a.ProductNumber,
                                EstimatePrice = a.EstimatePrice,
                                OfferdPrice = a.OfferdPrice,
                                CreatedDate = a.CreatedDate,
                                ModifiedDate = a.ModifiedDate,
                                Status = a.Status,
                                BuyerId = a.BuyerId,
                                IsSoled = a.IsSoled



                            }
                    ).ToListAsync();

            return aa;

        }
    }
}
