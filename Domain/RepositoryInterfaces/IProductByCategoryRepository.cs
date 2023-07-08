using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IProductByCategoryRepository : IRepository<CategoryimgCommon>
    {
       // Task<List<CategoryimgCommon>> GetCategoryImage();
    }
}
