using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsVisitorsService
    {
        //Common Methods
        Task<List<ProductsVisitorsVM>> Get();
        Task<ProductsVisitorsDTO> Get(int id);
        Task<ProductsVisitorsDTO> Create(ProductsVisitorsDTO entity);
        Task<ProductsVisitorsDTO> Update(ProductsVisitorsDTO entity);
        Task<int> Delete(int id);
    }
}
