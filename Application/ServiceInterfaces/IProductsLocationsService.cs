using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsLocationsService
    {
        //Common Methods
        Task<List<ProductsLocationsVM>> Get();
        Task<ProductsLocationsDTO> Get(int id);
        Task<ProductsLocationsDTO> Create(ProductsLocationsDTO entity);
        Task<ProductsLocationsDTO> Update(ProductsLocationsDTO entity);
        Task<ProductsLocationsDTO> GetByProductId(int id);
        Task<int> Delete(int id);
    }
}
