using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface ITempOtherLinkRepository : IRepository<TempOtherLinkHeading>
    {
        void CreateAudit(TempOtherLinkHeading model);
        Task<List<TempOtherLinkHeading>> GetByAction(string status);
    }
}
