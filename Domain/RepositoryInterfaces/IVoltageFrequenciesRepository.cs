using Domain.Models;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IVoltageFrequenciesRepository : IRepository<VoltageFrequencies>
    {
        Task<VoltageFrequencies> CheckDuplicate(VoltageFrequencies model);
    }
}
