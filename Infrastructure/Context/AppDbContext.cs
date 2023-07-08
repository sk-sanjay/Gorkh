using Application.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<DashboardAlerts> DashboardAlerts { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationDetails> NotificationDetails { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RoleMenus> RoleMenus { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<WebApiLogs> WebApiLogs { get; set; }
        public DbSet<WebAppLogs> WebAppLogs { get; set; }
        public DbSet<AuthenticationTickets> AuthenticationTickets { get; set; }
        public DbSet<SiteAuthenticationTickets> SiteAuthenticationTickets { get; set; }
        public DbSet<Audits> AuditLogs { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<SubCategories> SubCategories { get; set; }
        public DbSet<SubSubCategories> SubSubCategories { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Manufacturers> Manufacturers { get; set; }
        public DbSet<ShippingWeights> shippingWeights { get; set; }
        public DbSet<Currents> Currents { get; set; }
        public DbSet<VoltageFrequencies> VoltageFrequencies { get; set; }
        public DbSet<Specifications> Specifications { get; set; }
        public DbSet<SpecificationsSSCategories> SpecificationsSSCategories { get; set; }
        public DbSet<OrganisationTypes> OrganisationTypes { get; set; }
        public DbSet<Buyers> Buyers { get; set; }
        public DbSet<SellerRegistrations> SellerRegistrations { get; set; }
        public DbSet<BuyerSellerRegistrations> BuyerSellerRegistrations { get; set; }
       public DbSet<BuyerSellerRegistrations1> BuyerSellerRegistrations1 { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductsSpecifications> ProductsSpecifications { get; set; }
        public DbSet<Conditions> Conditions { get; set; }
        public DbSet<ProductsLocations> ProductsLocations { get; set; }
        public DbSet<ProductsDescriptions> ProductsDescriptions { get; set; }
        public DbSet<ProductsFileUploads> ProductsFileUploads { get; set; }
        public DbSet<Products1> Products1 { get; set; }
        public DbSet<ProductsEnquiries> ProductsEnquiries { get; set; }
        public DbSet<Banners> Banners { get; set; }
        public DbSet<BuyerRequirements> BuyerRequirements { get; set; }
        public DbSet<ProductsVisitors> ProductsVisitors { get; set; }
        public DbSet<ProductsBuyerIntrests> ProductsBuyerIntrests { get; set; }
        public DbSet<ProductsBuyerFavorites> ProductsBuyerFavorites { get; set; }
        public DbSet<ProductsPurchases> ProductsPurchases { get; set; }
        public DbSet<ProductsBuyerQueries> ProductsBuyerQueries { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<CategoryimgCommon> CategoryimgCommon { get; set; }

        // For other link  ........
        public DbSet<OtherLinkHeading> OtherLinkHeading { get; set; }
        public DbSet<TempOtherLinkHeading> TempOtherLinkHeading { get; set; }

        public DbSet<BuyerSellerCount> BuyerSellerCount { get; set; }
        public DbSet<ProductCount> ProductCount { get; set; }
        public DbSet<BuyerOffers> BuyerOffers { get; set; }
        public DbSet<OurCustomers> OurCustomers { get; set; }
        public DbSet<Testimonials> Testimonials { get; set; }
        public DbSet<VisitorRegistrations> VisitorRegistrations { get; set; }
    }
}