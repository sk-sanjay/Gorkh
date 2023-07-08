using Domain.Models;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
 public interface IVisitorRegistrationsRepository : IRepository<VisitorRegistrations>
    {
        Task<VisitorRegistrations> CheckDuplicate(VisitorRegistrations model);
        Task<VisitorRegistrations> GetbyEmailId(string email);
    }
}
