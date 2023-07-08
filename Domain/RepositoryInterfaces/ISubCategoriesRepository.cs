using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ISubCategoriesRepository : IRepository<SubCategories>
    {
        Task<List<SubCategories>> GetSubCategoryByCategory(int categoryid);
        Task<List<SubCategories>> GetSubcategory(int maincat);
        Task<SubCategories> CheckDuplicate(SubCategories model);
        //Task<List<SubCategories>> GetCategory();

    }
}
