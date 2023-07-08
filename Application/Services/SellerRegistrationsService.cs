using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SellerRegistrationsService : ISellerRegistrationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SellerRegistrationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Common Methods
        public async Task<List<SellerRegistrationsVM>> Get()
        {
            var models = await _unitOfWork.SellerRegistrationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SellerRegistrationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<SellerRegistrationsVM> Get(int id)
        {
            var model = await _unitOfWork.SellerRegistrationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<SellerRegistrationsVM>(model);
            return modelVM;
        }

        public async Task<SellerRegistrationsVM> GetbyEmail(string email)
        {
            var model = await _unitOfWork.SellerRegistrationsRepo.GetbyEmail(email).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<SellerRegistrationsVM>(model);
            return modelVM;
        }
        public async Task<SellerRegistrationsVM> GetbySellerId(int sellerid)
        {
            var model = await _unitOfWork.SellerRegistrationsRepo.GetbySellerId(sellerid).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<SellerRegistrationsVM>(model);
            return modelVM;
        }


        public async Task<bool> CheckEmail(string email)
        {
            var duplicate = await _unitOfWork.SellerRegistrationsRepo.CheckEmail(email).ConfigureAwait(false);
            return duplicate != null;
        }
        //public async Task<bool> CheckDuplicate(BuyersDTO argModelDto)
        //{
        //    var model = _mapper.Map<Buyers>(argModelDto);
        //    var duplicate = await _unitOfWork.BuyersRepo.CheckDuplicate(model).ConfigureAwait(false);
        //    return duplicate != null;
        //}

        public async Task<SellerRegistrationsDTO> Create(SellerRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<SellerRegistrations>(modelDto);
            _unitOfWork.SellerRegistrationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<SellerRegistrationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
    }
}
