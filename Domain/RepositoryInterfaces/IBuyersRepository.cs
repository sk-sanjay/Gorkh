using Domain.Models;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IBuyersRepository : IRepository<Buyers>
    {
        Task<Buyers> CheckEmail(string name);
        Task<Buyers> GetbyBuyerId(int buyerid);
    }
}
