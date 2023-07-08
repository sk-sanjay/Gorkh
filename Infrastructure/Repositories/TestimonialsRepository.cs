using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
   public class TestimonialsRepository : Repository<Testimonials>, ITestimonialsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public TestimonialsRepository(AppDbContext context) : base(context)
        {
        }
        public Task<Testimonials> CheckDuplicate(Testimonials model)
        {
            if (model.Id == 0)
            {
                var duplicateModel = DbContext.Testimonials.FirstOrDefaultAsync(x =>
                    x.Name == model.Name);
                return duplicateModel;
            }
            else
            {
                var duplicateModel = DbContext.Testimonials.FirstOrDefaultAsync(x =>
                    x.Name == model.Name && x.Id != model.Id);
                return duplicateModel;
            }
        }


        public async Task<List<Testimonials>> GetTestimonials()
        {
            return await(from a in DbContext.Testimonials where a.IsActive == true select a).ToListAsync();
        }
    }
}
