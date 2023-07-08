using Application.Dtos;
using Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IProductsBuyerFavoritesService
    {
        //Common Methods
        Task<List<ProductsBuyerFavoritesVM>> Get();
        Task<ProductsBuyerFavoritesDTO> Get(int id);
        Task<ProductsBuyerFavoritesDTO> Create(ProductsBuyerFavoritesDTO entity);
        Task<ProductsBuyerFavoritesDTO> Update(ProductsBuyerFavoritesDTO entity);
        Task<int> Delete(int id);

        //Custome Method
        Task<bool> CheckDuplicate(ProductsBuyerFavoritesDTO entity);
        Task<List<BuyerFavouriteProductsCommonVM>> GetFavoutiteProductsbybuyerid(int buyerid);
    }
}
