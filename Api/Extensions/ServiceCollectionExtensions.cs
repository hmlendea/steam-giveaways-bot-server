using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Core.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.DataAccess.Repositories;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationConfiguration config = new ApplicationConfiguration();
            configuration.Bind(nameof(ApplicationConfiguration), config);
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
