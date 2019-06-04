using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.DataAccess.Repositories;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            DataStoreSettings dataStoreSettings = new DataStoreSettings();
            MailSettings mailSettings = new MailSettings();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(MailSettings), mailSettings);

            services.AddSingleton(dataStoreSettings);
            services.AddSingleton(mailSettings);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IHmacEncoder<SteamAccountRequest>, SteamAccountRequestHmacEncoder>()
                .AddScoped<IHmacEncoder<SteamAccountResponse>, SteamAccountResponseHmacEncoder>()
                .AddScoped<IHmacEncoder<RecordRewardRequest>, RecordRewardRequestHmacEncoder>()
                .AddScoped<ISteamAccountService, SteamAccountService>()
                .AddScoped<IRewardService, RewardService>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ISteamAccountRepository, SteamAccountRepository>()
                .AddScoped<IRewardRepository, RewardRepository>();
        }
    }
}
