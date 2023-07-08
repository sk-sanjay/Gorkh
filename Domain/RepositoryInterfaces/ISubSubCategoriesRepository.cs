using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ISubSubCategoriesRepository : IRepository<SubSubCategories>
    {
        Task<SubSubCategories> CheckDuplicate(SubSubCategories model);
        Task<List<SubSubCategories>> GetSubSubCategoryBySubCategory(int subcategoryid);
        Task<List<SubSubCategories>> GetSubcategory(int maincat);
        Task<List<SubSubCategories>> GetSubSubCategoryBySubCategory2(int subcategoryid);
        Task<List<SubSubCategories>> SearchSubSubCategory(string prefix);

    }
}
