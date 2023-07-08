using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   public class OtherLinkRepository : Repository<OtherLinkHeading>, IOtherLinkRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public OtherLinkRepository(AppDbContext context) : base(context)
        {

        }
        public Task<OtherLinkHeading> Get(string Menu)
        {
            return DbContext.OtherLinkHeading.FirstOrDefaultAsync(a => a.EnglishHeadingName.ToLower() == Menu);
        }
        public Task<List<OtherLinkHeading>> GetHeading(string Type)
        {
            var Result = DbContext.OtherLinkHeading.Where(x => x.ParentId == 62).ToListAsync();
            return Result;
        }
        public Task<List<OtherLinkHeading>> GetCategoriesWithAll()
        {
            return DbContext.OtherLinkHeading.Where(c => c.Show && c.ParentId == null)
                .OrderBy(c => c.EnglishHeadingName).ToListAsync();
        }
    }
}
