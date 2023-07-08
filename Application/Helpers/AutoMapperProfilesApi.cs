using Application.Dtos;
using Application.ViewModels;
using AutoMapper;
using Domain.Models;

namespace Application.Helpers
{
    public class AutoMapperProfilesApi : Profile
    {
        public AutoMapperProfilesApi()
        {
            //ApplicationUser, UserProfileDTO
            CreateMap<ApplicationUser, UserProfileDTO>();
            CreateMap<UserProfileDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserVM>()
                .ForMember(dest => dest.PlainPass, opt => { opt.MapFrom(src => EnDeCryptor.DecryptStringAES(src.EncSecret)); });
            CreateMap<ApplicationUser, RegisterDTO>();
            CreateMap<ApplicationUser, DropdownStrVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.UserName); });
            //AuthenticationTickets
            CreateMap<AuthenticationTickets, AuthenticationTicketsVM>();
            CreateMap<AuthenticationTicketsDTO, AuthenticationTickets>();
            CreateMap<AuthenticationTickets, AuthenticationTicketsDTO>();

            //SiteAuthenticationTickets
            CreateMap<SiteAuthenticationTickets, SiteAuthenticationTicketsVM>();
            CreateMap<SiteAuthenticationTicketsDTO, SiteAuthenticationTickets>();
            CreateMap<SiteAuthenticationTickets, SiteAuthenticationTicketsDTO>();

            //Menus
            CreateMap<Menus, MenuVM>();
            CreateMap<MenuDTO, Menus>();
            CreateMap<Menus, MenuDTO>();
            //Notifications
            CreateMap<Notifications, NotificationsVM>();
            CreateMap<NotificationsDTO, Notifications>();
            CreateMap<Notifications, NotificationsDTO>();
            //Notifications
            CreateMap<NotificationDetails, NotificationDetailsVM>();
            CreateMap<NotificationDetailsDTO, NotificationDetails>();
            CreateMap<NotificationDetails, NotificationDetailsDTO>();
            //Countries
            CreateMap<Countries, CountriesVM>();
            CreateMap<CountriesDTO, Countries>();
            CreateMap<Countries, CountriesDTO>();
            CreateMap<Countries, DropdownVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.Name); });
            //States
            CreateMap<States, StatesVM>()
                .ForMember(x => x.CountryName, opt => { opt.MapFrom(src => src.Country.Name); });
            CreateMap<StatesDTO, States>();
            CreateMap<States, StatesDTO>();
            CreateMap<States, DropdownVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.StateName); });
            //Districts
            CreateMap<Districts, DistrictsVM>()
                .ForMember(dest => dest.StateName, opt => { opt.MapFrom(src => src.State.StateName); });
            CreateMap<DistrictsDTO, Districts>();
            CreateMap<Districts, DistrictsDTO>();
            CreateMap<Districts, DropdownVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.DistrictName); });
            //RoleMenus
            CreateMap<RoleMenuDTO, RoleMenus>();
            CreateMap<RoleMenus, RoleMenuDTO>();
            //Roles
            CreateMap<ApplicationRole, RoleDTO>();
            CreateMap<RoleDTO, ApplicationRole>();
            //DashboardAlerts
            CreateMap<DashboardAlerts, DashboardAlertsVM>();
            CreateMap<DashboardAlertsDTO, DashboardAlerts>();
            CreateMap<DashboardAlerts, DashboardAlertsDTO>();
            //Dropdown
            CreateMap<Dropdown, DropdownVM>();
            //WebApiLogs
            CreateMap<WebApiLogs, WebApiLogsVM>();
            CreateMap<WebApiLogsDTO, WebApiLogs>();
            CreateMap<WebApiLogs, WebApiLogsDTO>();
            //WebAppLogs
            CreateMap<WebAppLogs, WebAppLogsVM>();
            CreateMap<WebAppLogsDTO, WebAppLogs>();
            CreateMap<WebAppLogs, WebAppLogsDTO>();
            //Manufacturers
            CreateMap<Manufacturers, ManufacturersVM>();
            CreateMap<ManufacturersDTO, Manufacturers>();
            CreateMap<Manufacturers, ManufacturersDTO>();
            CreateMap<Manufacturers, DropdownVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.MfrName); });
            //ShippingWeights
            CreateMap<ShippingWeights, ShippingWeightsVM>();
            CreateMap<ShippingWeightsDTO, ShippingWeights>();
            CreateMap<ShippingWeights, ShippingWeightsDTO>();
            //Currents
            CreateMap<Currents, CurrentsVM>();
            CreateMap<CurrentsDTO, Currents>();
            CreateMap<Currents, CurrentsDTO>();
            //VoltageFrequencies
            CreateMap<VoltageFrequencies, VoltageFrequenciesVM>();
            CreateMap<VoltageFrequenciesDTO, VoltageFrequencies>();
            CreateMap<VoltageFrequencies, VoltageFrequenciesDTO>();

            //Category
            CreateMap<Categories, CategoriesVM>();
            CreateMap<CategoriesDTO, Categories>();
            CreateMap<Categories, CategoriesDTO>();
            CreateMap<Categories, DropdownVM>()
           .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.Name); });

            //OrganisationTypes
            CreateMap<OrganisationTypes, OrganisationTypesVM>();
            CreateMap<OrganisationTypesDTO, OrganisationTypes>();
            CreateMap<OrganisationTypes, OrganisationTypesDTO>();
            CreateMap<OrganisationTypes, DropdownVM>()
           .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.Name); });

            //SubCategory
            CreateMap<SubCategories, SubCategoriesVM>();
            CreateMap<SubCategoriesDTO, SubCategories>();
            CreateMap<SubCategories, SubCategoriesDTO>();

            //Sub-SubCategory
            CreateMap<SubSubCategories, SubSubCategoriesVM>();
            CreateMap<SubSubCategoriesDTO, SubSubCategories>();
            CreateMap<SubSubCategories, SubSubCategoriesDTO>();

            //City
            CreateMap<Cities, CitiesVM>();
            CreateMap<CitiesDTO, Cities>();
            CreateMap<Cities, CitiesDTO>();

            //Specifications
            CreateMap<Specifications, SpecificationsVM>();
            CreateMap<SpecificationsDTO, Specifications>();
            CreateMap<Specifications, SpecificationsDTO>();

            //Specifications
            CreateMap<SpecificationsSSCategories, SpecificationsSSCategoriesVM>();
            CreateMap<SpecificationsSSCategoriesDTO, SpecificationsSSCategories>();
            CreateMap<SpecificationsSSCategories, SpecificationsSSCategoriesDTO>();

            //Buyers 
            CreateMap<Buyers, BuyersVM>();
            CreateMap<BuyersDTO, Buyers>();
            CreateMap<Buyers, BuyersDTO>();

            //Seller Registrations 
            CreateMap<SellerRegistrations, SellerRegistrationsVM>();
            CreateMap<SellerRegistrationsDTO, SellerRegistrations>();
            CreateMap<SellerRegistrations, SellerRegistrationsDTO>();

            //Seller and Buyer Registrations 
            CreateMap<BuyerSellerRegistrations, BuyerSellerRegistrationsVM>();
            CreateMap<BuyerSellerRegistrationsDTO, BuyerSellerRegistrations>();
            CreateMap<BuyerSellerRegistrations, BuyerSellerRegistrationsDTO>();

            //Seller and Buyer Registrations common  
            CreateMap<BuyerCommon, BuyerCommonVM>();
            CreateMap<BuyerCommonDTO, BuyerSellerRegistrations>();
            CreateMap<BuyerCommon, BuyerCommonDTO>();

            //Products
            CreateMap<Products, ProductsVM>();
            CreateMap<ProductsDTO, Products>();
            CreateMap<Products, ProductsDTO>();
            CreateMap<Products, ProductsViewModel>();
            CreateMap<ProductsVM, ProductsViewModel>();

            //ProductsSpecifications
            CreateMap<ProductsSpecifications, ProductsSpecificationsVM>();
            CreateMap<ProductsSpecificationsDTO, ProductsSpecifications>();
            CreateMap<ProductsSpecifications, ProductsSpecificationsDTO>();

            //Conditions
            CreateMap<Conditions, ConditionsVM>();
            CreateMap<ConditionsDTO, Conditions>();
            CreateMap<Conditions, ConditionsDTO>();
            CreateMap<Conditions, DropdownVM>()
                .ForMember(dest => dest.Text, opt => { opt.MapFrom(src => src.ConditionName); });

            //Products Locations
            CreateMap<ProductsLocations, ProductsLocationsVM>();
            CreateMap<ProductsLocationsDTO, ProductsLocations>();
            CreateMap<ProductsLocations, ProductsLocationsDTO>();

            //Products Descriptions
            CreateMap<ProductsDescriptions, ProductsDescriptionsVM>();
            CreateMap<ProductsDescriptionsDTO, ProductsDescriptions>();
            CreateMap<ProductsDescriptions, ProductsDescriptionsDTO>();

            //Products File Uploads
            CreateMap<ProductsFileUploads, ProductsFileUploadsVM>();
            CreateMap<ProductsFileUploadsDTO, ProductsFileUploads>();
            CreateMap<ProductsFileUploads, ProductsFileUploadsDTO>();

            //Products For Stored Procedure
            CreateMap<Products1, Products1VM>();

            //Buyer Seller Data Stored Procedure
            CreateMap<BuyerSellerRegistrations1, BuyerSellerRegistrations1VM>();

            CreateMap<ProductsDetails, ProductsDetailsVM>();
            CreateMap<ProductsSpecificationsGets, ProductsSpecificationsGetsVM>();

            CreateMap<ProductsSellerDetails, ProductsSellerDetailsVM>();

            //Products Enquiry 
            CreateMap<ProductsEnquiries, ProductsEnquiriesVM>();
            CreateMap<ProductsEnquiriesDTO, ProductsEnquiries>();
            CreateMap<ProductsEnquiries, ProductsEnquiriesDTO>();

            //Banners
            CreateMap<Banners, BannersVM>();
            CreateMap<BannersDTO, Banners>();
            CreateMap<Banners, BannersDTO>();

            //BuyerRequirements
            CreateMap<BuyerRequirements, BuyerRequirementsVM>();
            CreateMap<BuyerRequirementsDTO, BuyerRequirements>();
            CreateMap<BuyerRequirements, BuyerRequirementsDTO>();

            //BuyerRequirements
            CreateMap<BuyerRequirements1, BuyerRequirementsVM1>();
            

            //Products Visitors
            CreateMap<ProductsVisitors, ProductsVisitorsVM>();
            CreateMap<ProductsVisitorsDTO, ProductsVisitors>();
            CreateMap<ProductsVisitors, ProductsVisitorsDTO>();

            CreateMap<ProductsBySeller, ProductsBySellerVM>();

            //Products Buyer Intrests
            CreateMap<ProductsBuyerIntrests, ProductsBuyerIntrestsVM>();
            CreateMap<ProductsBuyerIntrestsDTO, ProductsBuyerIntrests>();
            CreateMap<ProductsBuyerIntrests, ProductsBuyerIntrestsDTO>();

            CreateMap<ProductsBuyerIntrestsCommon, ProductsBuyerIntrestsCommonVM>();

            //Products Purchases
            CreateMap<ProductsPurchases, ProductsPurchasesVM>();
            CreateMap<ProductsPurchasesDTO, ProductsPurchases>();
            CreateMap<ProductsPurchases, ProductsPurchasesDTO>();

            CreateMap<ProductsPurchasesCommon, ProductsPurchasesCommonVM>();

            //Products Buyer Queries
            CreateMap<ProductsBuyerQueries, ProductsBuyerQueriesVM>();
            CreateMap<ProductsBuyerQueriesDTO, ProductsBuyerQueries>();
            CreateMap<ProductsBuyerQueries, ProductsBuyerQueriesDTO>();

            CreateMap<ProductsBuyerQueriesCommon, ProductsBuyerQueriesCommonVM>();

            //Products BuyerFavorites
            CreateMap<ProductsBuyerFavorites, ProductsBuyerFavoritesVM>();
            CreateMap<ProductsBuyerFavoritesDTO, ProductsBuyerFavorites>();
            CreateMap<ProductsBuyerFavorites, ProductsBuyerFavoritesDTO>();
            CreateMap<BuyerFavouriteProductsCommon, BuyerFavouriteProductsCommonVM>();

            //
            CreateMap<CategoryimgCommon, CategoryimgCommonVM>();

            // OtherLink
            CreateMap<OtherLinkHeading, OtherLinkDTO>();
            CreateMap<OtherLinkDTO, OtherLinkHeading>();
            CreateMap<OtherLinkHeading, OtherLinkVM>();
            // CreateMap<OtherLinkListPriorityDTO, OtherLinkListPriority>();

            // TempOtherLink
            CreateMap<TempOtherLinkHeading, TempOtherLinkHeadingDTO>();
            CreateMap<TempOtherLinkHeadingDTO, TempOtherLinkHeading>();
            CreateMap<TempOtherLinkHeading, TempOtherLinkVM>();

            // Buyer Seller Count
             CreateMap<BuyerSellerCount, BuyerSellerCountVM>();

            // Product Count
            CreateMap<ProductCount, ProductCountVM>();

            //ProductsBreadCrumbs
            CreateMap<ProductsBreadCrumbs, ProductsBreadCrumbsVM>();

            //Paments
            CreateMap<Payments, PaymentsVM>();
            CreateMap<PaymentsDTO, Payments>();
            CreateMap<Payments, PaymentsDTO>();

            CreateMap<PaymentsCommon, PaymentsCommonVM>();

            CreateMap<BuyerOffersCommon, BuyerOffersCommonVM>();

            //commonupdate
            CreateMap<CommonDTO, Common>();
            CreateMap<Common, CommonDTO>();
            // Related product
            CreateMap<RelatedProducts, RelatedProductsVM>();

            // Buyer Offer
            CreateMap<BuyerOffers, BuyerOffersVM>();
            CreateMap<BuyerOffersDTO, BuyerOffers>();
            CreateMap<BuyerOffers, BuyerOffersDTO>();

            //OurCustomers
            CreateMap<OurCustomers, OurCustomersVM>();
            CreateMap<OurCustomersDTO, OurCustomers>();
            CreateMap<OurCustomers, OurCustomersDTO>();

            //Testimonials
            CreateMap<Testimonials, TestimonialsVM>();
            CreateMap<TestimonialsDTO, Testimonials>();
            CreateMap<Testimonials, TestimonialsDTO>();

            // Visitor Registrations
            CreateMap<VisitorRegistrations, VisitorRegistrationsVM>();
            CreateMap<VisitorRegistrationsDTO, VisitorRegistrations>();
            CreateMap<VisitorRegistrations, VisitorRegistrationsDTO>();
        }
    }
}