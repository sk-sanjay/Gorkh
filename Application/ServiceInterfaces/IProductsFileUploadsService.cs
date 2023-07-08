using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsFileUploadsService
    {
        //Common Methods
        Task<List<ProductsFileUploadsVM>> Get();
        Task<ProductsFileUploadsDTO> Get(int id);
        //Task<ProductsFileUploadsDTO> Create(ProductsFileUploadsDTO entity);
        Task<List<ProductsFileUploadsDTO>> CreateRange(List<ProductsFileUploadsDTO> entities);

        Task<ProductsFileUploadsDTO> Update(ProductsFileUploadsDTO entity);
        Task<int> Delete(int id);
        //Get Product Description by product id
        Task<List<ProductsFileUploadsVM>> GetProductsFileUploadsByProductId(int productid, int flagimage);
        Task<List<ProductsFileUploadsVM>> GetProductsFileUploadsByProductId(int productid);
    }
}
