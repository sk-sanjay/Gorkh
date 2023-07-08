using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IVoltageFrequenciesService
    {
        //Common Methods
        Task<List<VoltageFrequenciesVM>> Get();
        Task<VoltageFrequenciesDTO> Get(int id);
        Task<bool> CheckDuplicate(VoltageFrequenciesDTO entity);
        Task<VoltageFrequenciesDTO> Create(VoltageFrequenciesDTO entity);
        Task<VoltageFrequenciesDTO> Update(VoltageFrequenciesDTO entity);
        Task<int> Delete(int id);
        //Task<List<VoltageFrequenciesDTO>> CreateRange(List<VoltageFrequenciesDTO> entities);
        //Task<List<VoltageFrequenciesDTO>> Upsert(List<VoltageFrequenciesDTO> entities);
        //Task<int> DeleteRange(List<VoltageFrequenciesDTO> entities);
        ////Custom Methods
        //Task<List<DropdownVM>> GetDropdown();
    }
}
