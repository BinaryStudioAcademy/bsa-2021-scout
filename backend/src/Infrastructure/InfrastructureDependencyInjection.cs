using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Dapper.Services;
using Infrastructure.Mongo.Interfaces;
using Infrastructure.Mongo.Services;
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
            services.AddMongoDb();
            services.AddElasticEngine();

            services.AddWriteRepositories();
            services.AddReadRepositories();

            services.AddScoped<IDomainEventService, DomainEventService>();
            return services;
        }
        private static IServiceCollection AddElasticEngine(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
            if (connectionString is null)
                throw new Exception("Elastic connection string url is not specified");
            var settings = new ConnectionSettings(new Uri(connectionString))
                .DefaultIndex("default_index")
                .DefaultMappingFor<ApplicantToTags>(m => m
                .IndexName("applicant_to_tags")
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
            services.AddScoped<IConnectionFactory, ConnectionFactory>();

            return services;
        }

        private static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            services.AddScoped<IMongoConnectionFactory, MongoConnectionFactory>();

            return services;
        }

        private static IServiceCollection AddWriteRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWriteRepository<User>, WriteRepository<User>>();
            services.AddScoped<IWriteRepository<ApplicantCv>, MongoWriteRepository<ApplicantCv>>();

            return services;
        }

        private static IServiceCollection AddReadRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadRepository<User>, UserReadRepository>();
            services.AddScoped<IReadRepository<ApplicantCv>, MongoReadRespoitory<ApplicantCv>>();
            return services;
        }
    }
}
