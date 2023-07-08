using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsBuyerQueriesService
    {
        //Common Methods
        Task<List<ProductsBuyerQueriesVM>> Get();
        Task<ProductsBuyerQueriesDTO> Get(int id);
        Task<ProductsBuyerQueriesDTO> Create(ProductsBuyerQueriesDTO entity);
        Task<ProductsBuyerQueriesDTO> Update(ProductsBuyerQueriesDTO entity);
        Task<int> Delete(int id);

        //Custome Method
        Task<List<ProductsBuyerQueriesCommonVM>> GetProductsByBuyer(int buyerid);
        Task<List<ProductsBuyerQueriesCommonVM>> GetProductsBuyerQueriesByPid(int ProductId, int buyerid);
        Task<List<ProductsBuyerQueriesCommonVM>> GetProductsBuyerQueriesForAdmin();
        Task<ProductsBuyerQueriesCommonVM> GetProductsBuyerQueriesById(int id);
        Task<ProductsBuyerQueriesCommonVM> GetProductsDetailsById(int ProductId);
    }
}
