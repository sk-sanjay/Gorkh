using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsBuyerQueriesRepository : IRepository<ProductsBuyerQueries>
    {
        Task<List<ProductsBuyerQueriesCommon>> GetProductsByBuyer(int buyerid);
        Task<List<ProductsBuyerQueriesCommon>> GetProductsBuyerQueriesByPid(int ProductId, int buyerid);
        Task<List<ProductsBuyerQueriesCommon>> GetProductsBuyerQueriesForAdmin();
        Task<ProductsBuyerQueriesCommon> GetProductsBuyerQueriesById(int id);  //Get Product and buyer details
        Task<ProductsBuyerQueriesCommon> GetProductsDetailsById(int ProductId);  //Get Product details
    }
}
