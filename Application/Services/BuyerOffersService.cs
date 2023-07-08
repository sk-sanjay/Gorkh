using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    class BuyerOffersService : IBuyerOffersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BuyerOffersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        //Common Methods
        public async Task<List<BuyerOffersVM>> Get()
        {
            var models = await _unitOfWork.BuyerOffersRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerOffersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<BuyerOffersCommonVM>> GetBuyerOffersForAdmin()
        {
            var models = await _unitOfWork.BuyerOffersRepo.GetBuyerOffersForAdmin().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerOffersCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<BuyerOffersVM> Get(int id)
        {
            var model = await _unitOfWork.BuyerOffersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerOffersVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(BuyerOffersDTO argModelDto)
        {
            var model = _mapper.Map<BuyerOffers>(argModelDto);
            var duplicate = await _unitOfWork.BuyerOffersRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }



        public async Task<BuyerOffersDTO> Create(BuyerOffersDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<BuyerOffers>(modelDto);
            _unitOfWork.BuyerOffersRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<BuyerOffersDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<BuyerOffersDTO> Update(BuyerOffersDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.BuyerOffersRepo.Update(_mapper.Map<BuyerOffers>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.BuyerOffersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.BuyerOffersRepo.Delete(model);
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
        public async Task<List<BuyerOffersDTO>> CreateRange(List<BuyerOffersDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<BuyerOffers>>(modelDtos);
            _unitOfWork.BuyerOffersRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<BuyerOffersDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<BuyerOffersDTO>> Upsert(List<BuyerOffersDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<BuyerOffers>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.BuyerOffersRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.BuyerOffersRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.BuyerOffersRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.BuyerOffersRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<BuyerOffersDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<BuyerOffersDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.BuyerOffersRepo.DeleteRange(_mapper.Map<List<BuyerOffers>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        //Update BuyerOffers
        public async Task<BuyerOffersDTO> UpdateBuyerofers(BuyerOffersDTO modelDto)
        {
            if (modelDto == null) return null;
            //var model = await _unitOfWork.BuyerOffersRepo.Get(modelDto.Id).ConfigureAwait(false);
            var model1 = await _unitOfWork.BuyerOffersRepo.GetBuyerOffersByProductNumber(modelDto.ProductNumber).ConfigureAwait(false);
            
            
            foreach (var item in model1)
            {
                if (item.Id== modelDto.Id)
                {
                    item.IsSoled = "SOLD";
                }
                else
                {
                    item.IsSoled = "OTHER";
                }

            }
            _unitOfWork.BuyerOffersRepo.UpdateRange(model1);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
    }
}
