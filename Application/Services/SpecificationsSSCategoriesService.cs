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
    class SpecificationsSSCategoriesService : ISpecificationsSSCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SpecificationsSSCategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<SpecificationsSSCategoriesVM>> Get()
        {
            var models = await _unitOfWork.SpecificationsSSCategoriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SpecificationsSSCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<SpecificationsSSCategoriesDTO> Get(int id)
        {
            var model = await _unitOfWork.SpecificationsSSCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<SpecificationsSSCategoriesDTO>(model);
            return modelDto;
        }
        //public async Task<bool> CheckDuplicate(SpecificationsSSCategoriesDTO modelDto)
        //{
        //    var model = _mapper.Map<SpecificationsSSCategories>(modelDto);
        //    var duplicate = await _unitOfWork.SpecificationsSSCategoriesRepo.CheckDuplicate(model).ConfigureAwait(false);
        //    return duplicate != null;
        //}
        public async Task<SpecificationsSSCategoriesDTO> Create(SpecificationsSSCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<SpecificationsSSCategories>(modelDto);
            _unitOfWork.SpecificationsSSCategoriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<SpecificationsSSCategoriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<SpecificationsSSCategoriesDTO> Update(SpecificationsSSCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.SpecificationsSSCategoriesRepo.Update(_mapper.Map<SpecificationsSSCategories>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.SpecificationsSSCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.SpecificationsSSCategoriesRepo.Delete(model);
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

        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.SpecificationsSSCategoriesRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        //public async Task<List<DropdownVM>> GetDropdownBySubcategory(int subcategoryid)
        //{
        //    var models = await _unitOfWork.SpecificationsSSCategoriesRepo.GetSubSubCategoryBySubCategory1(subcategoryid).ConfigureAwait(false);
        //    if (models == null || models.Count <= 0) return null;
        //    var modelVms = _mapper.Map<List<DropdownVM>>(models);
        //    if (modelVms == null || modelVms.Count <= 0) return null;
        //    return modelVms;
        //}

        public async Task<List<SpecificationsSSCategoriesDTO>> CreateRange(List<SpecificationsSSCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<SpecificationsSSCategories>>(modelDtos);
            _unitOfWork.SpecificationsSSCategoriesRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<SpecificationsSSCategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }

        public async Task<List<SpecificationsSSCategoriesVM>> GetSpecificationsSSCategories(int subsubcategoryid)
        {
            var models = await _unitOfWork.SpecificationsSSCategoriesRepo.GetSpecificationsSSCategories(subsubcategoryid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SpecificationsSSCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<int> UpdateSpecificationsSSCategories(List<SpecificationsSSCategoriesDTO> argModelDtos)
        {
            if (argModelDtos == null || argModelDtos.Count <= 0) return 0;
            int sscid = argModelDtos[0].SubSubCatId;
            var rawSSSCat = _mapper.Map<List<SpecificationsSSCategories>>(argModelDtos);
            var existingSSSCat = await _unitOfWork.SpecificationsSSCategoriesRepo.GetSpecificationsSSCategories(sscid).ConfigureAwait(false);
            var matchingSSSCat = new List<SpecificationsSSCategories>();
            var remainingSSSCat = new List<SpecificationsSSCategories>();
            foreach (var item in rawSSSCat)
            {
                //var match = existingRoleMenus.FirstOrDefault(erm => erm.RoleName == role && erm.MenuId == item.MenuId);
                var match = existingSSSCat.FirstOrDefault(erm => erm.SubSubCatId == sscid && erm.SpecfId == item.SpecfId);
                if (match != null)
                    matchingSSSCat.Add(match);
                else
                    remainingSSSCat.Add(item);
            }
            var nonMatchingSSSCat = existingSSSCat.Except(matchingSSSCat);
            foreach (var matchingSSSCat1 in matchingSSSCat)
                _unitOfWork.SpecificationsSSCategoriesRepo.Update(matchingSSSCat1);
            _unitOfWork.SpecificationsSSCategoriesRepo.CreateRange(remainingSSSCat);
            _unitOfWork.SpecificationsSSCategoriesRepo.DeleteRange(nonMatchingSSSCat);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? rowsChanged : 0;
        }

        public async Task<List<SpecificationsSSCategoriesVM>> GetSpecificationsSSCategoriesjoin(int subsubcategoryid)
        {
            var models = await _unitOfWork.SpecificationsSSCategoriesRepo.GetSpecificationsSSCategoriesjoin(subsubcategoryid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SpecificationsSSCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
    }
}
