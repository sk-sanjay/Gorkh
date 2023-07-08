using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface ICountriesRepository : IRepository<Countries>
    {
        Task<Countries> CheckDuplicate(Countries model);
        //Task<List<Countries>> GetCountrybystateid(int stateid);
        Task<List<Countries>> GetCountryByProdcutWise();
    }
}
