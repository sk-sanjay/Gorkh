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
    public class ProductsVisitorsService : IProductsVisitorsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsVisitorsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsVisitorsVM>> Get()
        {
            var models = await _unitOfWork.ProductsVisitorsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsVisitorsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsVisitorsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsVisitorsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsVisitorsDTO>(model);
            return modelDto;
        }
        public async Task<ProductsVisitorsDTO> Create(ProductsVisitorsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsVisitors>(modelDto);
            _unitOfWork.ProductsVisitorsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsVisitorsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsVisitorsDTO> Update(ProductsVisitorsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsVisitorsRepo.Update(_mapper.Map<ProductsVisitors>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsVisitorsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsVisitorsRepo.Delete(model);
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
