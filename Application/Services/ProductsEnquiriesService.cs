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
    public class ProductsEnquiriesService : IProductsEnquiriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsEnquiriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsEnquiriesVM>> Get()
        {
            var models = await _unitOfWork.ProductsEnquiriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsEnquiriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsEnquiriesDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsEnquiriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsEnquiriesDTO>(model);
            return modelDto;
        }
        public async Task<ProductsEnquiriesDTO> Create(ProductsEnquiriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsEnquiries>(modelDto);
            _unitOfWork.ProductsEnquiriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsEnquiriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsEnquiriesDTO> Update(ProductsEnquiriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsEnquiriesRepo.Update(_mapper.Map<ProductsEnquiries>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsEnquiriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsEnquiriesRepo.Delete(model);
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

        public async Task<List<ProductsEnquiriesVM>> GetProductsEnquiriesByPid(int ProductId)
        {
            var models = await _unitOfWork.ProductsEnquiriesRepo.GetProductsEnquiriesByPid(ProductId).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsEnquiriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
