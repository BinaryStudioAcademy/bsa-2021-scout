using Application.Interfaces;
using Application.Users.Dtos;
using Domain.Entities;
using Infrastructure.EF;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastracture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddCrudServices();            

            return services;
        }

        private static IServiceCollection AddCrudServices(this IServiceCollection services)
        {
            services.AddScoped<IService<UserDto>, Service<User, UserDto>>();

            return services;
        }
    }
}
