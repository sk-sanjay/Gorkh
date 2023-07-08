using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ISubCategoriesService
    {
        Task<List<SubCategoriesVM>> Get();
        Task<SubCategoriesVM> Get(int id);
        Task<bool> CheckDuplicate(SubCategoriesDTO argModelDto);
        Task<SubCategoriesDTO> Create(SubCategoriesDTO entity);
        Task<SubCategoriesDTO> Update(SubCategoriesDTO entity);
        Task<int> Delete(int id);
        Task<List<SubCategoriesDTO>> CreateRange(List<SubCategoriesDTO> entities);
        Task<List<SubCategoriesDTO>> Upsert(List<SubCategoriesDTO> entities);
        Task<int> DeleteRange(List<SubCategoriesDTO> entities);

        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        Task<List<DropdownVM>> GetDropdownByCategory(int categoryid);
        Task<List<SubCategoriesVM>> GetSubcategory(int maincat);
        //Task<List<SubCategoriesVM>> GetCategory();
    }
}
