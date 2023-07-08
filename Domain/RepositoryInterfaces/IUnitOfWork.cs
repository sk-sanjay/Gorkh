using System;
using System.Threading.Tasks;
namespace Domain.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISpecificationsSSCategoriesRepository SpecificationsSSCategoriesRepo { get; }
        ISpecificationsRepository SpecificationsRepo { get; }
        IVoltageFrequenciesRepository VoltageFrequenciesRepo { get; }
        ICurrentsRepository CurrentsRepo { get; }
        IShippingWeightsRepository ShippingWeightsRepo { get; }
        IManufacturersRepository ManufacturersRepo { get; }
        IAuthenticationTicketsRepository AuthenticationTicketsRepo { get; }
        ISiteAuthenticationTicketsRepository SiteAuthenticationTicketsRepo { get; }
        ICountriesRepository CountriesRepo { get; }
        IDashboardAlertsRepository DashboardAlertsRepo { get; }
        IDistrictsRepository DistrictsRepo { get; }
        IMenuRepository MenuRepo { get; }
        INotificationDetailsRepository NotificationDetailsRepo { get; }
        INotificationsRepository NotificationsRepo { get; }
        IRefreshTokensRepository RefreshTokensRepo { get; }
        IRoleMenuRepository RoleMenuRepo { get; }
        IStatesRepository StatesRepo { get; }
        IWebApiLogsRepository WebApiLogsRepo { get; }
        IWebAppLogsRepository WebAppLogsRepo { get; }
        ICategoriesRepository CategoriesRepo { get; }
        ISubCategoriesRepository SubCategoriesRepo { get; }
        ISubSubCategoriesRepository SubSubCategoriesRepo { get; set; }
        ICitiesRepository CitiesRepo { get; set; }
        IOrganisationTypesRepository OrganisationTypesRepo { get; set; }
        IBuyersRepository BuyersRepo { get; set; }
        ISellerRegistrationsRepository SellerRegistrationsRepo { get; set; }
        IBuyerSellerRegistrationsRepository BuyerSellerRegistrationsRepo { get; set; }
        IBuyerSellerRegistrations1Repository BuyerSellerRegistrations1Repo { get; set; }
        IBuyerRequirementsRepository BuyerRequirementsRepo { get; set; }
        //IBuyerSellerRegistrationsRepository BuyerCommonRepo { get; set; }
        IProductsRepository ProductsRepo { get; }
        IProductsSpecificationsRepository ProductsSpecificationsRepo { get; }
        IConditionsRepository ConditionsRepo { get; }
        IProductsLocationsRepository ProductsLocationsRepo { get; }
        IProductsDescriptionsRepository ProductsDescriptionsRepo { get; }
        IProductsFileUploadsRepository ProductsFileUploadsRepo { get; }
        IProducts1Repository Products1Repo { get; }
        IProductByCategoryRepository ProductByCategoryRepositoryRepo { get; }
        IProductsEnquiriesRepository ProductsEnquiriesRepo { get; }
        IBannersRepository BannersRepo { get; }
        IProductsVisitorsRepository ProductsVisitorsRepo { get; }
        IProductsBuyerIntrestsRepository ProductsBuyerIntrestsRepo { get; }
        IProductsBuyerFavoritesRepository ProductsBuyerFavoritesRepo { get; }
        IProductsPurchasesRepository ProductsPurchasesRepo { get; }
        IProductsBuyerQueriesRepository ProductsBuyerQueriesRepo { get; }
        IPaymentsRepository PaymentsRepo { get; }
        IOtherLinkRepository OtherLinkRepo { get; set; }
        ITempOtherLinkRepository TempOtherLinkRepo { get; set; }
        IBuyerandSellerCountRepository BuyerandSellerCountRepo { get; set; }
        IProductCountRepository ProductCountRepo { get; set; }
        IBuyerOffersRepository BuyerOffersRepo { get; set; }
        IOurCustomersRepository OurCustomersRepo { get; set; }
        ITestimonialsRepository TestimonialsRepo { get; set; }
        IVisitorRegistrationsRepository VisitorRegistrationsRepo { get; set; }

        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
