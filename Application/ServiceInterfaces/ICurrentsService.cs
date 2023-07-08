using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface ICurrentsService
    {
        //Common Methods
        Task<List<CurrentsVM>> Get();
        Task<CurrentsDTO> Get(int id);
        Task<bool> CheckDuplicate(CurrentsDTO entity);
        Task<CurrentsDTO> Create(CurrentsDTO entity);
        Task<CurrentsDTO> Update(CurrentsDTO entity);
        Task<int> Delete(int id);
        //Task<List<CurrentsDTO>> CreateRange(List<CurrentsDTO> entities);
        //Task<List<CurrentsDTO>> Upsert(List<CurrentsDTO> entities);
        //Task<int> DeleteRange(List<CurrentsDTO> entities);
        ////Custom Methods
        //Task<List<DropdownVM>> GetDropdown();
    }
}
