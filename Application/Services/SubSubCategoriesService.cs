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
    public class SubSubCategoriesService : ISubSubCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubSubCategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        //Common Methods
        public async Task<List<SubSubCategoriesVM>> Get()
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubSubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<SubSubCategoriesVM> Get(int id)
        {
            var model = await _unitOfWork.SubSubCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<SubSubCategoriesVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(SubSubCategoriesDTO argModelDto)
        {
            var model = _mapper.Map<SubSubCategories>(argModelDto);
            var duplicate = await _unitOfWork.SubSubCategoriesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }


        public async Task<SubSubCategoriesDTO> Create(SubSubCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<SubSubCategories>(modelDto);
            _unitOfWork.SubSubCategoriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<SubSubCategoriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<SubSubCategoriesDTO> Update(SubSubCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.SubSubCategoriesRepo.Update(_mapper.Map<SubSubCategories>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.SubSubCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.SubSubCategoriesRepo.Delete(model);
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
        public async Task<List<SubSubCategoriesDTO>> CreateRange(List<SubSubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<SubSubCategories>>(modelDtos);
            _unitOfWork.SubSubCategoriesRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<SubSubCategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<SubSubCategoriesDTO>> Upsert(List<SubSubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<SubSubCategories>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.SubSubCategoriesRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.SubSubCategoriesRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.SubSubCategoriesRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.SubSubCategoriesRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<SubSubCategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<SubSubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.SubSubCategoriesRepo.DeleteRange(_mapper.Map<List<SubSubCategories>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }




        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<DropdownVM>> GetDropdownByCategory(int categoryid)
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.GetSubSubCategoryBySubCategory(categoryid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            ////Move Kerala to first position
            //var index = modelVms.FindIndex(x => x.Text == "Kerala");
            //var item = modelVms[index];
            //modelVms[index] = modelVms[0];
            //modelVms[0] = item;
            return modelVms;
        }
        public async Task<List<SubSubCategoriesVM>> GetSubcategory(int maincat)
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.GetSubcategory(maincat).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubSubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<List<SubSubCategoriesVM>> GetSubSubCategoryBySubCategory2(int subcategoryid)
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.GetSubSubCategoryBySubCategory2(subcategoryid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubSubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<SubSubCategoriesVM>> SearchSubSubCategory(string prefix)
        {
            var models = await _unitOfWork.SubSubCategoriesRepo.SearchSubSubCategory(prefix).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubSubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

    }

}
