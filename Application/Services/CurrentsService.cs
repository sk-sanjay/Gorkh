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
    class CurrentsService : ICurrentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CurrentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<CurrentsVM>> Get()
        {
            var models = await _unitOfWork.CurrentsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<CurrentsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<CurrentsDTO> Get(int id)
        {
            var model = await _unitOfWork.CurrentsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<CurrentsDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(CurrentsDTO modelDto)
        {
            var model = _mapper.Map<Currents>(modelDto);
            var duplicate = await _unitOfWork.CurrentsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<CurrentsDTO> Create(CurrentsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Currents>(modelDto);
            _unitOfWork.CurrentsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<CurrentsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<CurrentsDTO> Update(CurrentsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.CurrentsRepo.Update(_mapper.Map<Currents>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.CurrentsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.CurrentsRepo.Delete(model);
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
