using Domain.Models;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsDescriptionsRepository : IRepository<ProductsDescriptions>
    {
        //Get Product Description by product id
        Task<ProductsDescriptions> GetProductsDescriptionsByProductId(int productid);
        Task<ProductsDescriptions> GetByProductId(int id);
    }
}
