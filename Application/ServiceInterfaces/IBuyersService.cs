using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IBuyersService
    {
        Task<List<BuyersVM>> Get();
        Task<BuyersVM> Get(int id);
        Task<BuyersDTO> Create(BuyersDTO entity);
        Task<bool> CheckEmail(string email);
        Task<BuyersVM> GetbyBuyerId(int buyerid);

    }
}
