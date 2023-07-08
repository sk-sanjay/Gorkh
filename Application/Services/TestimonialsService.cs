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
    public class TestimonialsService : ITestimonialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestimonialsService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        //Common Methods
        public async Task<List<TestimonialsVM>> Get()
        {
            var models = await _unitOfWork.TestimonialsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<TestimonialsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

        public async Task<TestimonialsVM> Get(int id)
        {
            var model = await _unitOfWork.TestimonialsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelVM = _mapper.Map<TestimonialsVM>(model);
            return modelVM;
        }

        public async Task<bool> CheckDuplicate(TestimonialsDTO argModelDto)
        {
            var model = _mapper.Map<Testimonials>(argModelDto);
            var duplicate = await _unitOfWork.TestimonialsRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }

        public async Task<TestimonialsDTO> Create(TestimonialsDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<Testimonials>(modelDto);
            _unitOfWork.TestimonialsRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<TestimonialsDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<TestimonialsDTO> Update(TestimonialsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.TestimonialsRepo.Update(_mapper.Map<Testimonials>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }

        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.TestimonialsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.TestimonialsRepo.Delete(model);
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

        public async Task<List<TestimonialsDTO>> CreateRange(List<TestimonialsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Testimonials>>(modelDtos);
            _unitOfWork.TestimonialsRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<TestimonialsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<List<TestimonialsDTO>> Upsert(List<TestimonialsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<Testimonials>>(modelDtos);
            foreach (var row in models)
            {
                if (_unitOfWork.TestimonialsRepo.GetEntityState(row) != EntityState.Detached) continue;
                var ExistingRow = await _unitOfWork.TestimonialsRepo.Get(row.Id).ConfigureAwait(false);
                if (ExistingRow != null)
                {
                    var attachedEntry = _unitOfWork.TestimonialsRepo.GetEntityEntry(ExistingRow);
                    attachedEntry.CurrentValues.SetValues(row);
                }
                else
                {
                    _unitOfWork.TestimonialsRepo.Create(row);
                }
            }
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<TestimonialsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<int> DeleteRange(List<TestimonialsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return -1;
            _unitOfWork.TestimonialsRepo.DeleteRange(_mapper.Map<List<Testimonials>>(modelDtos));
            return await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }





        //Custom Methods


        public async Task<List<TestimonialsVM>> GetTestimonials()
        {
            var models = await _unitOfWork.TestimonialsRepo.GetTestimonials().ConfigureAwait(false);

            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<TestimonialsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }


    }
}
