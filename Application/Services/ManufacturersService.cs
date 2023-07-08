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
    public class ManufacturersService : IManufacturersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ManufacturersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ManufacturersVM>> Get()
        {
            var models = await _unitOfWork.ManufacturersRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ManufacturersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ManufacturersDTO> Get(int id)
        {
            var model = await _unitOfWork.ManufacturersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ManufacturersDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(ManufacturersDTO modelDto)
        {
            var model = _mapper.Map<Manufacturers>(modelDto);
            var duplicate = await _unitOfWork.ManufacturersRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<ManufacturersDTO> Create(ManufacturersDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Manufacturers>(modelDto);
            _unitOfWork.ManufacturersRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ManufacturersDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ManufacturersDTO> Update(ManufacturersDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ManufacturersRepo.Update(_mapper.Map<Manufacturers>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ManufacturersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ManufacturersRepo.Delete(model);
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
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.ManufacturersRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
