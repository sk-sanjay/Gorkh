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
    public class ShippingWeightsService : IShippingWeightsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShippingWeightsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ShippingWeightsVM>> Get()
        {
            var models = await _unitOfWork.ShippingWeightsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ShippingWeightsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ShippingWeightsDTO> Get(int id)
        {
            var model = await _unitOfWork.ShippingWeightsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ShippingWeightsDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(ShippingWeightsDTO modelDto)
        {
            var model = _mapper.Map<ShippingWeights>(modelDto);
            var duplicate = await _unitOfWork.ShippingWeightsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<ShippingWeightsDTO> Create(ShippingWeightsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ShippingWeights>(modelDto);
            _unitOfWork.ShippingWeightsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ShippingWeightsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ShippingWeightsDTO> Update(ShippingWeightsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ShippingWeightsRepo.Update(_mapper.Map<ShippingWeights>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ShippingWeightsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ShippingWeightsRepo.Delete(model);
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
