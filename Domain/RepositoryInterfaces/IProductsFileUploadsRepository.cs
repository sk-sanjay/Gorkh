using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsFileUploadsRepository : IRepository<ProductsFileUploads>
    {
        //Get Product Image & Video by product id
        Task<List<ProductsFileUploads>> GetProductsFileUploadsByProductId(int productid, int flagimage);
        Task<List<ProductsFileUploads>> GetProductsFileUploadsByProductId(int productid);
    }
}
