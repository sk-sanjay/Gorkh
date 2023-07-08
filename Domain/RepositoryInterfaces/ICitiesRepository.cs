using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface ICitiesRepository : IRepository<Cities>
    {
        Task<List<Cities>> GetCitybystate(int sid);
        Task<Cities> CheckDuplicate(Cities model);
    }
}
