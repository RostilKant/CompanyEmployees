using Contracts;
using Entities;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyEmployees.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });
        
        public static void ConfigureKestrelIntegration(this IServiceCollection services) =>
            services.Configure<KestrelServerOptions>(options =>
            {
            });
        
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigurePostgreSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("CompanyEmployees")));


    }
}