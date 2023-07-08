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
    public class BannersService : IBannersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BannersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<BannersVM>> Get()
        {
            var models = await _unitOfWork.BannersRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BannersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<BannersDTO> Get(int id)
        {
            var model = await _unitOfWork.BannersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<BannersDTO>(model);
            return modelDto;
        }
        public async Task<BannersDTO> Create(BannersDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Banners>(modelDto);
            _unitOfWork.BannersRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<BannersDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<BannersDTO> Update(BannersDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.BannersRepo.Update(_mapper.Map<Banners>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.BannersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.BannersRepo.Delete(model);
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
        public async Task<List<BannersVM>> GetBannersForHomeSlider()
        {
            var models = await _unitOfWork.BannersRepo.GetBannersForHomeSlider().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BannersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
