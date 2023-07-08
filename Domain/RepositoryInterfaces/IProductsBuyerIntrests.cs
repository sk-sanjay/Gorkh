using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsBuyerIntrestsRepository : IRepository<ProductsBuyerIntrests>
    {
        Task<ProductsBuyerIntrests> CheckDuplicate(ProductsBuyerIntrests model);
        Task<List<ProductsBuyerIntrestsCommon>> GetProductsBuyerIntrestsByBuyer(int buyerid);
        Task<List<ProductsBuyerIntrestsCommon>> GetProductsBuyerIntrestsForAdmin();
        Task<ProductsBuyerIntrests> GetProductsBuyerIntrestsByBuyerandPid(int buyerid,int productid);
    }
}
