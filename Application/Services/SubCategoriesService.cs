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
    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubCategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<SubCategoriesVM>> Get()
        {
            var models = await _unitOfWork.SubCategoriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        //public async Task<List<SubCategoriesVM>> GetCategory()
        //{
        //    var models = await _unitOfWork.SubCategoriesRepo.GetCategory().ConfigureAwait(false);
        //    if (models == null || models.Count <= 0) return null;
        //    var modelVms = _mapper.Map<List<SubCategoriesVM>>(models);
        //    if (modelVms == null || modelVms.Count <= 0) return null;
        //    return modelVms;
        //}

        public async Task<SubCategoriesVM> Get(int id)
        {
            var model = await _unitOfWork.SubCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<SubCategoriesVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(SubCategoriesDTO argModelDto)
        {
            var model = _mapper.Map<SubCategories>(argModelDto);
            var duplicate = await _unitOfWork.SubCategoriesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }

        public async Task<SubCategoriesDTO> Create(SubCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<SubCategories>(modelDto);
            _unitOfWork.SubCategoriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<SubCategoriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<SubCategoriesDTO> Update(SubCategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.SubCategoriesRepo.Update(_mapper.Map<SubCategories>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.SubCategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.SubCategoriesRepo.Delete(model);
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

        public async Task<List<SubCategoriesDTO>> CreateRange(List<SubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<SubCategories>>(modelDtos);
            _unitOfWork.SubCategoriesRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<SubCategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<SubCategoriesDTO>> Upsert(List<SubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<SubCategories>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.SubCategoriesRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.SubCategoriesRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.SubCategoriesRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.SubCategoriesRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<SubCategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<SubCategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.SubCategoriesRepo.DeleteRange(_mapper.Map<List<SubCategories>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }


        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            var models = await _unitOfWork.SubCategoriesRepo.GetActive().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<List<DropdownVM>> GetDropdownByCategory(int categoryid)
        {
            var models = await _unitOfWork.SubCategoriesRepo.GetSubCategoryByCategory(categoryid).ConfigureAwait(false);
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
        public async Task<List<SubCategoriesVM>> GetSubcategory(int maincat)
        {
            var models = await _unitOfWork.SubCategoriesRepo.GetSubcategory(maincat).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<SubCategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

    }

}

