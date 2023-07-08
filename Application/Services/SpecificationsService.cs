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
    class SpecificationsService : ISpecificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SpecificationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<SpecificationsVM>> Get()
        {
            var models = await _unitOfWork.SpecificationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SpecificationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<SpecificationsDTO> Get(int id)
        {
            var model = await _unitOfWork.SpecificationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<SpecificationsDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(SpecificationsDTO modelDto)
        {
            var model = _mapper.Map<Specifications>(modelDto);
            var duplicate = await _unitOfWork.SpecificationsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<SpecificationsDTO> Create(SpecificationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Specifications>(modelDto);
            _unitOfWork.SpecificationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<SpecificationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<SpecificationsDTO> Update(SpecificationsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.SpecificationsRepo.Update(_mapper.Map<Specifications>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.SpecificationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.SpecificationsRepo.Delete(model);
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
        public async Task<List<SpecificationsVM>> GetByOrder()
        {
            var models = await _unitOfWork.SpecificationsRepo.GetByOrder().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SpecificationsVM>>(models);
            return modelVms;
        }
    }
}
