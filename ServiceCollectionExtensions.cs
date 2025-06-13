using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NuciDAL.Repositories;
using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;

using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server
{
    public static class ServiceCollectionExtensions
    {
        static DataStoreSettings dataStoreSettings;
        static TelegramSettings telegramSettings;
        static NuciLoggerSettings loggingSettings;

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            dataStoreSettings = new DataStoreSettings();
            telegramSettings = new TelegramSettings();
            loggingSettings = new NuciLoggerSettings();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(TelegramSettings), telegramSettings);
            configuration.Bind(nameof(NuciLoggerSettings), loggingSettings);

            services.AddSingleton(dataStoreSettings);
            services.AddSingleton(telegramSettings);
            services.AddSingleton(loggingSettings);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services) => services
            .AddSingleton<IStorefrontDataRetriever, StorefrontDataRetriever>()
            .AddSingleton<INotificationSender, TelegramNotificationSender>()
            .AddSingleton<ILogger, NuciLogger>()
            .AddSingleton<IRepository<UserEntity>>(x => new XmlRepository<UserEntity>(dataStoreSettings.UserStorePath))
            .AddSingleton<IRepository<SteamAccountEntity>>(x => new XmlRepository<SteamAccountEntity>(dataStoreSettings.SteamAccountStorePath))
            .AddSingleton<IRepository<RewardEntity>>(x => new XmlRepository<RewardEntity>(dataStoreSettings.RewardsStorePath))
            .AddSingleton<ISteamAccountService, SteamAccountService>()
            .AddSingleton<IRewardService, RewardService>()
            .AddSingleton<IUserService, UserService>();
    }
}
