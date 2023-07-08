using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface ISellerRegistrationsService
    {
        Task<List<SellerRegistrationsVM>> Get();
        Task<SellerRegistrationsVM> Get(int id);
        Task<SellerRegistrationsVM> GetbyEmail(string email);
        Task<SellerRegistrationsVM> GetbySellerId(int sellerid);
        Task<bool> CheckEmail(string email);
        Task<SellerRegistrationsDTO> Create(SellerRegistrationsDTO entity);

    }
}
