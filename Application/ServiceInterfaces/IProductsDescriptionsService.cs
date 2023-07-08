using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsDescriptionsService
    {
        //Common Methods
        Task<List<ProductsDescriptionsVM>> Get();
        Task<ProductsDescriptionsDTO> Get(int id);
        Task<ProductsDescriptionsDTO> Create(ProductsDescriptionsDTO entity);
        Task<ProductsDescriptionsDTO> Update(ProductsDescriptionsDTO entity);
        Task<ProductsDescriptionsDTO> GetByProductId(int id);
        Task<int> Delete(int id);

        //Get Product Description by product id
        Task<ProductsDescriptionsDTO> GetProductsDescriptionsByProductId(int productid);
    }
}
