using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IBuyerSellerRegistrationsRepository : IRepository<BuyerSellerRegistrations>
    {
        Task<BuyerSellerRegistrations> CheckEmail(string name);
        Task<BuyerSellerRegistrations> GetbyEmail(string email);
        Task<BuyerSellerRegistrations> GetbyMobile(string mobile);
        
        Task<BuyerSellerRegistrations> GetbySellerId(int sellerid);
        Task<BuyerSellerRegistrations> GetbyBuyerId(int buyerid);
        Task<List<BuyerSellerRegistrations>> GetdropdownbySellerId(int orgid);
        Task<BuyerSellerRegistrations> CheckDuplicate(BuyerSellerRegistrations model);
      
    }
}
