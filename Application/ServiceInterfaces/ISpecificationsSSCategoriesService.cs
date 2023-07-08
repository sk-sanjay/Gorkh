using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface ISpecificationsSSCategoriesService
    {
        //Common Methods
        Task<List<SpecificationsSSCategoriesVM>> Get();
        Task<SpecificationsSSCategoriesDTO> Get(int id);
        Task<SpecificationsSSCategoriesDTO> Create(SpecificationsSSCategoriesDTO entity);
        Task<SpecificationsSSCategoriesDTO> Update(SpecificationsSSCategoriesDTO entity);
        Task<int> Delete(int id);
        Task<List<SpecificationsSSCategoriesDTO>> CreateRange(List<SpecificationsSSCategoriesDTO> entities);

        Task<int> UpdateSpecificationsSSCategories(List<SpecificationsSSCategoriesDTO> entities);

        //Task<List<StatesDTO>> Upsert(List<StatesDTO> entities);
        //Task<int> DeleteRange(List<StatesDTO> entities);
        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        //Task<List<DropdownVM>> GetDropdownByCountry(int countryid);
        //Task<List<DropdownVM>> GetDropdownBySubcategory(int subcategoryid);

        Task<List<SpecificationsSSCategoriesVM>> GetSpecificationsSSCategories(int subsubcategoryid);
        Task<List<SpecificationsSSCategoriesVM>> GetSpecificationsSSCategoriesjoin(int subsubcategoryid);

    }
}
