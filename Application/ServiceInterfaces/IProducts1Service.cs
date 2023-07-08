using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IProducts1Service
    {
        Task<List<Products1VM>> GettProductsBySubCatId1(int catid, int subcategoryid, int countryId, int stateId, string saleType, int conditionId, string keyword);
        Task<List<Products1VM>> GetProductsasfeaturedmachine();
        Task<List<Products1VM>> GetLatestProducts();
        Task<List<CategoryimgCommonVM>> GetCategoryImage();
    }
}
