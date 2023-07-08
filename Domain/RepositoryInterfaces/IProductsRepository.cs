using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsRepository : IRepository<Products>
    {
        Task<List<ProductsBySeller>> GetProductsBySeller(int sellerid);
        Task<List<ProductsBySeller>> GetProductVisitor(int productid);
        Task<List<Products>> GetAllProductsForAdmin();
        Task<List<Products>> GetAllPendingProducts();
        Task<List<Products>> GettProductsBySubCatId(int subcategoryid);
        Task<List<RelatedProducts>> GetRelatedProducts(int subsubcatid);
        //Task<List<Products1>> GettProductsBySubCatId1(int subcategoryid);
        //Task<List<ProductsDetails>> GettProductsDetailsById(int id);
        Task<List<ProductsDetails>> GetProductsDetailsById(int id);
        Task<ProductsDetails> GettProductsDetailsById(int id);
        Task<ProductsDetails> GettProductsDetailsByIdwebsite(int id);
        //Task<ProductsDetails> GetProductsasfeaturedmachine();
        
        Task<ProductsSellerDetails> GetProductsSellerDetailsById(int id);
        Task<List<ProductsBreadCrumbs>> GetProductsBreadCrumbs(int catid,int subcatid);
    }
}
