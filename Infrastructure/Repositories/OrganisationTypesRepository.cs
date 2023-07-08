using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class OrganisationTypesRepository : Repository<OrganisationTypes>, IOrganisationTypesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public OrganisationTypesRepository(AppDbContext context) : base(context)
        {

        }
    }
}
