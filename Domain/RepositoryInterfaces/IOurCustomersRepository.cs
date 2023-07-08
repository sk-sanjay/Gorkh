using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
  public interface IOurCustomersRepository : IRepository<OurCustomers>
    {
        Task<List<OurCustomers>> GetOurCustomersHomePage();
    }
}
