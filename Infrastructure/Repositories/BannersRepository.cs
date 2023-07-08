using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Infrastructure.Repositories
{
    public class BannersRepository : Repository<Banners>, IBannersRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BannersRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<Banners>> GetBannersForHomeSlider()
        {
            //return await (from a in DbContext.Banners where a.IsActive == true select a).ToListAsync();
            return DbContext.Banners.Where(x => x.IsActive).Take(5).ToListAsync();
        }
    }
}
