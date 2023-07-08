using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IMenuRepository : IRepository<Menus>
    {
        Task<List<Menus>> GetWithAll();

        //NotInUse
        //Task<List<Menus>> GetAllByRole(string role);
    }
}
