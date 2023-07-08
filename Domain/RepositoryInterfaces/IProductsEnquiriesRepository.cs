using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductsEnquiriesRepository : IRepository<ProductsEnquiries>
    {
        Task<List<ProductsEnquiries>> GetProductsEnquiriesByPid(int ProductId);
    }
}
