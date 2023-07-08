using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ICategoriesService
    {
        Task<List<CategoriesVM>> Get();
        Task<CategoriesVM> Get(int id);
        Task<bool> CheckDuplicate(CategoriesDTO argModelDto);
        Task<CategoriesDTO> Create(CategoriesDTO entity);
        Task<CategoriesDTO> Update(CategoriesDTO entity);
        Task<int> Delete(int id);

        Task<List<CategoriesDTO>> CreateRange(List<CategoriesDTO> entities);
        Task<List<CategoriesDTO>> Upsert(List<CategoriesDTO> entities);
        Task<int> DeleteRange(List<CategoriesDTO> entities);
        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        Task<List<CategoriesVM>> GetAllCategoryWithChild();
    }
}
