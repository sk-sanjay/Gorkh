using Domain.Models;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IManufacturersRepository : IRepository<Manufacturers>
    {
        Task<Manufacturers> CheckDuplicate(Manufacturers model);
    }
}
