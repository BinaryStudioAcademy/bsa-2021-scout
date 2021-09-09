using Application.Interfaces;
using Application.Interfaces.AWS;
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
using Infrastructure.AWS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Nest;
using Infrastructure.Files.Abstraction;
using Infrastructure.Files.Read;
using Action = Domain.Entities.Action;
using Infrastructure.AWS.S3;
using Infrastructure.AWS.S3.Abstraction;
using Infrastructure.AWS.S3.Services;
using Infrastructure.AWS.Connection;
using Infrastructure.Files.Write;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddWriteRepositories();
            services.AddReadRepositories();

            services.AddDatabaseContext();
            services.AddDapper();
            services.AddMongoDb();
            services.AddElasticEngine();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISecurityService, SecurityService>();

            services.AddEvents();
            services.AddMail();
            services.AddJWT();
            services.AddAWS();
            services.AddFilesManagement();

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
            var elastic = new ElasticClient(settings);
            var exception = elastic.Ping().OriginalException;
            if (exception != null)
                throw exception;
            services.AddSingleton<IElasticClient>(elastic);

            return services;
        }
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services)
        {
            var vaultReadRepository = services.BuildServiceProvider().GetRequiredService<IVaultReadRepository>();

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

        private static IServiceCollection AddAWS(this IServiceCollection services)
        {
            services.AddSingleton<IAwsConnectionFactory, AwsConnectionFactory>();
            services.AddSingleton<IAwsS3ConnectionFactory, AwsS3ConnectionFactory>();
            services.AddSingleton<IAwsS3ReadRepository, AwsS3ReadRepository>();
            services.AddSingleton<IAwsS3WriteRepository, AwsS3WriteRepository>();
            services.AddSingleton<IS3Uploader, AwsS3Uploader>();

            services.AddSingleton<ITextParser, TextParser>();
            services.AddSingleton<IComprehendParser, ComprehendParser>();
            services.AddSingleton<ICvParser, CvParser>();

            return services;
        }

        private static IServiceCollection AddFilesManagement(this IServiceCollection services)
        {
            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<IApplicantCvFileReadRepository, ApplicantCvFileReadRepository>();
            services.AddScoped<IApplicantCvFileWriteRepository, ApplicantCvFileWriteRepository>();

            services.AddScoped<IApplicantPhotoFileWriteRepository, ApplicantPhotoFileWriteRepository>();

            services.AddScoped<IImageReadRepository, ImageReadRepository>();
            services.AddScoped<IImageWriteRepository, ImageWriteRepository>();

            services.AddScoped<IMailAttachmentFileWriteRepository, MailAttachmentFileWriteRepository>();

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
            services.AddScoped<IWriteRepository<ApplyToken>, WriteRepository<ApplyToken>>();
            services.AddScoped<IWriteRepository<Role>, WriteRepository<Role>>();
            services.AddScoped<IWriteRepository<UserToRole>, WriteRepository<UserToRole>>();

            services.AddScoped<IWriteRepository<Interview>, WriteRepository<Interview>>();
            services.AddScoped<IWriteRepository<UsersToInterview>, WriteRepository<UsersToInterview>>();

            services.AddScoped<IWriteRepository<RegisterPermission>, WriteRepository<RegisterPermission>>();
            services.AddScoped<IWriteRepository<Action>, ActionWriteRepository>();
            services.AddScoped<IWriteRepository<CandidateReview>, CandidateReviewWriteRepository>();

            services.AddScoped<IWriteRepository<Applicant>, ApplicantsWriteRepository>();
            services.AddScoped<IApplicantsWriteRepository, ApplicantsWriteRepository>();
            services.AddScoped<IApplicantsFromCsvWriteRepository, ApplicantsFromCsvWriteRepository>();

            services.AddScoped<IWriteRepository<FileInfo>, WriteRepository<FileInfo>>();
            services.AddScoped<IElasticWriteRepository<ElasticEntity>, ElasticWriteRepository<ElasticEntity>>();
            services.AddScoped<IWriteRepository<VacancyCandidate>, WriteRepository<VacancyCandidate>>();
            services.AddScoped<IWriteRepository<CandidateToStage>, WriteRepository<CandidateToStage>>();
            services.AddScoped<IWriteRepository<CandidateReview>, CandidateReviewWriteRepository>();
            services.AddScoped<ICandidateReviewWriteRepository, CandidateReviewWriteRepository>();
            services.AddScoped<IVacancyCandidateWriteRepository, VacancyCandidateWriteRepository>();
            services.AddScoped<IWriteRepository<EmailToken>, WriteRepository<EmailToken>>();
            services.AddScoped<IWriteRepository<MailTemplate>, MongoWriteRepository<MailTemplate>>();
            services.AddScoped<IWriteRepository<Project>, WriteRepository<Project>>();
            services.AddScoped<IWriteRepository<Review>, WriteRepository<Review>>();
            services.AddScoped<IWriteRepository<ReviewToStage>, WriteRepository<ReviewToStage>>();
            services.AddScoped<IWriteRepository<CandidateComment>, WriteRepository<CandidateComment>>();
            services.AddScoped<IWriteRepository<Pool>, WriteRepository<Pool>>();
            services.AddScoped<IWriteRepository<PoolToApplicant>, PoolToApplicantWriteRepository>();
            services.AddScoped<IPoolToApplicantWriteRepository, PoolToApplicantWriteRepository>();
            services.AddScoped<IWriteRepository<MailTemplate>, MongoWriteRepository<MailTemplate>>();
            services.AddScoped<IWriteRepository<CsvFile>, MongoWriteRepository<CsvFile>>();
            services.AddScoped<IWriteRepository<CvParsingJob>, WriteRepository<CvParsingJob>>();
            services.AddScoped<IWriteRepository<SkillsParsingJob>, WriteRepository<SkillsParsingJob>>();

            services.AddScoped<IWriteRepository<Domain.Entities.Action>, WriteRepository<Domain.Entities.Action>>();
            services.AddScoped<IWriteRepository<MailAttachment>, MongoWriteRepository<MailAttachment>>();

            services.AddScoped<IWriteRepository<UserFollowedEntity>, WriteRepository<UserFollowedEntity>>();
            services.AddScoped<IWriteRepository<ArchivedEntity>, WriteRepository<ArchivedEntity>>();

            services.AddScoped<IWriteRepository<ToDoTask>, WriteRepository<ToDoTask>>();
            services.AddScoped<IUserToTaskWriteRepository, UserToTaskWriteRepository>();

            return services;
        }

        private static IServiceCollection AddReadRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReadRepository<User>, UserReadRepository>();
            services.AddScoped<IReadRepository<Vacancy>, VacancyReadRepository>();
            services.AddScoped<IReadRepository<Company>, CompanyReadRepository>();
            services.AddScoped<IReadRepository<Role>, RoleReadRepository>();
            services.AddScoped<IReadRepository<UserToRole>, UserToRoleReadRepository>();
            services.AddScoped<IReadRepository<ApplyToken>, ApplyTokenReadRepository>();
            services.AddScoped<IReadRepository<Action>, ActionReadRepository>();
            services.AddScoped<IReadRepository<CandidateReview>, CandidateReviewReadRepository>();
            services.AddScoped<IReadRepository<CandidateToStage>, CandidateToStageReadRepository>();

            services.AddScoped<IInterviewReadRepository, InterviewReadRepository>();

            services.AddScoped<ICandidateToStageReadRepository, CandidateToStageReadRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IRTokenReadRepository, RTokenReadRepository>();

            services.AddScoped<IHomeDataReadRepository, HomeDataReadRepository>();
            services.AddScoped<IArchivedEntityReadRepository, ArchivedEntityReadRepository>();

            services.AddScoped<IReadRepository<RegisterPermission>, RegisterPermissionReadRepository>();
            services.AddScoped<IVaultReadRepository, VaultReadRepository>();

            services.AddScoped<IElasticReadRepository<ElasticEntity>, ElasticReadRepository<ElasticEntity>>();

            services.AddScoped<IReadRepository<Applicant>, ApplicantReadRepository>();
            services.AddScoped<IApplicantReadRepository, ApplicantReadRepository>();
            services.AddScoped<IUserFollowedReadRepository, UserFollowedEntityReadRepository>();

            services.AddScoped<IStageReadRepository, StageReadRepository>();
            services.AddScoped<IReadRepository<Stage>, StageReadRepository>();
            services.AddScoped<IReadRepository<VacancyCandidate>, VacancyCandidateReadRepository>();
            services.AddScoped<IVacancyCandidateReadRepository, VacancyCandidateReadRepository>();

            services.AddScoped<IVacancyTableReadRepository, VacancyTableReadRepository>();
            services.AddScoped<IVacancyReadRepository, VacancyReadRepository>();

            services.AddScoped<IReadRepository<CvParsingJob>, CvParsingJobReadRepository>();
            services.AddScoped<IReadRepository<SkillsParsingJob>, SkillsParsingJobReadRepository>();

            services.AddScoped<IReadRepository<Project>, ProjectReadRepository>();
            services.AddScoped<IReadRepository<MailTemplate>, MongoReadRespoitory<MailTemplate>>();
            services.AddScoped<IReadRepository<CsvFile>, MongoReadRespoitory<CsvFile>>();
            services.AddScoped<IReadRepository<EmailToken>, EmailTokenReadRepository>();
            services.AddScoped<IReadRepository<Review>, ReviewReadRepository>();

            services.AddScoped<IReadRepository<Pool>, PoolReadRepository>();
            services.AddScoped<IPoolReadRepository, PoolReadRepository>();

            services.AddScoped<IReadRepository<CandidateToStage>, CandidateToStageReadRepository>();

            services.AddScoped<IReadRepository<Domain.Entities.Action>, ActionReadRepository>();

            services.AddScoped<IMailTemplateReadRepository, MailTemplateReadRepository>();

            services.AddScoped<IReadRepository<ReviewToStage>, ReviewToStageReadRepository>();
            services.AddScoped<IReadRepository<CandidateComment>, CandidateCommentReadRepository>();


            services.AddScoped<IReadRepository<ToDoTask>, TaskReadRepository>();
            services.AddScoped<ITaskReadRepository, TaskReadRepository>();
            services.AddScoped<IReadRepository<UsersToInterview>, UserToInterviewRepository>();


            return services;
        }
    }
}
