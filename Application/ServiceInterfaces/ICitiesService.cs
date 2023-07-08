using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ICitiesService
    {
        Task<List<CitiesVM>> Get();
        Task<CitiesVM> Get(int id);
        Task<bool> CheckDuplicate(CitiesDTO argModelDto);
        Task<CitiesDTO> Create(CitiesDTO entity);
        Task<CitiesDTO> Update(CitiesDTO entity);
        Task<int> Delete(int id);
        Task<List<CitiesDTO>> CreateRange(List<CitiesDTO> entities);
        Task<List<CitiesDTO>> Upsert(List<CitiesDTO> entities);
        Task<int> DeleteRange(List<CitiesDTO> entities);
        Task<List<CitiesVM>> GetCitybystate(int sid);

        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        //Task<List<DropdownVM>> GetDropdownByCategory(int categoryid);

        //Task<List<SubSubCategoriesVM>> GetSubcategory(int maincat);
        //Task<List<SubCategoriesVM>> GetCategory();


    }
}
