using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ConditionsRepository : Repository<Conditions>, IConditionsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public ConditionsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
