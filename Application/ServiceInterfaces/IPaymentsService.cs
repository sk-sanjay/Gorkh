using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IPaymentsService
    {
        //Common Methods
        Task<List<PaymentsVM>> Get();
        Task<PaymentsDTO> Get(int id);
        Task<PaymentsDTO> Create(PaymentsDTO entity);
        Task<PaymentsDTO> Update(PaymentsDTO entity);
        Task<int> Delete(int id);
        //Custome Method
        Task<bool> CheckDuplicate(PaymentsDTO entity);
        Task<List<PaymentsCommonVM>> GetPaymentsForAdmin();
        Task<int> UpdatePaymentStatus(PaymentsUpdateDTO modelDto);
        Task<PaymentsDTO> GetProductsPaymentsByBuyerandPid(int buyerid, int productid);
        Task<List<PaymentsCommonVM>> GetProductsPaymentsByBuyer(int buyerid);
        Task<List<PaymentsCommonVM>> GetProductsPaymentsForAdmin();
        Task<List<PaymentsCommonVM>> GetPaymentsByProductIdForAdmin(int productid);
        Task<List<PaymentsCommonVM>> GetProductsPaymentsForSeller(int sellerid);
        Task<List<PaymentsCommonVM>> GetPaymentsByProductIdForSeller(int productid);
    }
}
