using Application.ServiceInterfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.Services
{
    class Products1Service : IProducts1Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Products1Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<Products1VM>> GettProductsBySubCatId1(int catid, int subcategoryid, int countryId, int stateId, string saleType, int conditionId, string keyword)
        {
            if (keyword == "0")
            { keyword = string.Empty; }
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("@CategoryId", catid.ToString()),
                new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
                new KeyValuePair<string, string>("@CountryId", countryId.ToString()),
                new KeyValuePair<string, string>("@StateId", stateId.ToString()),
                new KeyValuePair<string, string>("@SaleType", saleType.ToString()),
                new KeyValuePair<string, string>("@ConditionId", conditionId.ToString()),
                new KeyValuePair<string, string>("@SearchText", keyword.ToString()),
            };
            var model = await _unitOfWork.Products1Repo.GetListFromSql("Sp_Products_GetBySubCatId", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<Products1VM>>(model);
            return modelVm;
        }

        public async Task<List<Products1VM>> GetProductsasfeaturedmachine()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.Products1Repo.GetListFromSql("Sp_Products_Featured_Machines", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<Products1VM>>(model);
            return modelVm;
        }

        public async Task<List<Products1VM>> GetLatestProducts()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.Products1Repo.GetListFromSql("Sp_Latest_Products", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<Products1VM>>(model);
            return modelVm;
        }


        public async Task<List<CategoryimgCommonVM>> GetCategoryImage()
        {
            List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
            {
                // new KeyValuePair<string, string>("@SubCategoryId", subcategoryid.ToString()),
            };
            var model = await _unitOfWork.ProductByCategoryRepositoryRepo.GetListFromSql("Sp_ProductByCategory", p).ConfigureAwait(false);
            if (model == null) return null;
            var modelVm = _mapper.Map<List<CategoryimgCommonVM>>(model);
            return modelVm;
        }



    }
}
