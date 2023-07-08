using Application.Dtos;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
   public interface IBuyerOffersService
    {
        Task<List<BuyerOffersVM>> Get();
        Task<List<BuyerOffersCommonVM>> GetBuyerOffersForAdmin();
        Task<BuyerOffersVM> Get(int id);
        Task<BuyerOffersDTO> Create(BuyerOffersDTO entity);
        Task<BuyerOffersDTO> Update(BuyerOffersDTO entity);

        Task<BuyerOffersDTO> UpdateBuyerofers(BuyerOffersDTO entity);

        Task<int> Delete(int id);
        Task<bool> CheckDuplicate(BuyerOffersDTO entity);

        Task<List<BuyerOffersDTO>> CreateRange(List<BuyerOffersDTO> entities);
        Task<List<BuyerOffersDTO>> Upsert(List<BuyerOffersDTO> entities);
        Task<int> DeleteRange(List<BuyerOffersDTO> entities);
        //Custom Methods
    }
}
