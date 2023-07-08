using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
   public interface IBuyerOffersRepository : IRepository<BuyerOffers>
    {
        Task<BuyerOffers> CheckDuplicate(BuyerOffers model);
        Task<List<BuyerOffersCommon>> GetBuyerOffersForAdmin();
        Task<List<BuyerOffers>> GetBuyerOffersByProductNumber(string productno);
    }
}
