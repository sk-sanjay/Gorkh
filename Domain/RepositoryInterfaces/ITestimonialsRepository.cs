using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
  public interface ITestimonialsRepository : IRepository<Testimonials>
    {
        Task<Testimonials> CheckDuplicate(Testimonials model);
        Task<List<Testimonials>> GetTestimonials();
    }
}
