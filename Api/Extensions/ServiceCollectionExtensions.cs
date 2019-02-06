using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SteamAccountDistributor.Api.Models;
using SteamAccountDistributor.Core.Configuration;
using SteamAccountDistributor.DataAccess.DataObjects;
using SteamAccountDistributor.DataAccess.Repositories;
using SteamAccountDistributor.Security;
using SteamAccountDistributor.Service;

namespace SteamAccountDistributor.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            SteamAccountDistributorConfiguration config = new SteamAccountDistributorConfiguration();
            configuration.Bind(nameof(SteamAccountDistributorConfiguration), config);
            services.AddSingleton(config);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IHmacEncoder<SteamAccountRequest>, SteamAccountRequestHmacEncoder>()
                .AddScoped<IHmacEncoder<SteamAccountResponse>, SteamAccountResponseHmacEncoder>()
                .AddScoped<ISteamAccountService, SteamAccountService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ISteamAccountRepository, SteamAccountRepository>();
        }
    }
}
