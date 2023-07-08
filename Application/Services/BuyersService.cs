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
    public class BuyersService : IBuyersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BuyersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<BuyersVM>> Get()
        {
            var models = await _unitOfWork.BuyersRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyersVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<BuyersVM> Get(int id)
        {
            var model = await _unitOfWork.BuyersRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyersVM>(model);
            return modelVM;
        }

        //public async Task<bool> CheckDuplicate(BuyersDTO argModelDto)
        //{
        //    var model = _mapper.Map<Buyers>(argModelDto);
        //    var duplicate = await _unitOfWork.BuyersRepo.CheckDuplicate(model).ConfigureAwait(false);
        //    return duplicate != null;
        //}

        public async Task<BuyersDTO> Create(BuyersDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Buyers>(modelDto);
            _unitOfWork.BuyersRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<BuyersDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<bool> CheckEmail(string email)
        {
            var duplicate = await _unitOfWork.BuyersRepo.CheckEmail(email).ConfigureAwait(false);
            return duplicate != null;
        }

        public async Task<BuyersVM> GetbyBuyerId(int buyerid)
        {
            var model = await _unitOfWork.BuyersRepo.GetbyBuyerId(buyerid).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyersVM>(model);
            return modelVM;
        }


    }
}
