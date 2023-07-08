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
    class VoltageFrequenciesService : IVoltageFrequenciesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VoltageFrequenciesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<VoltageFrequenciesVM>> Get()
        {
            var models = await _unitOfWork.VoltageFrequenciesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<VoltageFrequenciesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<VoltageFrequenciesDTO> Get(int id)
        {
            var model = await _unitOfWork.VoltageFrequenciesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<VoltageFrequenciesDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(VoltageFrequenciesDTO modelDto)
        {
            var model = _mapper.Map<VoltageFrequencies>(modelDto);
            var duplicate = await _unitOfWork.VoltageFrequenciesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<VoltageFrequenciesDTO> Create(VoltageFrequenciesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<VoltageFrequencies>(modelDto);
            _unitOfWork.VoltageFrequenciesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<VoltageFrequenciesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<VoltageFrequenciesDTO> Update(VoltageFrequenciesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.VoltageFrequenciesRepo.Update(_mapper.Map<VoltageFrequencies>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.VoltageFrequenciesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.VoltageFrequenciesRepo.Delete(model);
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
