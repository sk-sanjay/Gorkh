using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductsBuyerQueriesService : IProductsBuyerQueriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsBuyerQueriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsBuyerQueriesVM>> Get()
        {
            var models = await _unitOfWork.ProductsBuyerQueriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerQueriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsBuyerQueriesDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsBuyerQueriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerQueriesDTO>(model);
            return modelDto;
        }
        public async Task<ProductsBuyerQueriesDTO> Create(ProductsBuyerQueriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsBuyerQueries>(modelDto);
            _unitOfWork.ProductsBuyerQueriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsBuyerQueriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsBuyerQueriesDTO> Update(ProductsBuyerQueriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsBuyerQueriesRepo.Update(_mapper.Map<ProductsBuyerQueries>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsBuyerQueriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsBuyerQueriesRepo.Delete(model);
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
        public async Task<List<ProductsBuyerQueriesCommonVM>> GetProductsByBuyer(int buyerid)
        {
            var models = await _unitOfWork.ProductsBuyerQueriesRepo.GetProductsByBuyer(buyerid).ConfigureAwait(false);
            
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerQueriesCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<ProductsBuyerQueriesCommonVM>> GetProductsBuyerQueriesByPid(int ProductId, int buyerid)
        {
            var models = await _unitOfWork.ProductsBuyerQueriesRepo.GetProductsBuyerQueriesByPid(ProductId, buyerid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerQueriesCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<ProductsBuyerQueriesCommonVM>> GetProductsBuyerQueriesForAdmin()
        {
            var models = await _unitOfWork.ProductsBuyerQueriesRepo.GetProductsBuyerQueriesForAdmin().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerQueriesCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsBuyerQueriesCommonVM> GetProductsBuyerQueriesById(int id)
        {
            var model = await _unitOfWork.ProductsBuyerQueriesRepo.GetProductsBuyerQueriesById(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerQueriesCommonVM>(model);
            return modelDto;
        }
        public async Task<ProductsBuyerQueriesCommonVM> GetProductsDetailsById(int ProductId)
        {
            var model = await _unitOfWork.ProductsBuyerQueriesRepo.GetProductsDetailsById(ProductId).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerQueriesCommonVM>(model);
            return modelDto;
        }
    }
}
