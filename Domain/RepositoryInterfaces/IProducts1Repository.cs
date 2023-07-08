using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProducts1Repository : IRepository<Products1>
    {
        Task<List<Products1>> GettProductsBySubCatId1(int catid, int subcategoryid);
        Task<List<Products1>> GetProductsasfeaturedmachine();
    }
}
