using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Authentication;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Repositories;
using Clean.Architecture.Infrastructure.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Clean.Architecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpClient("DummyJSON", (httpClient) =>
            {
                httpClient.BaseAddress = new Uri("https://dummyjson.com");
            });
            // add logger service
            //builder.Host.UseSerilog((context, configuration) =>
            //    configuration.ReadFrom.Configuration(context.Configuration));

           /* // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // Add logger service
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Clear default logging providers
                loggingBuilder.AddSerilog(dispose: true); // Add Serilog
            });*/
            services.AddTransient<IProductRepository, ProductRepository>();
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<AppDbContext>();
            services.AddScoped(sp => sp.GetRequiredService<AppDbContext>().CreateConnection());
            services.AddScoped<IDbConnectionWrapper, DapperDbConnectionWrapper>();
            services.AddTransient<IUnitOfWork, UnitOfWorkRepository>();
            
            return services;
        }

        
    }
}
