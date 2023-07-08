﻿using Application.AppSettings;
using Application.ServiceInterfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IoC
{
    public static class DependencyContainerAPP
    {
        public static IServiceCollection AddServicesAPP(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IRandomService, RandomService>();
            services.AddSingleton<IFileService, FileService>();
            return services;
        }
    }
}
