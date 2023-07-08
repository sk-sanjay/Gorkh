using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface ISpecificationsRepository : IRepository<Specifications>
    {
        Task<Specifications> CheckDuplicate(Specifications model);
        Task<List<Specifications>> GetByOrder();
    }
}
