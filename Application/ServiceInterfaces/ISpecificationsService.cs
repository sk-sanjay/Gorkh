using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface ISpecificationsService
    {
        //Common Methods
        Task<List<SpecificationsVM>> Get();
        Task<List<SpecificationsVM>> GetByOrder();
        Task<SpecificationsDTO> Get(int id);
        Task<bool> CheckDuplicate(SpecificationsDTO entity);
        Task<SpecificationsDTO> Create(SpecificationsDTO entity);
        Task<SpecificationsDTO> Update(SpecificationsDTO entity);
        Task<int> Delete(int id);
        //Task<List<SpecificationsDTO>> CreateRange(List<SpecificationsDTO> entities);
        //Task<List<SpecificationsDTO>> Upsert(List<SpecificationsDTO> entities);
        //Task<int> DeleteRange(List<SpecificationsDTO> entities);
        ////Custom Methods
        //Task<List<DropdownVM>> GetDropdown();

    }
}
