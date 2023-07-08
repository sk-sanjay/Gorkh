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
   public class OtherLinkService : IOtherLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OtherLinkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<OtherLinkDTO>> Get()
        {
            var categories = await _unitOfWork.OtherLinkRepo.Get().ConfigureAwait(false);
            if (categories == null || categories.Count <= 0) return null;
            var categoryDtos = _mapper.Map<List<OtherLinkDTO>>(categories);
            if (categoryDtos == null || categoryDtos.Count <= 0) return null;
            return categoryDtos;
        }


        public async Task<OtherLinkDTO> Add(OtherLinkDTO modelDto)
        {
            if (modelDto == null) return null;
            var heading = _mapper.Map<OtherLinkHeading>(modelDto);
            _unitOfWork.OtherLinkRepo.Create(heading);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto.Id = heading.Id;
            return rowsChanged > 0 ? modelDto : null;
        }
        //Custom Methods

        public async Task<List<OtherLinkVM>> GetCategoriesWithAll()
        {
            var categories = await _unitOfWork.OtherLinkRepo.GetCategoriesWithAll().ConfigureAwait(false);
            if (categories == null || categories.Count <= 0) return null;
            var categoryVms = _mapper.Map<List<OtherLinkVM>>(categories);
            return categoryVms;
        }

        public async Task<OtherLinkDTO> Get(int id)
        {
            var category = await _unitOfWork.OtherLinkRepo.Get(id).ConfigureAwait(false);
            if (category == null) return null;
            var categoryDto = _mapper.Map<OtherLinkDTO>(category);
            return categoryDto;
        }

        public async Task<OtherLinkVM> Get(string Heading)
        {
            var category = await _unitOfWork.OtherLinkRepo.Get(Heading).ConfigureAwait(false);
            if (category == null) return null;
            var categoryDto = _mapper.Map<OtherLinkVM>(category);
            return categoryDto;
        }
        public async Task<OtherLinkDTO> Update(OtherLinkDTO modelDto)
        {
            if (modelDto == null) return null;
            var category = _mapper.Map<OtherLinkHeading>(modelDto);
            _unitOfWork.OtherLinkRepo.Update(category);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<OtherLinkDTO> EditMenuContent(OtherLinkDTO modelDTO)
        {
            OtherLinkHeading menu = _mapper.Map<OtherLinkHeading>(modelDTO);
            _unitOfWork.OtherLinkRepo.Update(menu);
            int Result = await _unitOfWork.SaveChangesAsync();
            if (Result > 0) return modelDTO;
            return null;
        }
        //public async Task<int> Remove(int id)
        //{
        //    var category = await _unitOfWork.OtherLinkRepo.Get(id).ConfigureAwait(false);
        //    if (category == null) return -1;
        //    _unitOfWork.OtherLinkRepo.Remove(category);
        //    var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        //    return rowsChanged > 0 ? rowsChanged : -1;
        //}
    }
}
