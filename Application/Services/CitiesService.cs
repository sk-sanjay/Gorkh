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
    public class CitiesService : ICitiesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CitiesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<CitiesVM>> Get()
        {
            var models = await _unitOfWork.CitiesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<CitiesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<CitiesVM> Get(int id)
        {
            var model = await _unitOfWork.CitiesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<CitiesVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(CitiesDTO argModelDto)
        {
            var model = _mapper.Map<Cities>(argModelDto);
            var duplicate = await _unitOfWork.CitiesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }


        public async Task<CitiesDTO> Create(CitiesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Cities>(modelDto);
            _unitOfWork.CitiesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<CitiesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<CitiesDTO> Update(CitiesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.CitiesRepo.Update(_mapper.Map<Cities>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.CitiesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.CitiesRepo.Delete(model);
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
        public async Task<List<CitiesDTO>> CreateRange(List<CitiesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Cities>>(modelDtos);
            _unitOfWork.CitiesRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<CitiesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<CitiesDTO>> Upsert(List<CitiesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Cities>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.CitiesRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.CitiesRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.CitiesRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.CitiesRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<CitiesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<CitiesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.CitiesRepo.DeleteRange(_mapper.Map<List<Cities>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.CitiesRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<CitiesVM>> GetCitybystate(int sid)
        {
            var models = await _unitOfWork.CitiesRepo.GetCitybystate(sid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<CitiesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
