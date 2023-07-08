using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Application.ServiceInterfaces
{
    public interface IProductsService
    {
        //Common Methods
        Task<List<ProductsVM>> Get();
        Task<ProductsDTO> Get(int id);
        Task<ProductsDTO> Create(ProductsDTO entity);
        Task<ProductsDTO> Update(ProductsDTO entity);
        Task<int> Delete(int id);
        //Task<List<ProductsVM>> GetProductsBySeller(int sellerid);
        Task<List<ProductsBySellerVM>> GetProductsBySeller(int sellerid);
        Task<List<ProductsBySellerVM>> GetProductVisitor(int productid);
        Task<List<ProductsVM>> GetAllProductsForAdmin();
        Task<List<ProductsVM>> GetAllPendingProducts();
        Task<int> UpdateSelected(int id);
        Task<int> UpdateasFeatured(int id);
        Task<List<ProductsVM>> GettProductsBySubCatId(int subcategoryid);
        Task<List<RelatedProductsVM>> GetRelatedProducts(int subsubcatid);
        //Task<List<ProductsVM>> GetProductsasfeaturedmachine();
        //Task<List<ProductsViewModel>> GettProductsBySubCatId1(int subcategoryid);
        Task<List<ProductsViewModel>> GettProductsBySubCatId2(int subcategoryid);
        //Task<ProductsDetailsVM> GettProductsDetailsById(int id);
        Task<ProductsDetailsVM> GettProductsDetailsById(int id);
        Task<ProductsDetailsVM> GettProductsDetailsByIdwebsite(int id);
        Task<ProductsSellerDetailsVM> GetProductsSellerDetailsById(int id);
        Task<List<ProductsBreadCrumbsVM>> GetProductsBreadCrumbs(int catid, int subcatid);
        Task<int> FinalSubmitflagupdate(CommonDTO argModelDto);
    }
}
