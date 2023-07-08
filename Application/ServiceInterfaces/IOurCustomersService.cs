using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
   public interface IOurCustomersService
    {
        //Common Methods
        Task<List<OurCustomersVM>> Get();
        Task<OurCustomersDTO> Get(int id);
        Task<OurCustomersDTO> Create(OurCustomersDTO entity);
        Task<OurCustomersDTO> Update(OurCustomersDTO entity);
        Task<int> Delete(int id);
        Task<List<OurCustomersVM>> GetOurCustomersHomePage();
    }
}
