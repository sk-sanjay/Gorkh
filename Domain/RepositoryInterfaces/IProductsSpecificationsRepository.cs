using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsSpecificationsRepository : IRepository<ProductsSpecifications>
    {
        Task <List<ProductsSpecifications>> GetByProductId(int id);
    }
}
