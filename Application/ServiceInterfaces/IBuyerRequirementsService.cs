using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IBuyerRequirementsService
    {
        Task<List<BuyerRequirementsVM>> Get();
        Task<List<BuyerRequirementsVM1>> GetBuyerRequirements();
        Task<List<BuyerRequirementsVM1>> GetBuyerRequirementsforWebsite();
        Task<BuyerRequirementsVM1> GetBuyerRequirements(int id);
        Task<List<BuyerRequirementsVM1>> GetBuyerRequirementsbyusername(string email);
        Task<BuyerRequirementsVM> Get(int id);
        //Task<bool> CheckDuplicate(BuyerRequirementsDTO argModelDto);
        Task<BuyerRequirementsDTO> Create(BuyerRequirementsDTO entity);
        Task<BuyerRequirementsDTO> Update(BuyerRequirementsDTO entity);
        Task<int> Delete(int id);

        Task<List<BuyerRequirementsDTO>> CreateRange(List<BuyerRequirementsDTO> entities);
        Task<List<BuyerRequirementsDTO>> Upsert(List<BuyerRequirementsDTO> entities);
        Task<int> DeleteRange(List<BuyerRequirementsDTO> entities);
    }
}
