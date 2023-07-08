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
    public class BuyerRequirementsService : IBuyerRequirementsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BuyerRequirementsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Common Methods
        public async Task<List<BuyerRequirementsVM>> Get()
        {
            var models = await _unitOfWork.BuyerRequirementsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerRequirementsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<BuyerRequirementsVM1>> GetBuyerRequirements()
        {
            var models = await _unitOfWork.BuyerRequirementsRepo.GetBuyerRequirements().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerRequirementsVM1>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<BuyerRequirementsVM1>> GetBuyerRequirementsforWebsite()
        {
            var models = await _unitOfWork.BuyerRequirementsRepo.GetBuyerRequirementsforWebsite().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerRequirementsVM1>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        

        public async Task<BuyerRequirementsVM1> GetBuyerRequirements(int id)
        {
            var models = await _unitOfWork.BuyerRequirementsRepo.GetBuyerRequirements(id).ConfigureAwait(false);
            if (models == null) return null;
            var modelVms = _mapper.Map<BuyerRequirementsVM1>(models);
            return modelVms;
        }
        public async Task<List<BuyerRequirementsVM1>> GetBuyerRequirementsbyusername(string email)
        {
            var models = await _unitOfWork.BuyerRequirementsRepo.GetBuyerRequirementsbyusername(email).ConfigureAwait(false);
            if (models == null) return null;
            var modelVms = _mapper.Map <List<BuyerRequirementsVM1>>(models);
            return modelVms;
        }
      

        public async Task<BuyerRequirementsVM> Get(int id)
        {
            var model = await _unitOfWork.CategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerRequirementsVM>(model);
            return modelVM;
        }


        public async Task<BuyerRequirementsDTO> Create(BuyerRequirementsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<BuyerRequirements>(modelDto);
            _unitOfWork.BuyerRequirementsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<BuyerRequirementsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<BuyerRequirementsDTO> Update(BuyerRequirementsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.BuyerRequirementsRepo.Update(_mapper.Map<BuyerRequirements>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.BuyerRequirementsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.BuyerRequirementsRepo.Delete(model);
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

        public async Task<List<BuyerRequirementsDTO>> CreateRange(List<BuyerRequirementsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<BuyerRequirements>>(modelDtos);
            _unitOfWork.BuyerRequirementsRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<BuyerRequirementsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<BuyerRequirementsDTO>> Upsert(List<BuyerRequirementsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<BuyerRequirements>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.BuyerRequirementsRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.BuyerRequirementsRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.BuyerRequirementsRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.BuyerRequirementsRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<BuyerRequirementsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<BuyerRequirementsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.BuyerRequirementsRepo.DeleteRange(_mapper.Map<List<BuyerRequirements>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
