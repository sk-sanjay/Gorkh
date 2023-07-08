using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IBuyerSellerRegistrationsService
    {
        Task<List<BuyerSellerRegistrationsVM>> Get();
        Task<List<BuyerSellerRegistrations1VM>> GetBuyersandSellers();
        Task<List<BuyerSellerRegistrations1VM>> GetBuyersData();
        Task<List<BuyerSellerRegistrations1VM>> GetSellerData();
        Task<List<BuyerSellerRegistrations1VM>> GetBothData();
        Task<BuyerSellerRegistrationsVM> Get(int id);
        Task<BuyerSellerRegistrationsVM> GetbyEmail(string email);
        Task<BuyerSellerRegistrationsVM> GetbyMobile(string mobile);
        
        Task<BuyerSellerRegistrationsVM> GetbySellerId(int sellerid);
        Task<BuyerSellerRegistrationsVM> GetbyBuyerId(int buyerid);
        Task<List<BuyerSellerRegistrationsVM>> GetdropdownbySellerId(int orgid);
        //Task<BuyerSellerRegistrationsVM> CheckEmail(string email);
        Task<BuyerSellerRegistrationsDTO> Create(BuyerSellerRegistrationsDTO entity);
        Task<int> UpdateOrganisationType(BuyerCommonDTO modelDto);
        Task<int> UpdateDetails(SellerCommonDTO modelDto);
        
        Task<BuyerSellerRegistrationsDTO> UpdateBuyerDetails(BuyerSellerRegistrationsDTO modelDto);
        Task<BuyerSellerRegistrationsDTO> UpdateSellerProfile(BuyerSellerRegistrationsDTO modelDto);
        
        Task<BuyerCommonDTO> Update(BuyerCommonDTO modelDto);
        Task<bool> CheckDuplicate(BuyerSellerRegistrationsDTO argModelDto);
    }
}
