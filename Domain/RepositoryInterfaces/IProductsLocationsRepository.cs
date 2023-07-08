using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsLocationsRepository : IRepository<ProductsLocations>
    {
        Task<ProductsLocations> GetByProductId(int id);
    }
}
