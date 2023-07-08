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
    public class BuyerSellerRegistrationsService : IBuyerSellerRegistrationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuyerSellerRegistrationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<BuyerSellerRegistrationsVM>> Get()
        {
            var models = await _unitOfWork.BuyerSellerRegistrationsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerSellerRegistrationsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
           
        }

        public async Task<BuyerSellerRegistrationsVM> Get(int id)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerSellerRegistrationsVM>(model);
            return modelVM;
        }

        public async Task<BuyerCommonDTO> Update(BuyerCommonDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        //Update Buyer Profile
        public async Task<BuyerSellerRegistrationsDTO> UpdateBuyerDetails(BuyerSellerRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);

            model.FirstName = modelDto.FirstName;
            model.LastName = modelDto.LastName;
            model.CompanyName = modelDto.CompanyName;
            model.CompanyWebsite = modelDto.CompanyWebsite;
            model.Mobile = modelDto.Mobile;
            model.OrganisationType = modelDto.OrganisationType;
            model.Organisation = modelDto.Organisation;
            model.Department = modelDto.Department;
            model.Country = modelDto.Country;
            model.State = modelDto.State;
            model.City = modelDto.City;
            model.AddressLine1 = modelDto.AddressLine1;
            model.AddressLine2 = modelDto.AddressLine2;
            model.ZipCode = modelDto.ZipCode;
            _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(model));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        //Update Seller Profile

        public async Task<BuyerSellerRegistrationsDTO> UpdateSellerProfile(BuyerSellerRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);

            model.FirstName = modelDto.FirstName;
            model.LastName = modelDto.LastName;
            model.CompanyName = modelDto.CompanyName;
            model.CompanyWebsite = modelDto.CompanyWebsite;
            model.Mobile = modelDto.Mobile;
           // model.OrganisationType = modelDto.OrganisationType;
            model.Organisation = modelDto.Organisation;
            model.Department = modelDto.Department;
            model.Country = modelDto.Country;
            model.State = modelDto.State;
            model.City = modelDto.City;
            model.AddressLine1 = modelDto.AddressLine1;
            model.AddressLine2 = modelDto.AddressLine2;
            model.ZipCode = modelDto.ZipCode;
            model.Descriptionofproduct = modelDto.Descriptionofproduct;
            _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(model));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        

        public async Task<int> UpdateOrganisationType(BuyerCommonDTO modelDto)
        {
            if (modelDto == null) return 0;
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);
            model.OrganisationType = modelDto.OrganisationType;

            // _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(model));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }

        public async Task<int> UpdateDetails(SellerCommonDTO modelDto)
        {
            if (modelDto == null) return 0;
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.Get(modelDto.Id).ConfigureAwait(false);
            model.CompanyName = modelDto.CompanyName;
            model.CompanyWebsite = modelDto.CompanyWebsite;
            model.LandlineNo = modelDto.LandlineNo;
            model.Descriptionofproduct = modelDto.Descriptionofproduct;

            // _unitOfWork.BuyerSellerRegistrationsRepo.Update(_mapper.Map<BuyerSellerRegistrations>(model));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged;
        }



        public async Task<BuyerSellerRegistrationsVM> GetbyEmail(string email)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.GetbyEmail(email).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerSellerRegistrationsVM>(model);
            return modelVM;
        }

        public async Task<BuyerSellerRegistrationsVM> GetbyMobile(string mobile)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.GetbyMobile(mobile).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerSellerRegistrationsVM>(model);
            return modelVM;
        }

        public async Task<BuyerSellerRegistrationsVM> GetbySellerId(int sellerid)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.GetbySellerId(sellerid).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerSellerRegistrationsVM>(model);
            return modelVM;
        }


        public async Task<bool> CheckEmail(string email)
        {
            var duplicate = await _unitOfWork.BuyerSellerRegistrationsRepo.CheckEmail(email).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<bool> CheckDuplicate(BuyerSellerRegistrationsDTO argModelDto)
        {
            var model = _mapper.Map<BuyerSellerRegistrations>(argModelDto);
            var duplicate = await _unitOfWork.BuyerSellerRegistrationsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }

        public async Task<BuyerSellerRegistrationsDTO> Create(BuyerSellerRegistrationsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<BuyerSellerRegistrations>(modelDto);
            _unitOfWork.BuyerSellerRegistrationsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<BuyerSellerRegistrationsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<BuyerSellerRegistrationsVM> GetbyBuyerId(int buyerid)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.GetbyBuyerId(buyerid).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<BuyerSellerRegistrationsVM>(model);
            return modelVM;
        }

        public async Task<List<BuyerSellerRegistrationsVM>> GetdropdownbySellerId(int orgid)
        {
            var model = await _unitOfWork.BuyerSellerRegistrationsRepo.GetdropdownbySellerId(orgid).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<List<BuyerSellerRegistrationsVM>>(model);
            return modelVM;
        }

        public async Task<List<BuyerSellerRegistrations1VM>> GetBuyersandSellers()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {

            };
            var model = await _unitOfWork.BuyerSellerRegistrations1Repo.GetListFromSql("Sp_BuyerSellerData", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<BuyerSellerRegistrations1VM>>(model);
            return modelVm;
        }

        public async Task<List<BuyerSellerRegistrations1VM>> GetBuyersData()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {

            };
            var model = await _unitOfWork.BuyerSellerRegistrations1Repo.GetListFromSql("Sp_BuyerData", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<BuyerSellerRegistrations1VM>>(model);
            return modelVm;
        }
        public async Task<List<BuyerSellerRegistrations1VM>> GetSellerData()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {

            };
            var model = await _unitOfWork.BuyerSellerRegistrations1Repo.GetListFromSql("Sp_SellerData", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<BuyerSellerRegistrations1VM>>(model);
            return modelVm;
        }
        public async Task<List<BuyerSellerRegistrations1VM>> GetBothData()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {

            };
            var model = await _unitOfWork.BuyerSellerRegistrations1Repo.GetListFromSql("Sp_Buyer_Seller_Data", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<BuyerSellerRegistrations1VM>>(model);
            return modelVm;
        }
    }
}
