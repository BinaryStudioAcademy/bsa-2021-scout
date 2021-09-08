using Application;
using Application.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebAPI.Extensions;
using WebAPI.Middleware;
using Infrastructure.Vault.Extensions;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureVault(Configuration);

            services.AddApplication();
            services.AddInfrastructure();

            services.ConfigureJwt(Configuration);
            services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            services.AddHttpContextAccessor();

            services.AddSpecificCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Use bearer token to authorize",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.OperationFilter<AddAuthHeaderOperationFilter>();
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UserSpecificCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ForceJsonMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
