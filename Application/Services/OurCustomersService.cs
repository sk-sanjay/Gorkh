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
    class OurCustomersService : IOurCustomersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OurCustomersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<OurCustomersVM>> Get()
        {
            var models = await _unitOfWork.OurCustomersRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<OurCustomersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<OurCustomersDTO> Get(int id)
        {
            var model = await _unitOfWork.OurCustomersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<OurCustomersDTO>(model);
            return modelDto;
        }
        public async Task<OurCustomersDTO> Create(OurCustomersDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<OurCustomers>(modelDto);
            _unitOfWork.OurCustomersRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<OurCustomersDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<OurCustomersDTO> Update(OurCustomersDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.OurCustomersRepo.Update(_mapper.Map<OurCustomers>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.OurCustomersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.OurCustomersRepo.Delete(model);
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
        public async Task<List<OurCustomersVM>> GetOurCustomersHomePage()
        {
            var models = await _unitOfWork.OurCustomersRepo.GetOurCustomersHomePage().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<OurCustomersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}