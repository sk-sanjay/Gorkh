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
    class ProductsLocationsService : IProductsLocationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsLocationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsLocationsVM>> Get()
        {
            var models = await _unitOfWork.ProductsLocationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsLocationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsLocationsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsLocationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsLocationsDTO>(model);
            return modelDto;
        }
        public async Task<ProductsLocationsDTO> GetByProductId(int id)
        {
            var model = await _unitOfWork.ProductsLocationsRepo.GetByProductId(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<ProductsLocationsDTO>(model);
            return modelVm;
        }
        public async Task<ProductsLocationsDTO> Create(ProductsLocationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsLocations>(modelDto);
            _unitOfWork.ProductsLocationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsLocationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsLocationsDTO> Update(ProductsLocationsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsLocationsRepo.Update(_mapper.Map<ProductsLocations>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsLocationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsLocationsRepo.Delete(model);
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
    }
}
