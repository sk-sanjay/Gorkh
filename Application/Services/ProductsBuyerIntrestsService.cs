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
    public class ProductsBuyerIntrestsService : IProductsBuyerIntrestsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
      
        public ProductsBuyerIntrestsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
          
        }
        //Common Methods
        public async Task<List<ProductsBuyerIntrestsVM>> Get()
        {
            var models = await _unitOfWork.ProductsBuyerIntrestsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerIntrestsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsBuyerIntrestsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsBuyerIntrestsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerIntrestsDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(ProductsBuyerIntrestsDTO modelDto)
        {
            var model = _mapper.Map<ProductsBuyerIntrests>(modelDto);
            var duplicate = await _unitOfWork.ProductsBuyerIntrestsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<ProductsBuyerIntrestsDTO> Create(ProductsBuyerIntrestsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsBuyerIntrests>(modelDto);
            _unitOfWork.ProductsBuyerIntrestsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsBuyerIntrestsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsBuyerIntrestsDTO> Update(ProductsBuyerIntrestsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsBuyerIntrestsRepo.Update(_mapper.Map<ProductsBuyerIntrests>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsBuyerIntrestsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsBuyerIntrestsRepo.Delete(model);
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
        public async Task<List<ProductsBuyerIntrestsCommonVM>> GetProductsBuyerIntrestsByBuyer(int buyerid)
        {
            var models = await _unitOfWork.ProductsBuyerIntrestsRepo.GetProductsBuyerIntrestsByBuyer(buyerid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerIntrestsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<ProductsBuyerIntrestsCommonVM>> GetProductsBuyerIntrestsForAdmin()
        {
            var models = await _unitOfWork.ProductsBuyerIntrestsRepo.GetProductsBuyerIntrestsForAdmin().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerIntrestsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsBuyerIntrestsDTO> GetProductsBuyerIntrestsByBuyerandPid(int buyerid, int productid)
        {
            var model = await _unitOfWork.ProductsBuyerIntrestsRepo.GetProductsBuyerIntrestsByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerIntrestsDTO>(model);
            return modelDto;
        }
    }
}
