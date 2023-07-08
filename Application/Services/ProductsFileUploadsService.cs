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
    class ProductsFileUploadsService : IProductsFileUploadsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsFileUploadsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsFileUploadsVM>> Get()
        {
            var models = await _unitOfWork.ProductsFileUploadsRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsFileUploadsVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsFileUploadsDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsFileUploadsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsFileUploadsDTO>(model);
            return modelDto;
        }
        //public async Task<ProductsFileUploadsDTO> Create(ProductsFileUploadsDTO modelDto)
        //{
        //    if (modelDto == null) return null;
        //    var model = _mapper.Map<ProductsFileUploads>(modelDto);
        //    _unitOfWork.ProductsFileUploadsRepo.Create(model);
        //    var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        //    modelDto = _mapper.Map<ProductsFileUploadsDTO>(model);
        //    return rowsChanged > 0 ? modelDto : null;
        //}
        public async Task<List<ProductsFileUploadsDTO>> CreateRange(List<ProductsFileUploadsDTO> modelDtos)
        {
            if (modelDtos == null || modelDtos.Count <= 0) return null;
            var models = _mapper.Map<List<ProductsFileUploads>>(modelDtos);
            _unitOfWork.ProductsFileUploadsRepo.CreateRange(models);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDtos = _mapper.Map<List<ProductsFileUploadsDTO>>(models);
            return rowsChanged > 0 ? modelDtos : null;
        }
        public async Task<ProductsFileUploadsDTO> Update(ProductsFileUploadsDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsFileUploadsRepo.Update(_mapper.Map<ProductsFileUploads>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsFileUploadsRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsFileUploadsRepo.Delete(model);
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
        public async Task<List<ProductsFileUploadsVM>> GetProductsFileUploadsByProductId(int productid, int flagimage)
        {
            var model = await _unitOfWork.ProductsFileUploadsRepo.GetProductsFileUploadsByProductId(productid, flagimage).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<List<ProductsFileUploadsVM>>(model);
            return modelDto;
        }

        public async Task<List<ProductsFileUploadsVM>> GetProductsFileUploadsByProductId(int productid)
        {
            var model = await _unitOfWork.ProductsFileUploadsRepo.GetProductsFileUploadsByProductId(productid).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<List<ProductsFileUploadsVM>>(model);
            return modelDto;
        }
    }
}
