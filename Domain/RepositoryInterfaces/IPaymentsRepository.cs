using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IPaymentsRepository : IRepository<Payments>
    {
        Task<Payments> CheckDuplicate(Payments model);
        Task<List<PaymentsCommon>> GetPaymentsForAdmin();
        Task<Payments> GetProductsPaymentsByBuyerandPid(int buyerid, int productid);
        Task<List<PaymentsCommon>> GetProductsPaymentsByBuyer(int buyerid);
        Task<List<PaymentsCommon>> GetProductsPaymentsForAdmin();
        Task<List<PaymentsCommon>> GetPaymentsByProductIdForAdmin(int productid);
        Task<List<PaymentsCommon>> GetProductsPaymentsForSeller(int sellerid);
        Task<List<PaymentsCommon>> GetPaymentsByProductIdForSeller(int productid);
    }
}
