using Domain.Models;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ISellerRegistrationsRepository : IRepository<SellerRegistrations>
    {
        Task<SellerRegistrations> CheckEmail(string name);
        Task<SellerRegistrations> GetbyEmail(string email);
        Task<SellerRegistrations> GetbySellerId(int sellerid);

    }
}
