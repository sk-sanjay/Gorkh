using Application.ViewModels;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BuyerRequirementsRepository : Repository<BuyerRequirements>, IBuyerRequirementsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public BuyerRequirementsRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<List<BuyerRequirements1>> GetBuyerRequirements()
        {
            var WE = await (from Eqp in DbContext.BuyerRequirements
                            join Cat in DbContext.Categories on Eqp.CategoryId equals Cat.Id
                            join SubCat in DbContext.SubCategories on Eqp.SubCategoryId equals SubCat.Id into sc

                            from SCc in sc.DefaultIfEmpty()
                            join SubSubCat in DbContext.SubSubCategories on Eqp.SubSubCategoryId equals SubSubCat.Id into SSC


                            from SSCc in SSC.DefaultIfEmpty()
                            join Con in DbContext.Conditions on Eqp.ProductCondition equals Con.Id into connnn
                            from co in connnn.DefaultIfEmpty()

                            orderby Eqp.Id descending
                            select new BuyerRequirements1
                            {
                                Id = Eqp.Id,
                                Category = Cat.Name,
                                SubCategory = SCc.SubCategoryName,
                                SubSubCategory = SSCc.SubSubCategoriesName,
                                Files = Eqp.Files,
                                Descrition = Eqp.Descrition,
                                IsActive = Eqp.IsActive,
                                CreatedDate = Eqp.CreatedDate,
                                ModifiedDate = Eqp.ModifiedDate,
                                ProductListingTitle = Eqp.ProductListingTitle,
                                Qty = Eqp.Qty,
                                ProductCondition = co.ConditionName,
                                SubSubCategoryName=Eqp.SubSubCategoryName

                            }
                           ).ToListAsync();
            return WE;

        }

        public async Task<List<BuyerRequirements1>> GetBuyerRequirementsforWebsite()
        {
            var WE = await (from Eqp in DbContext.BuyerRequirements
                            join Cat in DbContext.Categories on Eqp.CategoryId equals Cat.Id
                            join SubCat in DbContext.SubCategories on Eqp.SubCategoryId equals SubCat.Id into sc
                            from SCc in sc.DefaultIfEmpty()
                            join SubSubCat in DbContext.SubSubCategories on Eqp.SubSubCategoryId equals SubSubCat.Id into SSC

                            from SSCc in SSC.DefaultIfEmpty()

                            orderby Eqp.Id descending
                            select new BuyerRequirements1
                            {
                                Id = Eqp.Id,
                                Category = Cat.Name,
                                SubCategory = SCc.SubCategoryName,
                                SubSubCategory = Eqp.SubSubCategoryName,
                                Files = Eqp.Files,
                                Descrition = Eqp.Descrition,
                                IsActive = Eqp.IsActive,
                                CreatedDate = Eqp.CreatedDate,
                                ModifiedDate = Eqp.ModifiedDate,
                                ProductListingTitle = Eqp.ProductListingTitle,
                                SubSubCategoryName = Eqp.SubSubCategoryName


                            }
                           ).Take(7).ToListAsync();
            return WE;

        }


        public async Task<BuyerRequirements1> GetBuyerRequirements(int id)
        {
            return await (from Eqp in DbContext.BuyerRequirements
                          join Cat in DbContext.Categories on Eqp.CategoryId equals Cat.Id
                          join SubCat in DbContext.SubCategories on Eqp.SubCategoryId equals SubCat.Id into sc
                          from SCc in sc.DefaultIfEmpty()
                          join SubSubCat in DbContext.SubSubCategories on Eqp.SubSubCategoryId equals SubSubCat.Id into SSC
                          from SSCc in SSC.DefaultIfEmpty()
                          join Con in DbContext.Conditions on Eqp.ProductCondition equals Con.Id into connnn
                          from co in connnn.DefaultIfEmpty()
                          where Eqp.Id == id
                          select new BuyerRequirements1
                          {
                              Id = Eqp.Id,
                              Category = Cat.Name,
                              EmailID = Eqp.EmailID,
                              SubCategory = SCc.SubCategoryName,
                              SubSubCategory = SSCc.SubSubCategoriesName,
                              Files = Eqp.Files,
                              Descrition = Eqp.Descrition,
                              IsActive = Eqp.IsActive,
                              CreatedDate = Eqp.CreatedDate,
                              ModifiedDate = Eqp.ModifiedDate,
                              ProductListingTitle = Eqp.ProductListingTitle,
                              Qty = Eqp.Qty,
                              ProductCondition = co.ConditionName,
                              SubSubCategoryName = Eqp.SubSubCategoryName
                          }
                           ).FirstOrDefaultAsync();


        }
        public async Task<List<BuyerRequirements1>> GetBuyerRequirementsbyusername(string email)
        {
            return await (from Eqp in DbContext.BuyerRequirements
                          join Cat in DbContext.Categories on Eqp.CategoryId equals Cat.Id
                          join SubCat in DbContext.SubCategories on Eqp.SubCategoryId equals SubCat.Id into sc
                          from SCc in sc.DefaultIfEmpty()
                          //join SubSubCat in DbContext.SubSubCategories on Eqp.SubSubCategoryId equals SubSubCat.Id into SSC
                          //from SSCc in SSC.DefaultIfEmpty()
                          join Con in DbContext.Conditions on Eqp.ProductCondition equals Con.Id into connnn
                          from co in connnn.DefaultIfEmpty()
                          where Eqp.EmailID == email
                          orderby Eqp.Id descending
                          select new BuyerRequirements1
                          {
                              Id = Eqp.Id,
                              FullName=Eqp.FullName,
                             
                              Category = Cat.Name,
                              SubCategory = SCc.SubCategoryName,
                              SubSubCategory = Eqp.SubSubCategoryName,
                              Files = Eqp.Files,
                              Descrition = Eqp.Descrition,
                              IsActive = Eqp.IsActive,
                              CreatedDate = Eqp.CreatedDate,
                              ModifiedDate = Eqp.ModifiedDate,
                              ProductListingTitle = Eqp.ProductListingTitle,
                              Qty = Eqp.Qty,
                              ProductCondition = co.ConditionName,
                              SubSubCategoryName = Eqp.SubSubCategoryName
                          }
                           ).ToListAsync();


        }
    }
}
