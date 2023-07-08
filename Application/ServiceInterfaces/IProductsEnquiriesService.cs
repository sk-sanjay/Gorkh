using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsEnquiriesService
    {
        //Common Methods
        Task<List<ProductsEnquiriesVM>> Get();
        Task<ProductsEnquiriesDTO> Get(int id);
        Task<ProductsEnquiriesDTO> Create(ProductsEnquiriesDTO entity);
        Task<ProductsEnquiriesDTO> Update(ProductsEnquiriesDTO entity);
        Task<int> Delete(int id);
        //Custome Method
        Task<List<ProductsEnquiriesVM>> GetProductsEnquiriesByPid(int ProductId);
    }
}
