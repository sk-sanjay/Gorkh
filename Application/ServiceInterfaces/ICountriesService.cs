using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface ICountriesService
    {
        //Common Methods
        Task<List<CountriesVM>> Get();
        //Task<List<CountriesVM>> GetCountrybystateid(int stateid);
        Task<CountriesDTO> Get(int id);
        Task<bool> CheckDuplicate(CountriesDTO argModelDto);
        Task<CountriesDTO> Create(CountriesDTO entity);
        Task<CountriesDTO> Update(CountriesDTO entity);
        Task<int> Delete(int id);
        Task<List<CountriesDTO>> CreateRange(List<CountriesDTO> entities);
        Task<List<CountriesDTO>> Upsert(List<CountriesDTO> entities);
        Task<int> DeleteRange(List<CountriesDTO> entities);
        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
        Task<List<DropdownVM>> GetCountryByProdcutWise();
    }
}
