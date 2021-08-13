using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebAPI.Extensions;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .ApplyDatabaseMigrations()
                .ApplyElasticSeeding()
                .ApplyDatabaseSeeding()
                .ApplyMongoSeeding()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
