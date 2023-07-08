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
    public class PaymentsService : IPaymentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PaymentsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<PaymentsVM>> Get()
        {
            var models = await _unitOfWork.PaymentsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<PaymentsDTO> Get(int id)
        {
            var model = await _unitOfWork.PaymentsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<PaymentsDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(PaymentsDTO modelDto)
        {
            var model = _mapper.Map<Payments>(modelDto);
            var duplicate = await _unitOfWork.PaymentsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<PaymentsDTO> Create(PaymentsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Payments>(modelDto);
            _unitOfWork.PaymentsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<PaymentsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<PaymentsDTO> Update(PaymentsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.PaymentsRepo.Update(_mapper.Map<Payments>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.PaymentsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.PaymentsRepo.Delete(model);
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
        public async Task<List<PaymentsCommonVM>> GetPaymentsForAdmin()
        {
            var models = await _unitOfWork.PaymentsRepo.GetPaymentsForAdmin().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<int> UpdatePaymentStatus(PaymentsUpdateDTO modelDto)
        {
            if (modelDto == null) return 0;
            var model = await _unitOfWork.PaymentsRepo.Get(modelDto.Id).ConfigureAwait(false);
            model.PaymentStatus = modelDto.PaymentStatus;
            model.RecDate = modelDto.RecDate;

            // _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(model));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }
        public async Task<PaymentsDTO> GetProductsPaymentsByBuyerandPid(int buyerid, int productid)
        {
            var model = await _unitOfWork.PaymentsRepo.GetProductsPaymentsByBuyerandPid(buyerid, productid).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<PaymentsDTO>(model);
            return modelDto;
        }
        public async Task<List<PaymentsCommonVM>> GetProductsPaymentsByBuyer(int buyerid)
        {
            var models = await _unitOfWork.PaymentsRepo.GetProductsPaymentsByBuyer(buyerid).ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<PaymentsCommonVM>> GetProductsPaymentsForAdmin()
        {
            var models = await _unitOfWork.PaymentsRepo.GetProductsPaymentsForAdmin().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<PaymentsCommonVM>> GetPaymentsByProductIdForAdmin(int productid)
        {
            var models = await _unitOfWork.PaymentsRepo.GetPaymentsByProductIdForAdmin(productid).ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<PaymentsCommonVM>> GetProductsPaymentsForSeller(int sellerid)
        {
            var models = await _unitOfWork.PaymentsRepo.GetProductsPaymentsForSeller(sellerid).ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<PaymentsCommonVM>> GetPaymentsByProductIdForSeller(int productid)
        {
            var models = await _unitOfWork.PaymentsRepo.GetPaymentsByProductIdForSeller(productid).ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<PaymentsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
