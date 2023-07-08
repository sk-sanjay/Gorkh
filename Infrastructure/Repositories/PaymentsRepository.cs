using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PaymentsRepository : Repository<Payments>, IPaymentsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public PaymentsRepository(AppDbContext context) : base(context)
        {
        }
        public Task<Payments> CheckDuplicate(Payments model)
        {
            var duplicateModel = DbContext.Payments.FirstOrDefaultAsync(x =>
                  x.ProductId == model.ProductId && x.BuyerId == model.BuyerId);
            return duplicateModel;
        }
        public async Task<List<PaymentsCommon>> GetPaymentsForAdmin()
        {
            var aa = await (from a in DbContext.Payments
                            join b in DbContext.Products on a.ProductId equals b.Id
                            join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                            orderby a.CreatedDate descending
                            select new PaymentsCommon
                            {
                                Id = a.Id,
                                PaymentModeId=a.PaymentModeId,
                                DdChequeNo=a.DdChequeNo,
                                ChAndBgDate=a.ChAndBgDate,
                                DrawnOn=a.DrawnOn,
                                AmountRp=a.AmountRp,
                                BgNo=a.BgNo,
                                CreatedDate = a.CreatedDate,
                                PaymentStatus=a.PaymentStatus,
                                RecDate=a.RecDate,
                                ProductId = a.ProductId,
                                BuyerId=a.BuyerId,
                                ProductNumber = b.ProductNumber,
                                SubSubCategory = b.SubSubCategory,
                                BuyerFullName = c.FirstName + ' ' + c.LastName
                            }
                    ).ToListAsync();

            return aa;

        }
        public async Task<Payments> GetProductsPaymentsByBuyerandPid(int buyerid, int productid)
        {
            var result = await DbContext.Payments.FirstOrDefaultAsync(x =>
                x.BuyerId == buyerid && x.ProductId == productid);
            return result;
        }
        public async Task<List<PaymentsCommon>> GetProductsPaymentsByBuyer(int buyerid)
        {
            return await (from a in DbContext.Payments
                          join b in DbContext.Products on a.ProductId equals b.Id
                          //join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                          where a.BuyerId == buyerid
                          orderby a.Id descending
                          select new PaymentsCommon
                          {
                              Id = a.Id,
                              PaymentModeId = a.PaymentModeId,
                              DdChequeNo = a.DdChequeNo,
                              ChAndBgDate = a.ChAndBgDate,
                              DrawnOn = a.DrawnOn,
                              AmountRp = a.AmountRp,
                              BgNo = a.BgNo,
                              CreatedDate = a.CreatedDate,
                              PaymentStatus = a.PaymentStatus,
                              RecDate=a.RecDate,
                              ProductId = a.ProductId,
                              BuyerId = a.BuyerId,
                              ProductNumber = b.ProductNumber,
                              ReservePrice = b.ReservePrice,
                              SubSubCategory = b.SubSubCategory
                          }
                          ).ToListAsync();

        }
        public async Task<List<PaymentsCommon>> GetProductsPaymentsForAdmin()
        {
            var countpayments = (from a in DbContext.Payments
                          group a by a.ProductId into Pay
                          select new
                          {
                              productid = Pay.Key,
                              cnt = Pay.Count()
                          });

            var aa = await (from a in DbContext.Products
                            join b in countpayments on a.Id equals b.productid
                            orderby a.Id descending
                            select new PaymentsCommon
                            {
                                Id = a.Id,
                                ProductNumber = a.ProductNumber,
                                ReservePrice = a.ReservePrice,
                                SubSubCategory = a.SubSubCategory,
                                TotalPaidBuyer = b.cnt
                            }
                         ).ToListAsync();
            return aa;

        }
        public async Task<List<PaymentsCommon>> GetPaymentsByProductIdForAdmin(int productid)
        {
            var aa = await (from a in DbContext.Payments
                            join b in DbContext.Products on a.ProductId equals b.Id
                            join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                            where a.ProductId == productid
                            orderby a.CreatedDate descending
                            select new PaymentsCommon
                            {
                                Id = a.Id,
                                PaymentModeId = a.PaymentModeId,
                                DdChequeNo = a.DdChequeNo,
                                ChAndBgDate = a.ChAndBgDate,
                                DrawnOn = a.DrawnOn,
                                AmountRp = a.AmountRp,
                                BgNo = a.BgNo,
                                CreatedDate = a.CreatedDate,
                                PaymentStatus = a.PaymentStatus,
                                ProductId = a.ProductId,
                                BuyerId = a.BuyerId,
                                ProductNumber = b.ProductNumber,
                                SubSubCategory = b.SubSubCategory,
                                BuyerFullName = c.FirstName + ' ' + c.LastName
                            }
                    ).ToListAsync();

            return aa;

        }
        public async Task<List<PaymentsCommon>> GetProductsPaymentsForSeller(int sellerid)
        {
            var countpayments = (from a in DbContext.Payments
                                 group a by a.ProductId into Pay
                                 select new
                                 {
                                     productid = Pay.Key,
                                     cnt = Pay.Count()
                                 });

            var aa = await (from a in DbContext.Products
                            join b in countpayments on a.Id equals b.productid
                            where a.SellerId == sellerid
                            orderby a.Id descending
                            select new PaymentsCommon
                            {
                                Id = a.Id,
                                ProductNumber = a.ProductNumber,
                                ReservePrice = a.ReservePrice,
                                SubSubCategory = a.SubSubCategory,
                                TotalPaidBuyer = b.cnt
                            }
                         ).ToListAsync();
            return aa;

        }
        public async Task<List<PaymentsCommon>> GetPaymentsByProductIdForSeller(int productid)
        {
            var aa = await (from a in DbContext.Payments
                            join b in DbContext.Products on a.ProductId equals b.Id
                            join c in DbContext.BuyerSellerRegistrations on a.BuyerId equals c.Id
                            where a.ProductId == productid
                            orderby a.CreatedDate descending
                            select new PaymentsCommon
                            {
                                Id = a.Id,
                                PaymentModeId = a.PaymentModeId,
                                DdChequeNo = a.DdChequeNo,
                                ChAndBgDate = a.ChAndBgDate,
                                DrawnOn = a.DrawnOn,
                                AmountRp = a.AmountRp,
                                BgNo = a.BgNo,
                                CreatedDate = a.CreatedDate,
                                PaymentStatus = a.PaymentStatus,
                                ProductId = a.ProductId,
                                BuyerId = a.BuyerId,
                                ProductNumber = b.ProductNumber,
                                SubSubCategory = b.SubSubCategory,
                                BuyerFullName = c.FirstName + ' ' + c.LastName
                            }
                    ).ToListAsync();

            return aa;

        }
    }
}
