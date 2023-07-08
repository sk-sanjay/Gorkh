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
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //Common Methods
        public async Task<List<CategoriesVM>> Get()
        {
            var models = await _unitOfWork.CategoriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<CategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<CategoriesVM> Get(int id)
        {
            var model = await _unitOfWork.CategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<CategoriesVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(CategoriesDTO argModelDto)
        {
            var model = _mapper.Map<Categories>(argModelDto);
            var duplicate = await _unitOfWork.CategoriesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }

        public async Task<CategoriesDTO> Create(CategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Categories>(modelDto);
            _unitOfWork.CategoriesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<CategoriesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<CategoriesDTO> Update(CategoriesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.CategoriesRepo.Update(_mapper.Map<Categories>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.CategoriesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.CategoriesRepo.Delete(model);
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

        public async Task<List<CategoriesDTO>> CreateRange(List<CategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Categories>>(modelDtos);
            _unitOfWork.CategoriesRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<CategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<CategoriesDTO>> Upsert(List<CategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Categories>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.CategoriesRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.CategoriesRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.CategoriesRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.CategoriesRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<CategoriesDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<CategoriesDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.CategoriesRepo.DeleteRange(_mapper.Map<List<Categories>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }





        //Custom Methods
        public async Task<List<DropdownVM>> GetDropdown()
        {
            //var models = await _unitOfWork.CategoriesRepo.GetActive().ConfigureAwait(false);
            var models = await _unitOfWork.CategoriesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<DropdownVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            ////Move India to first position
            //var index = modelVms.FindIndex(x => x.Text == "India");
            //var item = modelVms[index];
            //modelVms[index] = modelVms[0];
            //modelVms[0] = item;
            return modelVms;
        }

        public async Task<List<CategoriesVM>> GetAllCategoryWithChild()
        {
            var models = await _unitOfWork.CategoriesRepo.GetAllCategoryWithChild().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<CategoriesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }


    }

}
