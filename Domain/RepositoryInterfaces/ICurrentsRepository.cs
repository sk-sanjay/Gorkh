using Domain.Models;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface ICurrentsRepository : IRepository<Currents>
    {
        Task<Currents> CheckDuplicate(Currents model);
    }
}
