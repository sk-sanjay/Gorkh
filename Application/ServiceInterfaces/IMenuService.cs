using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IMenuService
    {
        //Common Methods
        Task<List<MenuVM>> Get();
        Task<MenuDTO> Get(int id);
        Task<MenuDTO> Create(MenuDTO argModelDto);
        Task<MenuDTO> Update(MenuDTO argModelDto);
        Task<int> Delete(int id);
        //Custom Methods
        Task<List<MenuVM>> GetWithAll();
        Task<List<MenuVM>> GetAllByRole(string role);
    }
}
