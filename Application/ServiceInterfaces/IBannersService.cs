using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IBannersService
    {
        //Common Methods
        Task<List<BannersVM>> Get();
        Task<BannersDTO> Get(int id);
        Task<BannersDTO> Create(BannersDTO entity);
        Task<BannersDTO> Update(BannersDTO entity);
        Task<int> Delete(int id);

        //Custom Method
        Task<List<BannersVM>> GetBannersForHomeSlider();
    }
}
