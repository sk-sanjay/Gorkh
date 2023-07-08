using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   public class TempOtherLinkRepository : Repository<TempOtherLinkHeading>, ITempOtherLinkRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public TempOtherLinkRepository(AppDbContext context) : base(context)
        {

        }
        public void CreateAudit(TempOtherLinkHeading model)
        {
            DbContext.TempOtherLinkHeading.Add(model);
        }
        public Task<List<TempOtherLinkHeading>> GetByAction(string status)
        {
            return DbContext.TempOtherLinkHeading.Where(c => c.Status == status).OrderByDescending(c => c.Id).ToListAsync();
            
        }
    }
}
