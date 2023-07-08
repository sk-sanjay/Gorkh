namespace Application.ServiceInterfaces
{
    public interface IDataService
    {
        IAuthenticationTicketsService AuthenticationTickets { get; }
        ISiteAuthenticationTicketsService SiteAuthenticationTickets { get; }
        ICountriesService Countries { get; }
        IDashboardAlertsService DashboardAlerts { get; }
        IDistrictsService Districts { get; }
        IMenuService Menus { get; }
        INotificationDetailsService NotificationDetails { get; }
        INotificationsService Notifications { get; }
        IRefreshTokenService RefreshTokens { get; }
        IRoleMenuService RoleMenus { get; }
        IStatesService States { get; }
        IWebApiLogsService WebApiLogs { get; }
        IWebAppLogsService WebAppLogs { get; }
        ICategoriesService Categories { get; }
        ISubCategoriesService SubCategories { get; set; }
        ISubSubCategoriesService SubSubCategories { get; }
        ICitiesService Cities { get; }
        IManufacturersService Manufacturers { get; }
        IShippingWeightsService ShippingWeights { get; }
        ICurrentsService Currents { get; }
        IVoltageFrequenciesService VoltageFrequencies { get; }
        ISpecificationsService Specifications { get; }
        ISpecificationsSSCategoriesService SpecificationsSSCategories { get; }
        IOrganisationTypesService OrganisationTypes { get; }
        IBuyersService Buyers { get; }
        ISellerRegistrationsService SellerRegistrations { get; }
        IBuyerSellerRegistrationsService BuyerSellerRegistrations { get; }
       // IBuyerSellerRegistrations1Service BuyerSellerRegistrations1 { get; }
        IProductsService Products { get; }
        IProductsSpecificationsService ProductsSpecifications { get; }
        IConditionsService Conditions { get; }
        IProductsLocationsService ProductsLocations { get; }
        IProductsDescriptionsService ProductsDescriptions { get; }
        IProductsFileUploadsService ProductsFileUploads { get; }
        IProducts1Service Products1 { get; }
        IProductsEnquiriesService ProductsEnquiries { get; }
        IBannersService Banners { get; }
        IBuyerRequirementsService BuyerRequirements { get; }
        IProductsVisitorsService ProductsVisitors { get; }
        IProductsBuyerIntrestsService ProductsBuyerIntrests { get; }
        IProductsPurchasesService ProductsPurchases { get; }
        IProductsBuyerQueriesService ProductsBuyerQueries { get; }
        IProductsBuyerFavoritesService ProductsBuyerFavorites { get; }
        IPaymentsService Payments { get; }
        IOtherLinkService OtherLinkHeadings { get; }
        ITempOtherLinkHeadingService TempOtherLinkHeadings { get; }
        IBuyerOffersService BuyerOffers { get; }
        IOurCustomersService OurCustomers { get; }

        ITestimonialsService Testimonials { get; }
        IVisitorRegistrationsService VisitorRegistrations { get; }

    }
}
