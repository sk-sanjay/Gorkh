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
    public class ProductsRepository : Repository<Products>, IProductsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ProductsRepository(AppDbContext context) : base(context)
        {
        }
        //public async Task<List<Products>> GetProductsBySeller(int sellerid)
        //{
        //    return await (from a in DbContext.Products where a.SellerId == sellerid orderby a.Id descending select a).ToListAsync();

        //}
        public async Task<List<ProductsBySeller>> GetProductsBySeller(int sellerid)
        {
            var countv = (from a in DbContext.ProductsVisitors
                          group a by a.ProductId into ag
                          select new
                          {
                              productid = ag.Key,
                              cnt = ag.Count()
                          });

            var countInterest = (from a in DbContext.ProductsBuyerIntrests
                                 group a by a.ProductId into Ints
                                 select new
                                 {
                                     productid = Ints.Key,
                                     picount = Ints.Count()
                                 });

            var aa = await (from a in DbContext.Products
                            join b in countv on a.Id equals b.productid into cv
                            from x in cv.DefaultIfEmpty()
                            join c in countInterest on a.Id equals c.productid into cv1
                            from c1 in cv1.DefaultIfEmpty()
                            where a.SellerId == sellerid //&& a.FinalSubmit == 1
                            orderby a.Id descending
                            select new ProductsBySeller
                            {
                                Id = a.Id,
                                ProductNumber = a.ProductNumber,
                                SaleType = a.SellerType,
                                ReservePrice = a.ReservePrice,
                                IsApprove = a.IsApprove,
                                SubSubCategory = a.SubSubCategory,
                                totalVisitor = x.cnt,
                                //totalVisitor = (x == null ? 0 : x.cnt)
                                TotalInterest = c1.picount

                            }
                          ).ToListAsync();
            return aa;

        }

        public async Task<List<ProductsBySeller>> GetProductVisitor(int productid)
        {
            var countv = (from a in DbContext.ProductsVisitors
                          group a by a.ProductId into ag
                          select new
                          {
                              productid = ag.Key,
                              cnt = ag.Count()
                          });

            var countInterest = (from a in DbContext.ProductsBuyerIntrests
                                 group a by a.ProductId into Ints
                                 select new
                                 {
                                     productid = Ints.Key,
                                     picount = Ints.Count()
                                 });

            var aa = await (from a in DbContext.Products
                            join b in countv on a.Id equals b.productid into cv
                            from x in cv.DefaultIfEmpty()
                            join c in countInterest on a.Id equals c.productid into cv1
                            from c1 in cv1.DefaultIfEmpty()
                            where a.Id == productid //&& a.FinalSubmit == 1
                            orderby a.Id descending
                            select new ProductsBySeller
                            {
                                Id = a.Id,
                                ProductNumber = a.ProductNumber,
                                SaleType = a.SellerType,
                                ReservePrice = a.ReservePrice,
                                IsApprove = a.IsApprove,
                                SubSubCategory = a.SubSubCategory,
                                totalVisitor = x.cnt,
                                //totalVisitor = (x == null ? 0 : x.cnt)
                                TotalInterest = c1.picount

                            }
                          ).ToListAsync();
            return aa;

        }

        public async Task<List<Products>> GetAllProductsForAdmin()
        {
            return await (from a in DbContext.Products where a.FinalSubmit == 1 orderby a.Id descending select a).ToListAsync();
        }

        public async Task<List<Products>> GetAllPendingProducts()
        {
            return await (from a in DbContext.Products where a.FinalSubmit == null orderby a.Id descending select a).ToListAsync();
        }

        public async Task<List<Products>> GettProductsBySubCatId(int subcategoryid)
        {
            return await (from a in DbContext.Products where a.SubCategoryId == subcategoryid select a).ToListAsync();
            //return await DbContext.Products.Where(x => x.SubCategoryId == subcategoryid).ToListAsync();
        }
        public async Task<List<RelatedProducts>> GetRelatedProducts(int subsubcatid)
        {

            return await (from subsubcat in DbContext.SubSubCategories
                          join Prod in DbContext.Products on subsubcat.Id equals Prod.SubSubCatId
                          join PF in DbContext.ProductsFileUploads on Prod.Id equals PF.ProductId
                          
                          where subsubcat.Id == subsubcatid && Prod.IsApprove==true
                          select new RelatedProducts
                          {
                              ProductName= subsubcat.SubSubCategoriesName,
                              ProductNumber=Prod.ProductNumber,
                              ProductImage=PF.ProductImage
                          }
                           ).ToListAsync();
        }
        public async Task<List<ProductsDetails>> GetProductsDetailsById(int id)
        {
            return await (from a in DbContext.Products
                          join b in DbContext.Categories on a.CategoryId equals b.Id
                          join c in DbContext.SubCategories on a.SubCategoryId equals c.Id
                          join d in DbContext.SubSubCategories on a.SubSubCatId equals d.Id
                          join e in DbContext.Conditions on a.ConditionId equals e.Id
                          join f in DbContext.ProductsLocations on a.Id equals f.ProductId
                          join g in DbContext.Countries on f.CountryId equals g.Id
                          join h in DbContext.States on f.StateId equals h.Id
                          join i in DbContext.Cities on f.CityId equals i.Id
                          join j in DbContext.ProductsDescriptions on a.Id equals j.ProductId
                          //join k in DbContext.ProductsFileUploads on a.Id equals k.ProductId
                          where a.Id == id
                          select new ProductsDetails
                          {
                              Id = a.Id,
                              ProductNumber = a.ProductNumber,
                              SaleType = a.SaleType,
                              SellerType = a.SellerType,
                              QuantityAvl = a.QuantityAvl,
                              YearofProc = a.YearofProc,
                              InventoryId = a.InventoryId,
                              SerialNo = a.SerialNo,
                              EstimatePrice=a.EstimatePrice,
                              ReservePrice = a.ReservePrice,
                              Name = b.Name,
                              SubCategoryName = c.SubCategoryName,
                              SubSubCategoriesName = d.SubSubCategoriesName,
                              ConditionName = e.ConditionName,
                              AddressLine1 = f.AddressLine1,
                              AddressLine2 = f.AddressLine2,
                              ZipCode = f.ZipCode,
                              CountryName = g.Name,
                              StateName = h.StateName,
                              CityName = i.CityName,
                              Descriptions = j.Descriptions
                          }
                          ).ToListAsync();
        }

        public async Task<ProductsDetails> GettProductsDetailsById(int id)
        {
            var specfDataList = (from a in DbContext.ProductsSpecifications
                                 join b in DbContext.Specifications on a.SpecfId equals b.Id
                                 where a.ProductId == id
                                 select new ProductsSpecificationsGets
                                 {
                                     Id = a.Id,
                                     SpecfSSCatField = a.SpecfSSCatField,
                                     SpecfName = b.SpecfName
                                 }).ToList();
            var imgDataList = (from a in DbContext.ProductsFileUploads where a.ProductId == id select a).ToList();

            return await (from a in DbContext.Products
                          join b in DbContext.Categories on a.CategoryId equals b.Id
                          join c in DbContext.SubCategories on a.SubCategoryId equals c.Id
                          join d in DbContext.SubSubCategories on a.SubSubCatId equals d.Id
                          join e in DbContext.Conditions on a.ConditionId equals e.Id
                          join f in DbContext.ProductsLocations on a.Id equals f.ProductId
                          join g in DbContext.Countries on f.CountryId equals g.Id
                          join h in DbContext.States on f.StateId equals h.Id
                          join i in DbContext.Cities on f.CityId equals i.Id
                          join j in DbContext.ProductsDescriptions on a.Id equals j.ProductId
                          //join k in DbContext.ProductsFileUploads on a.Id equals k.ProductId
                          where a.Id == id //&& a.FinalSubmit==1
                          select new ProductsDetails
                          {
                              Id = a.Id,
                              CatId = a.CategoryId,
                              SubCatId = a.SubCategoryId,
                              SubSubCatId = a.SubSubCatId,
                              ProductNumber = a.ProductNumber,
                              SaleType = a.SaleType,
                              SellerType = a.SellerType,
                              QuantityAvl = a.QuantityAvl,
                              YearofProc = a.YearofProc,
                              InventoryId = a.InventoryId,
                              SerialNo = a.SerialNo,
                              EstimatePrice = a.EstimatePrice,
                              ReservePrice = a.ReservePrice,
                              CreatedDate = a.CreatedDate,
                              Name = b.Name,
                              SubCategoryName = c.SubCategoryName,
                              SubSubCategoriesName = d.SubSubCategoriesName,
                              ConditionName = e.ConditionName,
                              AddressLine1 = f.AddressLine1,
                              AddressLine2 = f.AddressLine2,
                              ZipCode = f.ZipCode,
                              CountryName = g.Name,
                              StateName = h.StateName,
                              CityName = i.CityName,
                              SellerId=a.SellerId,
                              Descriptions = j.Descriptions,
                              ProductsFileUpload = imgDataList,
                              ProductsSpecificationsGet = specfDataList
                          }
                          ).FirstOrDefaultAsync();
        }

        public async Task<ProductsDetails> GettProductsDetailsByIdwebsite(int id)
        {
            var specfDataList = (from proSpec in DbContext.ProductsSpecifications

                                 join spec in DbContext.Specifications on proSpec.SpecfId equals spec.Id
                                 join pro in DbContext.Products on proSpec.ProductId equals pro.Id
                                 join mapp in DbContext.SpecificationsSSCategories on new{proSpec.SpecfId ,pro.CategoryId,pro.SubCategoryId,pro.SubSubCatId} equals new { mapp.SpecfId, mapp.CategoryId, mapp.SubCategoryId, mapp.SubSubCatId } 
                                 where proSpec.ProductId == id
                                 select new ProductsSpecificationsGets 
                                 {
                                     Id = proSpec.Id,
                                     SpecfSSCatField = proSpec.SpecfSSCatField,
                                     SpecfName = spec.SpecfName,
                                     Sequence = mapp.Sequence
                                 }).OrderBy(x => x.Sequence).ToList();
            var imgDataList = (from a in DbContext.ProductsFileUploads where a.ProductId == id select a).ToList();

            return await (from a in DbContext.Products
                          join b in DbContext.Categories on a.CategoryId equals b.Id
                          join c in DbContext.SubCategories on a.SubCategoryId equals c.Id
                          join d in DbContext.SubSubCategories on a.SubSubCatId equals d.Id
                          join e in DbContext.Conditions on a.ConditionId equals e.Id
                          join f in DbContext.ProductsLocations on a.Id equals f.ProductId
                          join g in DbContext.Countries on f.CountryId equals g.Id
                          join h in DbContext.States on f.StateId equals h.Id
                          join i in DbContext.Cities on f.CityId equals i.Id
                          join j in DbContext.ProductsDescriptions on a.Id equals j.ProductId
                          //join k in DbContext.ProductsFileUploads on a.Id equals k.ProductId
                          where a.Id == id && a.IsApprove == true //&& a.FinalSubmit==1
                          select new ProductsDetails
                          {
                              Id = a.Id,
                              CatId = a.CategoryId,
                              SubCatId = a.SubCategoryId,
                              SubSubCatId = a.SubSubCatId,
                              ProductNumber = a.ProductNumber,
                              SaleType = a.SaleType,
                              SellerType = a.SellerType,
                              QuantityAvl = a.QuantityAvl,
                              YearofProc = a.YearofProc,
                              InventoryId = a.InventoryId,
                              SerialNo = a.SerialNo,
                              EstimatePrice = a.EstimatePrice,
                              ReservePrice = a.ReservePrice,
                              CreatedDate = a.CreatedDate,
                              Name = b.Name,
                              SubCategoryName = c.SubCategoryName,
                              SubSubCategoriesName = d.SubSubCategoriesName,
                              ConditionName = e.ConditionName,
                              AddressLine1 = f.AddressLine1,
                              AddressLine2 = f.AddressLine2,
                              ZipCode = f.ZipCode,
                              CountryName = g.Name,
                              StateName = h.StateName,
                              CityName = i.CityName,
                              Descriptions = j.Descriptions,
                              ProductsFileUpload = imgDataList,
                              ProductsSpecificationsGet = specfDataList
                          }
                          ).FirstOrDefaultAsync();
        }
       
        public async Task<ProductsSellerDetails> GetProductsSellerDetailsById(int id)
        {
            return await (from a in DbContext.Products
                          join b in DbContext.SubSubCategories on a.SubSubCatId equals b.Id
                          join c in DbContext.BuyerSellerRegistrations on a.SellerId equals c.Id
                          where a.Id == id
                          select new ProductsSellerDetails
                          {
                              Id = a.Id,
                              SubSubCategoriesName = b.SubSubCategoriesName,
                              SellerName = c.FirstName + " " + c.LastName,
                              SellerEmail = c.Email

                          }
                          ).FirstOrDefaultAsync();
        }
        public async Task<List<ProductsBreadCrumbs>> GetProductsBreadCrumbs(int catid, int subcatid)
        {
            var Result = from e in DbContext.Categories
                         join d in DbContext.SubCategories on e.Id equals d.CategoryId into cats
                         from subcats in cats.DefaultIfEmpty()
                         select new ProductsBreadCrumbs
                         {
                             CatId = e.Id,
                             CatName = e.Name,
                             SubCatid = subcats.Id,
                             SubCatName = subcats.SubCategoryName
                         };


            if (catid > 0 && subcatid == 0)
            {
                Result = Result.Where(x => x.CatId == catid)
                    .Select(x => new ProductsBreadCrumbs
                    { CatId = x.CatId, CatName = x.CatName }).Take(1);
            }
            if (subcatid > 0)
            {
                Result = Result.Where(x => x.SubCatid == subcatid)
                    .Select(x => new ProductsBreadCrumbs
                    { CatId = x.CatId, CatName = x.CatName, SubCatid = x.SubCatid, SubCatName = x.SubCatName }).Take(1);
            }
            return await Result.ToListAsync();
        }
    }
    
}
