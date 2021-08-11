using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.EF;
using Infrastructure.EF.Seeding;
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
        public async static Task<IHost> ApplyElasticSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var client = scope.ServiceProvider.GetService<IElasticClient>();

            await client.IndexManyAsync<ElasticEntity>(
                ApplicantTagsSeeds.GetSeed()
            );
            
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
