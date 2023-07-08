using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface ISpecificationsSSCategoriesRepository : IRepository<SpecificationsSSCategories>
    {

        //Get Specification Category by sub sub category wise
        Task<List<SpecificationsSSCategories>> GetSpecificationsSSCategories(int subsubcategoryid);
        Task<List<SpecificationsSSCategories>> GetSpecificationsSSCategoriesjoin(int subsubcategoryid);
    }

}
