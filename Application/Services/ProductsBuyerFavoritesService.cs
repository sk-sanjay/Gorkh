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
   public class ProductsBuyerFavoritesService : IProductsBuyerFavoritesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductsBuyerFavoritesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Common Methods
        public async Task<List<ProductsBuyerFavoritesVM>> Get()
        {
            var models = await _unitOfWork.ProductsBuyerFavoritesRepo.Get().ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<ProductsBuyerFavoritesVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }
        public async Task<ProductsBuyerFavoritesDTO> Get(int id)
        {
            var model = await _unitOfWork.ProductsBuyerFavoritesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return null;
            var modelDto = _mapper.Map<ProductsBuyerFavoritesDTO>(model);
            return modelDto;
        }
        public async Task<bool> CheckDuplicate(ProductsBuyerFavoritesDTO modelDto)
        {
            var model = _mapper.Map<ProductsBuyerFavorites>(modelDto);
            var duplicate = await _unitOfWork.ProductsBuyerFavoritesRepo.CheckDuplicate(model).ConfigureAwait(false);
            return duplicate != null;
        }
        public async Task<ProductsBuyerFavoritesDTO> Create(ProductsBuyerFavoritesDTO modelDto)
        {
            if (modelDto == null) return null;
            var model = _mapper.Map<ProductsBuyerFavorites>(modelDto);
            _unitOfWork.ProductsBuyerFavoritesRepo.Create(model);
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            modelDto = _mapper.Map<ProductsBuyerFavoritesDTO>(model);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<ProductsBuyerFavoritesDTO> Update(ProductsBuyerFavoritesDTO modelDto)
        {
            if (modelDto == null) return null;
            _unitOfWork.ProductsBuyerFavoritesRepo.Update(_mapper.Map<ProductsBuyerFavorites>(modelDto));
            var rowsChanged = await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return rowsChanged > 0 ? modelDto : null;
        }
        public async Task<int> Delete(int id)
        {
            var model = await _unitOfWork.ProductsBuyerFavoritesRepo.Get(id).ConfigureAwait(false);
            if (model == null) return -1;
            _unitOfWork.ProductsBuyerFavoritesRepo.Delete(model);
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

        public async Task<List<BuyerFavouriteProductsCommonVM>> GetFavoutiteProductsbybuyerid(int buyerid)
        {
            var models = await _unitOfWork.ProductsBuyerFavoritesRepo.GetFavoutiteProductsbybuyerid(buyerid).ConfigureAwait(false);
            if (models == null || models.Count <= 0) return null;
            var modelVms = _mapper.Map<List<BuyerFavouriteProductsCommonVM>>(models);
            if (modelVms == null || modelVms.Count <= 0) return null;
            return modelVms;
        }

    }
}
