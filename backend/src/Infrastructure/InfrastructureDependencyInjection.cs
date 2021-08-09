﻿using Application.Interfaces;
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

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastracture(this IServiceCollection services)
        {
            services.AddDatabaseContext();

            services.AddDapper();
            services.AddMongoDb();

            services.AddWriteRepositories();
            services.AddReadRepositories();

            services.AddEvents();
            services.AddMail();
            services.AddJWT();

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

        private static IServiceCollection AddWriteRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWriteRepository<User>, WriteRepository<User>>();
            services.AddScoped<IWriteRepository<RefreshToken>, WriteRepository<RefreshToken>>();
            services.AddScoped<IWriteRepository<ApplicantCv>, MongoWriteRepository<ApplicantCv>>();
            services.AddScoped<IWriteRepository<VacancyCandidate>, WriteRepository<VacancyCandidate>>();
            services.AddScoped<IWriteRepository<CandidateToStage>, CandidateToStageWriteRepository>();
            services.AddScoped<ICandidateToStageWriteRepository, CandidateToStageWriteRepository>();
            services.AddScoped<IWriteRepository<EmailToken>, WriteRepository<EmailToken>>();


            return services;
        }

        private static IServiceCollection AddReadRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadRepository<User>, UserReadRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IRTokenReadRepository, RTokenReadRepository>();
            services.AddScoped<IReadRepository<ApplicantCv>, MongoReadRespoitory<ApplicantCv>>();
            services.AddScoped<IStageReadRepository, StageReadRepository>();
            services.AddScoped<IReadRepository<Stage>, StageReadRepository>();
            services.AddScoped<IReadRepository<VacancyCandidate>, VacancyCandidateReadRepository>();
            services.AddScoped<IVacancyCandidateReadRepository, VacancyCandidateReadRepository>();
            services.AddScoped<IMailTemplateReadRepository, MailTemplateReadRepository>();
            services.AddScoped<IReadRepository<EmailToken>, EmailTokenReadRepository>();


            return services;
        }
    }
}
