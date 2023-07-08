using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SellerRegistrationsRepository : Repository<SellerRegistrations>, ISellerRegistrationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public SellerRegistrationsRepository(AppDbContext context) : base(context)
        {

        }
        public Task<SellerRegistrations> CheckEmail(string name)
        {
            return DbContext.SellerRegistrations.FirstOrDefaultAsync(x => x.Email == name);
        }

        public Task<SellerRegistrations> GetbyEmail(string email)
        {
            return DbContext.SellerRegistrations.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<SellerRegistrations> GetbySellerId(int sellerid)
        {
            return DbContext.SellerRegistrations.FirstOrDefaultAsync(x => x.Id == sellerid);
        }


    }
}
