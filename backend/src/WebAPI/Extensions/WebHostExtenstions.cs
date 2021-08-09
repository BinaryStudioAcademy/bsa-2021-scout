using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Infrastructure.EF;
using Infrastructure.EF.Seeds;
using Infrastructure.Elastic.Seeding;
using Infrastructure.Repositories.Read;
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

            await client.IndexManyAsync<ApplicantToTags>(
                ApplicantToTagsSeeds.GetSeed()
            );
            
            return host;
        }
        public async static Task<IHost> ApplyVacancySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Vacancy>>();

            foreach(var vacancy in VacancySeeds.Vacancies){
                await repo.CreateAsync(vacancy);
            }
            
            return host;
        }
    }
}
