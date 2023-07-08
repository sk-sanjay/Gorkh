using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsBuyerIntrestsService
    {
        //Common Methods
        Task<List<ProductsBuyerIntrestsVM>> Get();
        Task<ProductsBuyerIntrestsDTO> Get(int id);
        Task<ProductsBuyerIntrestsDTO> Create(ProductsBuyerIntrestsDTO entity);
        Task<ProductsBuyerIntrestsDTO> Update(ProductsBuyerIntrestsDTO entity);
        Task<int> Delete(int id);

        //Custome Method
        Task<bool> CheckDuplicate(ProductsBuyerIntrestsDTO entity);
        Task<List<ProductsBuyerIntrestsCommonVM>> GetProductsBuyerIntrestsByBuyer(int buyerid);
        Task<List<ProductsBuyerIntrestsCommonVM>> GetProductsBuyerIntrestsForAdmin();
        Task<ProductsBuyerIntrestsDTO> GetProductsBuyerIntrestsByBuyerandPid(int buyerid, int productid);
    }
}
