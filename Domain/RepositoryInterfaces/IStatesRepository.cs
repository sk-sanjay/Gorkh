using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IStatesRepository : IRepository<States>
    {
        Task<List<States>> GetStatesByCountry(int countryid);
        Task<States> CheckDuplicate(States model);
        Task<List<States>> GetState(int cid);
        //Task<List<SubCategories>> GetSubcategory(int maincat);
        Task<List<States>> GetStatesByProdcutWise(int countryid);
    }
}
