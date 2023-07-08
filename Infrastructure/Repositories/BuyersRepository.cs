using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BuyersRepository : Repository<Buyers>, IBuyersRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyersRepository(AppDbContext context) : base(context)
        {

        }
        public Task<Buyers> CheckEmail(string name)
        {
            return DbContext.Buyers.FirstOrDefaultAsync(x => x.Email == name);
        }

        public Task<Buyers> GetbyBuyerId(int buyerid)
        {
            return DbContext.Buyers.FirstOrDefaultAsync(x => x.Id == buyerid);
        }
    }
}
