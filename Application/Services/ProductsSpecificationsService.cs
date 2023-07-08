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
    class ProductsSpecificationsService : IProductsSpecificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsSpecificationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsSpecificationsVM>> Get()
        {
            var models = await _unitOfWork.ProductsSpecificationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsSpecificationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsSpecificationsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsSpecificationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsSpecificationsDTO>(model);
            return modelDto;
        }
        public async Task<ProductsSpecificationsDTO> Create(ProductsSpecificationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsSpecifications>(modelDto);
            _unitOfWork.ProductsSpecificationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsSpecificationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsSpecificationsDTO> Update(ProductsSpecificationsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsSpecificationsRepo.Update(_mapper.Map<ProductsSpecifications>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsSpecificationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsSpecificationsRepo.Delete(model);
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
