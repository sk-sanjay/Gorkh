using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Domain.RepositoryInterfaces
{
    public interface IProductsPurchasesRepository : IRepository<ProductsPurchases>
    {
        Task<ProductsPurchases> CheckDuplicate(ProductsPurchases model);
        Task<List<ProductsPurchasesCommon>> GetProductsPurchasesByBuyer(int buyerid);
        Task<List<ProductsPurchasesCommon>> GetProductsPurchasesForAdmin();
        Task<ProductsPurchases> GetProductsPurchasesByBuyerandPid(int buyerid, int productid);
    }
}
