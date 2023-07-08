using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    class SpecificationsSSCategoriesRepository : Repository<SpecificationsSSCategories>, ISpecificationsSSCategoriesRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public SpecificationsSSCategoriesRepository(AppDbContext context) : base(context)
        {
        }
        public Task<List<SpecificationsSSCategories>> GetSpecificationsSSCategories(int subsubcategoryid)
        {
            return DbContext.SpecificationsSSCategories
                .Where(x => x.SubSubCatId == subsubcategoryid && x.IsActive).ToListAsync();
        }

        public async Task<List<SpecificationsSSCategories>> GetSpecificationsSSCategoriesjoin(int subsubcategoryid)
        {
            return await (from a in DbContext.SpecificationsSSCategories
                          join b in DbContext.Specifications
                          on a.SpecfId equals b.Id
                          where a.SubSubCatId == subsubcategoryid && a.IsActive == true
                          orderby a.Sequence
                          select new SpecificationsSSCategories
                          {
                              SpecfName = b.SpecfName,
                              TextType = b.TextType,
                              SpecfId = a.SpecfId,
                              Sequence = a.Sequence,
                              IsMandatory = a.IsMandatory

                          }).ToListAsync();
        }

    }
}
