using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IBuyerRequirementsRepository : IRepository<BuyerRequirements>
    {
        Task<List<BuyerRequirements1>> GetBuyerRequirements();
        Task<List<BuyerRequirements1>> GetBuyerRequirementsforWebsite();
        Task<BuyerRequirements1> GetBuyerRequirements(int id);
        Task<List<BuyerRequirements1>> GetBuyerRequirementsbyusername(string email);
    }
}
