using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IBannersRepository : IRepository<Banners>
    {
        Task<List<Banners>> GetBannersForHomeSlider();
    }
}
