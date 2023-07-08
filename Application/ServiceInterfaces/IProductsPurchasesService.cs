using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsPurchasesService
    {
        //Common Methods
        Task<List<ProductsPurchasesVM>> Get();
        Task<ProductsPurchasesDTO> Get(int id);
        Task<ProductsPurchasesDTO> Create(ProductsPurchasesDTO entity);
        Task<ProductsPurchasesDTO> Update(ProductsPurchasesDTO entity);
        Task<int> Delete(int id);

        //Custome Method
        Task<bool> CheckDuplicate(ProductsPurchasesDTO entity);
        Task<List<ProductsPurchasesCommonVM>> GetProductsPurchasesByBuyer(int buyerid);
        Task<List<ProductsPurchasesCommonVM>> GetProductsPurchasesForAdmin();
        Task<ProductsPurchasesDTO> GetProductsPurchasesByBuyerandPid(int buyerid, int productid);
    }
}
