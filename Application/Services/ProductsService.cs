using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
namespace Application.Services
{
    class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsVM>> Get()
        {
            var models = await _unitOfWork.ProductsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDTO>(model);
            return modelDto;
        }
        public async Task<ProductsDTO> Create(ProductsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Products>(modelDto);
            _unitOfWork.ProductsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsDTO> Update(ProductsDTO modelDto)
        {
            if (modelDto == null) return null;
            var ProductsSpecifications = await _unitOfWork.ProductsSpecificationsRepo.GetByProductId(modelDto.Id).ConfigureAwait(false);
            _unitOfWork.ProductsSpecificationsRepo.DeleteRange(ProductsSpecifications);
            var rowsChanged1 = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            _unitOfWork.ProductsRepo.Update(_mapper.Map<Products>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var ProductsSpecifications = await _unitOfWork.ProductsSpecificationsRepo.GetByProductId(id).ConfigureAwait(false);
            _unitOfWork.ProductsSpecificationsRepo.DeleteRange(ProductsSpecifications);

            var model = await _unitOfWork.ProductsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsRepo.Delete(model);
            //return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            var rowsChanged = -1;
            try
            {
                rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(DbUpdateException) || ex.GetType() == typeof(DbUpdateConcurrencyException))
                    rowsChanged = -2;
            }
            return rowsChanged;
        }
        public async Task<int> FinalSubmitflagupdate(CommonDTO argModelDto)
        {
            var model = await _unitOfWork.ProductsRepo.Get(argModelDto.ID).ConfigureAwait(false);
            if (model == null) return 0;
            model.Id = argModelDto.ID;
            model.FinalSubmit = 1;
            _unitOfWork.ProductsRepo.Update(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }
        public async Task<List<ProductsBySellerVM>> GetProductsBySeller(int sellerid)
        {
            var models = await _unitOfWork.ProductsRepo.GetProductsBySeller(sellerid).ConfigureAwait(false);
            foreach(var a in models)
            {
                if(a.totalVisitor==null)
                {
                    a.totalVisitor = 0; // check if right table row data null then default value should be 0
                }
                if (a.TotalInterest == null)
                {
                    a.TotalInterest = 0; // check if right table row data null then default value should be 0
                }
            }
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBySellerVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<List<ProductsBySellerVM>> GetProductVisitor(int productid)
        {
            var models = await _unitOfWork.ProductsRepo.GetProductVisitor(productid).ConfigureAwait(false);
            foreach (var a in models)
            {
                if (a.totalVisitor == null)
                {
                    a.totalVisitor = 0; // check if right table row data null then default value should be 0
                }
                if (a.TotalInterest == null)
                {
                    a.TotalInterest = 0; // check if right table row data null then default value should be 0
                }
            }
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBySellerVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<ProductsVM>> GetAllProductsForAdmin()
        {
            var models = await _unitOfWork.ProductsRepo.GetAllProductsForAdmin().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<List<ProductsVM>> GetAllPendingProducts()
        {
            var models = await _unitOfWork.ProductsRepo.GetAllPendingProducts().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<int> UpdateSelected(int id)
        {
            var model = await _unitOfWork.ProductsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            if (!model.IsApprove)
            {
                model.IsApprove = true;

                _unitOfWork.ProductsRepo.Update(model);
            }
            else
            {
                model.IsApprove = false;
                _unitOfWork.ProductsRepo.Update(model);
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? rowsChanged : -1;
        }

        public async Task<int> UpdateasFeatured(int id)
        {
            var model = await _unitOfWork.ProductsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            if (!model.IsFeatured)
            {
                model.IsFeatured = true;
                _unitOfWork.ProductsRepo.Update(model);
            }
            else
            {
                model.IsFeatured = false;
                _unitOfWork.ProductsRepo.Update(model);
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? rowsChanged : -1;
        }


        
        public async Task<List<ProductsVM>> GettProductsBySubCatId(int subcategoryid)
        {
            var models = await _unitOfWork.ProductsRepo.GettProductsBySubCatId(subcategoryid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        //public async Task<List<ProductsVM>> GetProductsasfeaturedmachine()
        //{
        //    var models = await _unitOfWork.ProductsRepo.GetProductsasfeaturedmachine().ConfigureAwait(false);
        //    if (models == null) return null;
        //    var modelVms = _mapper.Map<List<ProductsVM>>(models);
        //    if (modelVms == null || modelVms.Count <= 0) return null;
        //    return modelVms;
        //}

        //public async Task<List<Products1VM>> GettProductsBySubCatId1(int subcategoryid)
        //{
        //    List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
        //    };
        //    var model = await _unitOfWork.Products1Repo.GetListFromSql("Sp_Products_GetBySubCatId", p).ConfigureAwait(false);
        //    if (model == null) return null;
        //    var modelVm = _mapper.Map<List<Products1VM>>(model);
        //    return modelVm;
        //}
        public async Task<List<ProductsViewModel>> GettProductsBySubCatId2(int subcategoryid)
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.ProductsRepo.GetListFromSql("Sp_Products_GetBySubCatId", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<ProductsViewModel>>(model);
            return modelVm;
        }
        //Get Products Details By Id
        public async Task<ProductsDetailsVM> GettProductsDetailsById(int id)
        {
            var model = await _unitOfWork.ProductsRepo.GettProductsDetailsById(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDetailsVM>(model);
            if (modelDto == null) return null;
            return modelDto;
        }
        //Get Products Details By Id
        public async Task<ProductsDetailsVM> GettProductsDetailsByIdwebsite(int id)
        {
            var model = await _unitOfWork.ProductsRepo.GettProductsDetailsByIdwebsite(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDetailsVM>(model);
            if (modelDto == null) return null;
            return modelDto;
        }
        //Get Products Details By Id
        public async Task<ProductsDetailsVM> GetProductsDetailsById(int id)
        {
            var model = await _unitOfWork.ProductsRepo.GetProductsDetailsById(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDetailsVM>(model);
            if (modelDto == null) return null;
            return modelDto;
        }
        //Get Products and Seller Details By Id
        public async Task<ProductsSellerDetailsVM> GetProductsSellerDetailsById(int id)
        {
            var model = await _unitOfWork.ProductsRepo.GetProductsSellerDetailsById(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsSellerDetailsVM>(model);
            if (modelDto == null) return null;
            return modelDto;
        }
        public async Task<List<ProductsBreadCrumbsVM>> GetProductsBreadCrumbs(int catid, int subcatid)
        {
            if (catid <= 0 && subcatid <= 0) return null;
            var models = await _unitOfWork.ProductsRepo.GetProductsBreadCrumbs(catid, subcatid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBreadCrumbsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<List<RelatedProductsVM>> GetRelatedProducts(int subsubcatid)
        {
            var models = await _unitOfWork.ProductsRepo.GetRelatedProducts(subsubcatid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<RelatedProductsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms.Distinct(new Productcompare()).ToList();
        }
        public class Productcompare : IEqualityComparer<RelatedProductsVM>
        {
            public bool Equals([AllowNull] RelatedProductsVM x, [AllowNull] RelatedProductsVM y)
            {
                return x.ProductNumber == y.ProductNumber;
            }

            public int GetHashCode([DisallowNull] RelatedProductsVM obj)
            {
                return obj.ProductNumber.GetHashCode();
            }
        }
    }
}
