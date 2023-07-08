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
    public class ProductsPurchasesService : IProductsPurchasesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsPurchasesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsPurchasesVM>> Get()
        {
            var models = await _unitOfWork.ProductsPurchasesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsPurchasesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsPurchasesDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsPurchasesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsPurchasesDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(ProductsPurchasesDTO modelDto)
        {
            var model = _mapper.Map<ProductsPurchases>(modelDto);
            var duplicate = await _unitOfWork.ProductsPurchasesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<ProductsPurchasesDTO> Create(ProductsPurchasesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsPurchases>(modelDto);
            _unitOfWork.ProductsPurchasesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsPurchasesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsPurchasesDTO> Update(ProductsPurchasesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsPurchasesRepo.Update(_mapper.Map<ProductsPurchases>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsPurchasesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsPurchasesRepo.Delete(model);
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
        public async Task<List<ProductsPurchasesCommonVM>> GetProductsPurchasesByBuyer(int buyerid)
        {
            var models = await _unitOfWork.ProductsPurchasesRepo.GetProductsPurchasesByBuyer(buyerid).ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsPurchasesCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<ProductsPurchasesCommonVM>> GetProductsPurchasesForAdmin()
        {
            var models = await _unitOfWork.ProductsPurchasesRepo.GetProductsPurchasesForAdmin().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsPurchasesCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsPurchasesDTO> GetProductsPurchasesByBuyerandPid(int buyerid, int productid)
        {
            var model = await _unitOfWork.ProductsPurchasesRepo.GetProductsPurchasesByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsPurchasesDTO>(model);
            return modelDto;
        }
    }
}
