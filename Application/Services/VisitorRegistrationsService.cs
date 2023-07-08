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

    public class VisitorRegistrationsService : IVisitorRegistrationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VisitorRegistrationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Common Methods
        public async Task<List<VisitorRegistrationsVM>> Get()
        {
            var models = await _unitOfWork.VisitorRegistrationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<VisitorRegistrationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<VisitorRegistrationsVM> Get(int id)
        {
            var model = await _unitOfWork.CategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<VisitorRegistrationsVM>(model);
            return modelVM;
        }


        public async Task<VisitorRegistrationsDTO> Create(VisitorRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<VisitorRegistrations>(modelDto);
            _unitOfWork.VisitorRegistrationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<VisitorRegistrationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<VisitorRegistrationsDTO> Update(VisitorRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.VisitorRegistrationsRepo.Update(_mapper.Map<VisitorRegistrations>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.VisitorRegistrationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.VisitorRegistrationsRepo.Delete(model);
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

        public async Task<List<VisitorRegistrationsDTO>> CreateRange(List<VisitorRegistrationsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<VisitorRegistrations>>(modelDtos);
            _unitOfWork.VisitorRegistrationsRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<VisitorRegistrationsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<VisitorRegistrationsDTO>> Upsert(List<VisitorRegistrationsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<VisitorRegistrations>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.VisitorRegistrationsRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.VisitorRegistrationsRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.VisitorRegistrationsRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.VisitorRegistrationsRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<VisitorRegistrationsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<VisitorRegistrationsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.VisitorRegistrationsRepo.DeleteRange(_mapper.Map<List<VisitorRegistrations>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<bool> CheckDuplicate(VisitorRegistrationsDTO argModelDto)
        {
            var model = _mapper.Map<VisitorRegistrations>(argModelDto);
            var duplicate = await _unitOfWork.VisitorRegistrationsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<VisitorRegistrationsVM> GetbyEmailId(string email)
        {
            var model = await _unitOfWork.VisitorRegistrationsRepo.GetbyEmailId(email).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<VisitorRegistrationsVM>(model);
            return modelVM;
        }

    }
}