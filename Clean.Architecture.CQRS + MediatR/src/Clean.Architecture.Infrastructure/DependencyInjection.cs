using Clean.Architecture.Core.Common.Interfaces.Authentication;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Authentication;
using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Repositories;
using Clean.Architecture.Infrastructure.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clean.Architecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddHttpClient("DummyJSON", client =>
            {
                client.BaseAddress = new Uri("https://dummyjson.com"); 
            });
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
