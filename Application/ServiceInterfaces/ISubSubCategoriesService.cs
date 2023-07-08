using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ISubSubCategoriesService
    {
        Task<List<SubSubCategoriesVM>> Get();
        Task<SubSubCategoriesVM> Get(int id);
        Task<bool> CheckDuplicate(SubSubCategoriesDTO argModelDto);
        Task<SubSubCategoriesDTO> Create(SubSubCategoriesDTO entity);
        Task<SubSubCategoriesDTO> Update(SubSubCategoriesDTO entity);
        Task<int> Delete(int id);
        Task<List<SubSubCategoriesDTO>> CreateRange(List<SubSubCategoriesDTO> entities);
        Task<List<SubSubCategoriesDTO>> Upsert(List<SubSubCategoriesDTO> entities);
        Task<int> DeleteRange(List<SubSubCategoriesDTO> entities);


        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        Task<List<DropdownVM>> GetDropdownByCategory(int categoryid);

        Task<List<SubSubCategoriesVM>> GetSubcategory(int maincat);
        //Task<List<SubCategoriesVM>> GetCategory();
        Task<List<SubSubCategoriesVM>> GetSubSubCategoryBySubCategory2(int subcategoryid);
        Task<List<SubSubCategoriesVM>> SearchSubSubCategory(string prefix);

    }
}
