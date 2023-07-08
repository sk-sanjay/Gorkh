using Domain.Models;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IShippingWeightsRepository : IRepository<ShippingWeights>
    {
        Task<ShippingWeights> CheckDuplicate(ShippingWeights model);
    }
}
