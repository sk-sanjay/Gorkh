using Application.ServiceInterfaces;
using Application.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Rotativa.AspNetCore;
using System;
using WebSite.Helpers;
using WebSite.Middlewares;

namespace WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServicesSite(Configuration);
            services.AddResponseCaching();
            services.AddControllers(); // this is necessary for the captcha's image provider
            services.AddNotyf(notyfConfig => { notyfConfig.DurationInSeconds = 7; notyfConfig.IsDismissable = true; notyfConfig.Position = NotyfPosition.TopRight; });
            services.AddHttpClient<IHttpClientService, HttpClientServiceSite>();
            var mvcBuilder = services.AddRazorPages()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif

            services.AddAntiforgery(o =>
            {
                o.HeaderName = "XSRF-TOKEN";
                o.Cookie.Domain = Configuration["Domain"];
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
                o.Cookie.Name = ".SURPS.Antiforgery";
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Errors/AccessDenied/";
                    options.ClaimsIssuer = "Site";
                    options.LoginPath = "/Account/Seller-Login/";
                    options.LogoutPath = "/";
                    options.Cookie.Domain = Configuration["Domain"];
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(Configuration["CookieExpiry"]));
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                    options.Cookie.IsEssential = true;
                    options.Cookie.Name = ".SURPS.AuthCookie";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SessionStore = new CustomTicketStore(services);
                });
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Domain = Configuration["Domain"];
                options.IdleTimeout = TimeSpan.FromMinutes(120);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".SURPS.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment hostingEnvironment, IEmailService emailService)
        {
            app.UseNotyf();
            app.UseRewriter(new RewriteOptions()
                .AddRedirectToHttps(StatusCodes.Status301MovedPermanently));
            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionHandlingMiddleware>();
                app.UseStatusCodePagesWithReExecute("/Errors/Exception/{0}");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    //const int durationInSeconds = 60 * 60 * 24 * 365;
                    const int durationInSeconds = 60;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] = "private,max-age=" + durationInSeconds;
                }
            });
            //app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                // this is necessary for the captcha's image provider
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Common}/{action=Index}/{id?}");
            });
            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)hostingEnvironment);
        }
    }
}