using Domain.Entities;
using Infrastructure.EF;
using Infrastructure.EF.Seeding;
using Domain.Interfaces.Abstractions;
using Infrastructure.Mongo.Interfaces;
using Infrastructure.Mongo.Seeding;
using Infrastructure.Elastic.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;

namespace WebAPI.Extensions
{
    public static class WebHostExtenstions
    {
        public static IHost ApplyDatabaseMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();

            return host;
        }
        public static IHost ApplyElasticSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var client = scope.ServiceProvider.GetService<IElasticClient>();

            client.IndexMany<ElasticEntity>(
                ApplicantTagsSeeds.GetSeed()
            );

            return host;
        }

        public static IHost ApplyMongoSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var connectionFactory = scope.ServiceProvider.GetService<IMongoConnectionFactory>();
            var repository = scope.ServiceProvider.GetService<IReadRepository<MailTemplate>>();
            var connection = connectionFactory.GetMongoConnection();
            var collection = connection.GetCollection<MailTemplate>(typeof(MailTemplate).Name);

            try
            {
                MailTemplate check = repository.GetByPropertyAsync("Slug", "default").Result;
            }
            catch
            {
                collection.InsertOne(MailTemplatesSeeds.GetSeed());
            }

            return host;
        }
        public static IHost ApplyDatabaseSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            ApplicationDbContextSeeding.Seed(scope);

            return host;
        }
    }
}
