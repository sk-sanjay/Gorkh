using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IManufacturersService
    {
        //Common Methods
        Task<List<ManufacturersVM>> Get();
        Task<ManufacturersDTO> Get(int id);
        Task<bool> CheckDuplicate(ManufacturersDTO entity);
        Task<ManufacturersDTO> Create(ManufacturersDTO entity);
        Task<ManufacturersDTO> Update(ManufacturersDTO entity);
        Task<int> Delete(int id);
        //Task<List<ManufacturersDTO>> CreateRange(List<ManufacturersDTO> entities);
        //Task<List<ManufacturersDTO>> Upsert(List<ManufacturersDTO> entities);
        //Task<int> DeleteRange(List<ManufacturersDTO> entities);
        //Custom Methods
        Task<List<DropdownVM>> GetDropdown();
    }
}
