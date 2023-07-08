using Application.Dtos;
using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
   public class TempOtherLinkService : ITempOtherLinkHeadingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TempOtherLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<TempOtherLinkVM>> Get()
        {
            var models = await _unitOfWork.TempOtherLinkRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var tempModelVms = _mapper.Map<List<TempOtherLinkVM>>(models);
            if (tempModelVms == null || tempModelVms.Count <= 0) return null;
            return tempModelVms.OrderBy(x => x.Priority).ToList();
        }

        public async Task<TempOtherLinkHeadingDTO> Get(int id)
        {
            var tempModels = await _unitOfWork.TempOtherLinkRepo.Get(id).ConfigureAwait(false);
            if (tempModels == null) return null;
            var tempModelsDto = _mapper.Map<TempOtherLinkHeadingDTO>(tempModels);
            return tempModelsDto;
        }

        public async Task<TempOtherLinkHeadingDTO> Add(TempOtherLinkHeadingDTO modelDto)
        {
            if (modelDto == null) return null;
            var tempModel = _mapper.Map<TempOtherLinkHeading>(modelDto);
            _unitOfWork.TempOtherLinkRepo.Create(tempModel);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<TempOtherLinkHeadingDTO> Update(TempOtherLinkHeadingDTO modelDto)
        {
            if (modelDto == null) return null;
            var tempModel = _mapper.Map<TempOtherLinkHeading>(modelDto);
            _unitOfWork.TempOtherLinkRepo.Update(tempModel);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Remove(int id)
        {
            var tempModel = await _unitOfWork.TempOtherLinkRepo.Get(id).ConfigureAwait(false);
            if (tempModel == null) return -1;
            _unitOfWork.TempOtherLinkRepo.Delete(tempModel);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? rowsChanged : -1;
        }

        //Custom Methods
        public async Task<TempOtherLinkHeadingDTO> CreateAudit(TempOtherLinkHeadingDTO modelDto)
        {
            if (modelDto == null) return null;
            var tempModel = _mapper.Map<TempOtherLinkHeading>(modelDto);
            _unitOfWork.TempOtherLinkRepo.CreateAudit(tempModel);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<List<TempOtherLinkVM>> GetByAction(string status)
        {
            var Tempproduct = await _unitOfWork.TempOtherLinkRepo.GetByAction(status).ConfigureAwait(false);
            if (Tempproduct == null || Tempproduct.Count <= 0) return null;
            var TempproductVms = _mapper.Map<List<TempOtherLinkVM>>(Tempproduct);
            return TempproductVms;
        }

    }
}
