using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using Domain.Interfaces.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Dapper.Services;
using Infrastructure.Mongo.Interfaces;
using Infrastructure.Mongo.Services;
using Infrastructure.EF;
using Infrastructure.Repositories.Read;
using Infrastructure.Repositories.Write;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Services;
using Infrastructure.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Nest;
using Domain.Interfaces;
using Infrastructure.Files.Abstraction;
using Infrastructure.Files.Read;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDatabaseContext();

            services.AddDapper();
            services.AddMongoDb();

            services.AddWriteRepositories();
            services.AddReadRepositories();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISecurityService, SecurityService>();

            services.AddEvents();
            services.AddMail();
            services.AddJWT();
            services.AddFilesManagement();

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
                .DefaultMappingFor<ElasticEntity>(m => m
                .IndexName("elastic_entity")
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

        private static IServiceCollection AddEvents(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventService, DomainEventService>();

            return services;
        }

        private static IServiceCollection AddMail(this IServiceCollection services)
        {
            services.AddScoped<IMailBuilderService, MailBuilderService>();
            services.AddScoped<ISmtpFactory, GmailSmtpFactory>();

            return services;
        }

        private static IServiceCollection AddJWT(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISecurityService, SecurityService>();

            return services;
        }

        private static IServiceCollection AddFilesManagement(this IServiceCollection services)
        {
            services.AddScoped<IAwsS3ConnectionFactory, AwsS3ConnectionFactory>();

            services.AddScoped<IFileReadRepository, AwsS3FileReadRepository>();
            services.AddScoped<IFileWriteRepository, AwsS3FileWriteRepository>();

            services.AddScoped<IApplicantCvFileReadRepository, ApplicantCvFileReadRepository>();
            services.AddScoped<IApplicantCvFileWriteRepository, ApplicantCvFileWriteRepository>();

            return services;
        }

        private static IServiceCollection AddWriteRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWriteRepository<User>, WriteRepository<User>>();
            services.AddScoped<IWriteRepository<Vacancy>, WriteRepository<Vacancy>>();
            services.AddScoped<IWriteRepository<Project>, WriteRepository<Project>>();
            services.AddScoped<IWriteRepository<Company>, WriteRepository<Company>>();
            services.AddScoped<IWriteRepository<Stage>, WriteRepository<Stage>>();
            services.AddScoped<IWriteRepository<RefreshToken>, WriteRepository<RefreshToken>>();
            services.AddScoped<IWriteRepository<Role>, WriteRepository<Role>>();
            services.AddScoped<IWriteRepository<UserToRole>, WriteRepository<UserToRole>>();
            services.AddScoped<IWriteRepository<RegisterPermission>, WriteRepository<RegisterPermission>>();

            services.AddScoped<IWriteRepository<Applicant>, ApplicantsWriteRepository>();
            services.AddScoped<IApplicantsFromCsvWriteRepository, ApplicantsFromCsvWriteRepository>();

            services.AddScoped<IWriteRepository<FileInfo>, WriteRepository<FileInfo>>();
            services.AddScoped<IElasticWriteRepository<ElasticEntity>, ElasticWriteRepository<ElasticEntity>>();

            services.AddScoped<IWriteRepository<VacancyCandidate>, WriteRepository<VacancyCandidate>>();
            services.AddScoped<IWriteRepository<CandidateToStage>, CandidateToStageWriteRepository>();
            services.AddScoped<ICandidateToStageWriteRepository, CandidateToStageWriteRepository>();
            services.AddScoped<IVacancyCandidateWriteRepository, VacancyCandidateWriteRepository>();
            services.AddScoped<IWriteRepository<EmailToken>, WriteRepository<EmailToken>>();
            services.AddScoped<IWriteRepository<Project>, WriteRepository<Project>>();
            services.AddScoped<IWriteRepository<Pool>, WriteRepository<Pool>>();
            services.AddScoped<IWriteRepository<PoolToApplicant>, PoolToApplicantWriteRepository>();
            services.AddScoped<IPoolToApplicantWriteRepository, PoolToApplicantWriteRepository>();

            return services;
        }

        private static IServiceCollection AddReadRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadRepository<User>, UserReadRepository>();
            services.AddScoped<IReadRepository<Vacancy>, VacancyReadRepository>();
            services.AddScoped<IReadRepository<Project>, ProjectReadRepository>();
            services.AddScoped<IReadRepository<Company>, CompanyReadRepository>();
            services.AddScoped<IReadRepository<Role>, RoleReadRepository>();
            services.AddScoped<IReadRepository<UserToRole>, UserToRoleReadRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IRTokenReadRepository, RTokenReadRepository>();
            services.AddScoped<IReadRepository<RegisterPermission>, RegisterPermissionReadRepository>();

            services.AddScoped<IElasticReadRepository<ElasticEntity>, ElasticReadRepository<ElasticEntity>>();

            services.AddScoped<IApplicantReadRepository, ApplicantReadRepository>();

            services.AddScoped<IStageReadRepository, StageReadRepository>();
            services.AddScoped<IReadRepository<Stage>, StageReadRepository>();
            services.AddScoped<IReadRepository<VacancyCandidate>, VacancyCandidateReadRepository>();
            services.AddScoped<IVacancyCandidateReadRepository, VacancyCandidateReadRepository>();
    
            services.AddScoped<IVacancyTableReadRepository, VacancyTableReadRepository>();
            services.AddScoped<IVacancyReadRepository, VacancyReadRepository>();

            services.AddScoped<IReadRepository<Project>, ProjectReadRepository>();
            services.AddScoped<IReadRepository<MailTemplate>, MongoReadRespoitory<MailTemplate>>();
            services.AddScoped<IReadRepository<EmailToken>, EmailTokenReadRepository>();

            services.AddScoped<IReadRepository<Pool>, PoolReadRepository>();
            services.AddScoped<IPoolReadRepository, PoolReadRepository>();


            return services;
        }
    }
}
