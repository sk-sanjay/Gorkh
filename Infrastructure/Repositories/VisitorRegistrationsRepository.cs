using Application.ViewModels;
using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public class VisitorRegistrationsRepository : Repository<VisitorRegistrations>, IVisitorRegistrationsRepository
    {
        private AppDbContext DbContext => _dbContext as AppDbContext;
        public VisitorRegistrationsRepository(AppDbContext context) : base(context)
        {

        }
        public Task<VisitorRegistrations> CheckDuplicate(VisitorRegistrations model)
        {
            var duplicateModel = DbContext.VisitorRegistrations.FirstOrDefaultAsync(x =>
                    x.Email == model.Email);
            return duplicateModel;

        }
        public Task<VisitorRegistrations> GetbyEmailId(string email)
        {
            return DbContext.VisitorRegistrations.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
