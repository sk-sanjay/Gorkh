using Application.ServiceInterfaces;
using AutoMapper;
using Domain.RepositoryInterfaces;
namespace Application.Services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IVoltageFrequenciesService VoltageFrequencies { get; }
        public ICountriesService Countries { get; }
        public IDashboardAlertsService DashboardAlerts { get; }
        public IDistrictsService Districts { get; }
        public IMenuService Menus { get; }
        public INotificationsService Notifications { get; }
        public INotificationDetailsService NotificationDetails { get; }
        public IRefreshTokenService RefreshTokens { get; }
        public IRoleMenuService RoleMenus { get; }
        public IStatesService States { get; }
        public IWebApiLogsService WebApiLogs { get; }
        public IWebAppLogsService WebAppLogs { get; }
        public IAuthenticationTicketsService AuthenticationTickets { get; }
        public ISiteAuthenticationTicketsService SiteAuthenticationTickets { get; }
        public ICategoriesService Categories { get; }
        public ISubCategoriesService SubCategories { get; set; }
        public ISubSubCategoriesService SubSubCategories { get; }
        public ICitiesService Cities { get; }
        public IManufacturersService Manufacturers { get; }
        public IShippingWeightsService ShippingWeights { get; }
        public ICurrentsService Currents { get; }
        public ISpecificationsService Specifications { get; }
        public ISpecificationsSSCategoriesService SpecificationsSSCategories { get; }
        public IOrganisationTypesService OrganisationTypes { get; }
        public IBuyersService Buyers { get; }
        public ISellerRegistrationsService SellerRegistrations { get; }
        public IBuyerRequirementsService BuyerRequirements { get; }
        public IBuyerSellerRegistrationsService BuyerSellerRegistrations { get; }
        public IProductsService Products { get; }
        public IProductsSpecificationsService ProductsSpecifications { get; }
        public IConditionsService Conditions { get; }
        public IProductsLocationsService ProductsLocations { get; }
        public IProductsDescriptionsService ProductsDescriptions { get; }
        public IProductsFileUploadsService ProductsFileUploads { get; }
        public IProducts1Service Products1 { get; }
        public IProductsEnquiriesService ProductsEnquiries { get; }
        public IBannersService Banners { get; }
        public IProductsVisitorsService ProductsVisitors { get; }
        public IProductsBuyerIntrestsService ProductsBuyerIntrests { get; }
        public IProductsPurchasesService ProductsPurchases { get; }
        public IProductsBuyerQueriesService ProductsBuyerQueries { get; }
        public IProductsBuyerFavoritesService ProductsBuyerFavorites { get; }
        public IPaymentsService Payments { get; }
        // for OtherLink
        public IOtherLinkService OtherLinkHeadings { get; }
        public ITempOtherLinkHeadingService TempOtherLinkHeadings { get; }
        public IBuyerOffersService BuyerOffers { get; }
        public IOurCustomersService OurCustomers { get; }
        public ITestimonialsService Testimonials { get; }
        public IVisitorRegistrationsService VisitorRegistrations { get; }

        public DataService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            VisitorRegistrations = new VisitorRegistrationsService(_unitOfWork, _mapper);
            Testimonials = new TestimonialsService(_unitOfWork, _mapper);
            OurCustomers = new OurCustomersService(_unitOfWork, _mapper);
            OtherLinkHeadings = new OtherLinkService(_unitOfWork, _mapper);
            TempOtherLinkHeadings = new TempOtherLinkService(_unitOfWork, _mapper);
            BuyerOffers = new BuyerOffersService(_unitOfWork, _mapper);

            Payments = new PaymentsService(_unitOfWork, _mapper);
            ProductsBuyerQueries = new ProductsBuyerQueriesService(_unitOfWork, _mapper);
            ProductsPurchases = new ProductsPurchasesService(_unitOfWork, _mapper);
            ProductsBuyerFavorites = new ProductsBuyerFavoritesService(_unitOfWork, _mapper);
            ProductsBuyerIntrests = new ProductsBuyerIntrestsService(_unitOfWork, _mapper);
            ProductsVisitors = new ProductsVisitorsService(_unitOfWork, _mapper);
            BuyerRequirements = new BuyerRequirementsService(_unitOfWork, _mapper);
            Banners = new BannersService(_unitOfWork, _mapper);
            ProductsEnquiries = new ProductsEnquiriesService(_unitOfWork, _mapper);
            Products1 = new Products1Service(_unitOfWork, _mapper);
            ProductsFileUploads = new ProductsFileUploadsService(_unitOfWork, _mapper);
            ProductsDescriptions = new ProductsDescriptionsService(_unitOfWork, _mapper);
            ProductsLocations = new ProductsLocationsService(_unitOfWork, _mapper);
            Conditions = new ConditionsService(_unitOfWork, _mapper);
            ProductsSpecifications = new ProductsSpecificationsService(_unitOfWork, _mapper);
            Products = new ProductsService(_unitOfWork, _mapper);
            SellerRegistrations = new SellerRegistrationsService(_unitOfWork, _mapper);
            BuyerSellerRegistrations = new BuyerSellerRegistrationsService(_unitOfWork, _mapper);
            Buyers = new BuyersService(_unitOfWork, _mapper);
            OrganisationTypes = new OrganisationTypesService(_unitOfWork, _mapper);
            SpecificationsSSCategories = new SpecificationsSSCategoriesService(_unitOfWork, _mapper);
            Specifications = new SpecificationsService(_unitOfWork, _mapper);
            VoltageFrequencies = new VoltageFrequenciesService(_unitOfWork, _mapper);
            Currents = new CurrentsService(_unitOfWork, _mapper);
            ShippingWeights = new ShippingWeightsService(_unitOfWork, _mapper);
            Manufacturers = new ManufacturersService(_unitOfWork, _mapper);
            Countries = new CountriesService(_unitOfWork, _mapper);
            DashboardAlerts = new DashboardAlertsService(_unitOfWork, _mapper);
            Districts = new DistrictsService(_unitOfWork, _mapper);
            Menus = new MenuService(_unitOfWork, _mapper);
            Notifications = new NotificationsService(_unitOfWork, _mapper);
            NotificationDetails = new NotificationDetailsService(_unitOfWork, _mapper);
            RefreshTokens = new RefreshTokenService(_unitOfWork, _mapper);
            RoleMenus = new RoleMenuService(_unitOfWork, _mapper);
            States = new StatesService(_unitOfWork, _mapper);
            WebApiLogs = new WebApiLogsService(_unitOfWork, _mapper);
            WebAppLogs = new WebAppLogsService(_unitOfWork, _mapper);
            AuthenticationTickets = new AuthenticationTicketsService(_unitOfWork, _mapper);
            SiteAuthenticationTickets = new SiteAuthenticationTicketsService(_unitOfWork, _mapper);
            Categories = new CategoriesService(_unitOfWork, _mapper);
            SubCategories = new SubCategoriesService(_unitOfWork, _mapper);
            SubSubCategories = new SubSubCategoriesService(_unitOfWork, _mapper);
            Cities = new CitiesService(_unitOfWork, _mapper);
        }
    }
}