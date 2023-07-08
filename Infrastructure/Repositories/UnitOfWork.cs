using Domain.Models;
using Domain.RepositoryInterfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDashboardAlertsRepository DashboardAlertsRepo { get; }
        public ICountriesRepository CountriesRepo { get; }
        public IDistrictsRepository DistrictsRepo { get; }
        public IMenuRepository MenuRepo { get; }
        public INotificationsRepository NotificationsRepo { get; }
        public INotificationDetailsRepository NotificationDetailsRepo { get; }
        public IRefreshTokensRepository RefreshTokensRepo { get; }
        public IRoleMenuRepository RoleMenuRepo { get; }
        public IStatesRepository StatesRepo { get; }
        public IWebApiLogsRepository WebApiLogsRepo { get; }
        public IWebAppLogsRepository WebAppLogsRepo { get; }
        public IAuthenticationTicketsRepository AuthenticationTicketsRepo { get; }
        public ISiteAuthenticationTicketsRepository SiteAuthenticationTicketsRepo { get; }
        public IManufacturersRepository ManufacturersRepo { get; }
        public IShippingWeightsRepository ShippingWeightsRepo { get; }
        public ICurrentsRepository CurrentsRepo { get; }
        public IVoltageFrequenciesRepository VoltageFrequenciesRepo { get; }
        public ICategoriesRepository CategoriesRepo { get; set; }
        public ISubCategoriesRepository SubCategoriesRepo { get; set; }
        public ISubSubCategoriesRepository SubSubCategoriesRepo { get; set; }
        public ICitiesRepository CitiesRepo { get; set; }
        public ISpecificationsRepository SpecificationsRepo { get; }
        public ISpecificationsSSCategoriesRepository SpecificationsSSCategoriesRepo { get; }
        public IOrganisationTypesRepository OrganisationTypesRepo { get; set; }
        public IBuyersRepository BuyersRepo { get; set; }
        public ISellerRegistrationsRepository SellerRegistrationsRepo { get; set; }
        public IBuyerSellerRegistrationsRepository BuyerSellerRegistrationsRepo { get; set; }
        public IBuyerSellerRegistrations1Repository BuyerSellerRegistrations1Repo { get; set; }
        public IProductsRepository ProductsRepo { get; }
        public IProductsSpecificationsRepository ProductsSpecificationsRepo { get; }
        public IConditionsRepository ConditionsRepo { get; }
        public IProductsLocationsRepository ProductsLocationsRepo { get; }
        public IProductsDescriptionsRepository ProductsDescriptionsRepo { get; }
        public IBuyerRequirementsRepository BuyerRequirementsRepo { get; set; }
        public IProductsFileUploadsRepository ProductsFileUploadsRepo { get; }
        public IProducts1Repository Products1Repo { get; }
        public IProductsEnquiriesRepository ProductsEnquiriesRepo { get; }
        public IBannersRepository BannersRepo { get; }
        public IProductsVisitorsRepository ProductsVisitorsRepo { get; }
        public IProductsBuyerIntrestsRepository ProductsBuyerIntrestsRepo { get; }
        public IProductsBuyerFavoritesRepository ProductsBuyerFavoritesRepo { get; }
        public IProductsPurchasesRepository ProductsPurchasesRepo { get; }
        public IProductsBuyerQueriesRepository ProductsBuyerQueriesRepo { get; }
        public IPaymentsRepository PaymentsRepo { get; }
       public IProductByCategoryRepository ProductByCategoryRepositoryRepo { get; }
        public IOtherLinkRepository OtherLinkRepo { get; set; }
        public ITempOtherLinkRepository TempOtherLinkRepo { get; set; }
        public IBuyerandSellerCountRepository BuyerandSellerCountRepo { get; set; }
        public IProductCountRepository ProductCountRepo { get; set; }
        public IBuyerOffersRepository BuyerOffersRepo { get; set; }
        public IOurCustomersRepository OurCustomersRepo { get; set; }
        public ITestimonialsRepository TestimonialsRepo { get; set; }
        public IVisitorRegistrationsRepository VisitorRegistrationsRepo { get; set; }


        private readonly AppDbContext _dbContext;
        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            VisitorRegistrationsRepo = new VisitorRegistrationsRepository(_dbContext);
            TestimonialsRepo = new TestimonialsRepository(_dbContext);
            OurCustomersRepo = new OurCustomersRepository(_dbContext);
            BuyerOffersRepo = new BuyerOffersRepository(_dbContext);
            ProductCountRepo = new ProductCountRepository(_dbContext);
            BuyerandSellerCountRepo = new BuyerandSellerCountRepository(_dbContext);
            OtherLinkRepo = new OtherLinkRepository(_dbContext);
            TempOtherLinkRepo = new TempOtherLinkRepository(_dbContext);

            PaymentsRepo = new PaymentsRepository(_dbContext);
            ProductsBuyerQueriesRepo =new ProductsBuyerQueriesRepository(_dbContext);
            ProductByCategoryRepositoryRepo = new ProductByCategoryRepository(_dbContext);
            
            ProductsPurchasesRepo = new ProductsPurchasesRepository(_dbContext);
            ProductsBuyerFavoritesRepo = new ProductsBuyerFavoritesRepository(_dbContext);
            ProductsBuyerIntrestsRepo = new ProductsBuyerIntrestsRepository(_dbContext);
            ProductsVisitorsRepo = new ProductsVisitorsRepository(_dbContext);
            BuyerRequirementsRepo = new BuyerRequirementsRepository(_dbContext);
            BannersRepo = new BannersRepository(_dbContext);
            ProductsEnquiriesRepo = new ProductsEnquiriesRepository(_dbContext);
            Products1Repo = new Products1Repository(_dbContext);
            ProductsFileUploadsRepo = new ProductsFileUploadsRepository(_dbContext);
            ProductsDescriptionsRepo = new ProductsDescriptionsRepository(_dbContext);
            ProductsLocationsRepo = new ProductsLocationsRepository(_dbContext);
            SellerRegistrationsRepo = new SellerRegistrationsRepository(_dbContext);
            BuyerSellerRegistrationsRepo = new BuyerSellerRegistrationsRepository(_dbContext);
            BuyerSellerRegistrations1Repo = new BuyerSellerRegistrations1Repository(_dbContext);
            BuyersRepo = new BuyersRepository(_dbContext);
            OrganisationTypesRepo = new OrganisationTypesRepository(_dbContext);
            SpecificationsSSCategoriesRepo = new SpecificationsSSCategoriesRepository(_dbContext);
            SubSubCategoriesRepo = new SubSubCategoriesRepository(_dbContext);
            CitiesRepo = new CitiesRepository(_dbContext);
            SubCategoriesRepo = new SubCategoriesRepository(_dbContext);
            CategoriesRepo = new CategoriesRepository(_dbContext);
            DashboardAlertsRepo = new DashboardAlertsRepository(_dbContext);
            CountriesRepo = new CountriesRepository(_dbContext);
            DistrictsRepo = new DistrictsRepository(_dbContext);
            MenuRepo = new MenuRepository(_dbContext);
            NotificationsRepo = new NotificationsRepository(_dbContext);
            NotificationDetailsRepo = new NotificationDetailsRepository(_dbContext);
            RefreshTokensRepo = new RefreshTokensRepository(_dbContext);
            RoleMenuRepo = new RoleMenuRepository(_dbContext);
            StatesRepo = new StatesRepository(_dbContext);
            WebApiLogsRepo = new WebApiLogsRepository(_dbContext);
            WebAppLogsRepo = new WebAppLogsRepository(_dbContext);
            AuthenticationTicketsRepo = new AuthenticationTicketsRepository(_dbContext);
            SiteAuthenticationTicketsRepo = new SiteAuthenticationTicketsRepository(_dbContext);
            ManufacturersRepo = new ManufacturersRepository(_dbContext);
            ShippingWeightsRepo = new shippingWeightsRepository(_dbContext);
            CurrentsRepo = new CurrentsRepository(_dbContext);
            VoltageFrequenciesRepo = new VoltageFrequenciesRepository(_dbContext);
            SpecificationsRepo = new SpecificationsRepository(_dbContext);
            ProductsRepo = new ProductsRepository(_dbContext);
            ProductsSpecificationsRepo = new ProductsSpecificationsRepository(_dbContext);
            ConditionsRepo = new ConditionsRepository(_dbContext);
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            //OnBeforeSaveChanges();
            return _dbContext.SaveChangesAsync();
        }
        private void OnBeforeSaveChanges()
        {
            _dbContext.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                if (entry.Entity is Audits || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entry.Entity.GetType().Name == "RefreshTokens" || entry.Entity.GetType().Name == "AuthenticationTickets")
                    continue;
                var auditEntry = new AuditEntry(entry) { TableName = entry.Entity.GetType().Name };
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = Application.Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            if (property.Metadata.Name == "CreatedBy")
                                auditEntry.UserName = property.CurrentValue.ToString();
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = Application.Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            if (property.Metadata.Name == "ModifiedBy")
                                auditEntry.UserName = property.CurrentValue.ToString();
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = Application.Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                                if (property.Metadata.Name == "ModifiedBy")
                                    auditEntry.UserName = property.CurrentValue.ToString();
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                _dbContext.AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}
