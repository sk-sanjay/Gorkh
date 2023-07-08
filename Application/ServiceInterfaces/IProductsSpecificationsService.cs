using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IProductsSpecificationsService
    {
        //Common Methods
        Task<List<ProductsSpecificationsVM>> Get();
        Task<ProductsSpecificationsDTO> Get(int id);
        Task<ProductsSpecificationsDTO> Create(ProductsSpecificationsDTO entity);
        Task<ProductsSpecificationsDTO> Update(ProductsSpecificationsDTO entity);
        Task<int> Delete(int id);
    }
}
