using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ICategoriesRepository : IRepository<Categories>
    {
        Task<Categories> CheckDuplicate(Categories model);
        Task<List<Categories>> GetAllCategoryWithChild();
    }
}
