using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IOtherLinkRepository : IRepository<OtherLinkHeading>
    {
        Task<OtherLinkHeading> Get(string Menu);
        Task<List<OtherLinkHeading>> GetCategoriesWithAll();
        Task<List<OtherLinkHeading>> GetHeading(string Type);
    }
}
