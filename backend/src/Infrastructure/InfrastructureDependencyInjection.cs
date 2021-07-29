using Application.Interfaces;
using Application.Users.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Dapper.Services;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Repositories.Read;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Nest;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDatabaseContext();
            services.AddDapper();

            services.AddWriteRepositories();
            services.AddReadRepositories();

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddElasticEngine();
            return services;
        }
        private static IServiceCollection AddElasticEngine(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
            if (connectionString is null)
                throw new Exception("Elastic connection string url is not specified");
            var settings = new ConnectionSettings(new Uri(connectionString))
                .DefaultIndex("default_index")
                .DefaultMappingFor<User>(m => m
                .IndexName("users")
            );
            services.AddSingleton<IElasticClient>(new ElasticClient(settings));
            return services;
        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

            if (connectionString is null)
                throw new Exception("Database connection string is not specified");

            services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(
                            connectionString,
                            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                        )
                );

            return services;
        }

        private static IServiceCollection AddDapper(this IServiceCollection services)
        {
            services.AddTransient<IConnectionFactory, ConnectionFactory>();

            return services;
        }

        private static IServiceCollection AddWriteRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWriteRepository<User>, WriteRepository<User>>();
            services.AddScoped<IWriteRepository<Applicant>, ElasticWriteRepository<Applicant>>();

            return services;
        }

        private static IServiceCollection AddReadRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadRepository<User>, UserReadRepository>();
            services.AddScoped<IReadRepository<Applicant>, ElasticReadRepository<Applicant>>();

            return services;
        }
    }
}
