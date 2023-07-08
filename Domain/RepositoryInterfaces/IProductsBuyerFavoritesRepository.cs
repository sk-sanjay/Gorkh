using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsBuyerFavoritesRepository : IRepository<ProductsBuyerFavorites>
    {
        Task<ProductsBuyerFavorites> CheckDuplicate(ProductsBuyerFavorites model);
       Task<List<BuyerFavouriteProductsCommon>> GetFavoutiteProductsbybuyerid(int buyerid);


    }
}
