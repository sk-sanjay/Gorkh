using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IShippingWeightsService
    {
        //Common Methods
        Task<List<ShippingWeightsVM>> Get();
        Task<ShippingWeightsDTO> Get(int id);
        Task<bool> CheckDuplicate(ShippingWeightsDTO entity);
        Task<ShippingWeightsDTO> Create(ShippingWeightsDTO entity);
        Task<ShippingWeightsDTO> Update(ShippingWeightsDTO entity);
        Task<int> Delete(int id);
        //Task<List<ShippingWeightsDTO>> CreateRange(List<ShippingWeightsDTO> entities);
        //Task<List<ShippingWeightsDTO>> Upsert(List<ShippingWeightsDTO> entities);
        //Task<int> DeleteRange(List<ShippingWeightsDTO> entities);
        ////Custom Methods
        //Task<List<DropdownVM>> GetDropdown();
    }
}
