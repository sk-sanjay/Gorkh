using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public class OurCustomersRepository : Repository<OurCustomers>, IOurCustomersRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public OurCustomersRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<OurCustomers>> GetOurCustomersHomePage()
        {
            return await (from a in DbContext.OurCustomers where a.IsActive == true select a).ToListAsync();

        }
    }
}
