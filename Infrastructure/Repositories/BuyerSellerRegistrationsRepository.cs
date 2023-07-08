using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BuyerSellerRegistrationsRepository : Repository<BuyerSellerRegistrations>, IBuyerSellerRegistrationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyerSellerRegistrationsRepository(AppDbContext context) : base(context)
        {

        }
        public Task<BuyerSellerRegistrations> CheckEmail(string name)
        {
            return DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x => x.Email == name);
        }
        public Task<BuyerSellerRegistrations> CheckDuplicate(BuyerSellerRegistrations model)
        {
            var duplicateModel = DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x =>
                    x.Email == model.Email);
                return duplicateModel;
           
        }

        public Task<BuyerSellerRegistrations> GetbyEmail(string email)
        {
            return DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<BuyerSellerRegistrations> GetbyMobile(string mobile)
        {
            return DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x => x.Mobile == mobile);
        }

        public Task<BuyerSellerRegistrations> GetbySellerId(int sellerid)
        {
            return DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x => x.Id == sellerid);
        }

        public Task<BuyerSellerRegistrations> GetbyBuyerId(int buyerid)
        {
            return DbContext.BuyerSellerRegistrations.FirstOrDefaultAsync(x => x.Id == buyerid);
        }
        public Task<List<BuyerSellerRegistrations>> GetdropdownbySellerId(int orgid)
        {
            return DbContext.BuyerSellerRegistrations.Where(x => x.Id == orgid).ToListAsync();
        }


    }
}


