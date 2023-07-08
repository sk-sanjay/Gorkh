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
    class ProductsDescriptionsService : IProductsDescriptionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsDescriptionsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsDescriptionsVM>> Get()
        {
            var models = await _unitOfWork.ProductsDescriptionsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsDescriptionsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsDescriptionsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsDescriptionsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDescriptionsDTO>(model);
            return modelDto;
        }
        public async Task<ProductsDescriptionsDTO> GetByProductId(int id)
        {
            var model = await _unitOfWork.ProductsDescriptionsRepo.GetByProductId(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<ProductsDescriptionsDTO>(model);
            return modelVm;
        }
        public async Task<ProductsDescriptionsDTO> Create(ProductsDescriptionsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsDescriptions>(modelDto);
            _unitOfWork.ProductsDescriptionsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsDescriptionsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsDescriptionsDTO> Update(ProductsDescriptionsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsDescriptionsRepo.Update(_mapper.Map<ProductsDescriptions>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsDescriptionsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsDescriptionsRepo.Delete(model);
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
        public async Task<ProductsDescriptionsDTO> GetProductsDescriptionsByProductId(int productid)
        {
            var model = await _unitOfWork.ProductsDescriptionsRepo.GetProductsDescriptionsByProductId(productid).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsDescriptionsDTO>(model);
            return modelDto;
        }
    }
}
