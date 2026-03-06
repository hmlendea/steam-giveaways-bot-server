using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NuciDAL.Repositories;
using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service;
using NuciNotifications.Client;
using NuciNotifications.Client.Configuration;

namespace SteamGiveawaysBot.Server
{
    public static class ServiceCollectionExtensions
    {
        static DataStoreSettings dataStoreSettings;
        static NotificationSettings notificationSettings;
        static SecuritySettings securitySettings;
        static NuciNotificationsSettings nuciNotificationsSettings;
        static NuciLoggerSettings loggingSettings;

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            dataStoreSettings = new DataStoreSettings();
            notificationSettings = new NotificationSettings();
            securitySettings = new SecuritySettings();
            nuciNotificationsSettings = new NuciNotificationsSettings();
            loggingSettings = new NuciLoggerSettings();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(NotificationSettings), notificationSettings);
            configuration.Bind(nameof(SecuritySettings), securitySettings);
            configuration.Bind(nameof(NuciNotificationsSettings), nuciNotificationsSettings);
            configuration.Bind(nameof(NuciLoggerSettings), loggingSettings);

            services.AddSingleton(dataStoreSettings);
            services.AddSingleton(notificationSettings);
            services.AddSingleton(securitySettings);
            services.AddSingleton(nuciNotificationsSettings);
            services.AddSingleton(loggingSettings);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services) => services
            .AddSingleton<IStorefrontDataRetriever, StorefrontDataRetriever>()
            .AddSingleton<INuciNotificationsClient>(x => new NuciNotificationsClient(nuciNotificationsSettings))
            .AddSingleton<ILogger, NuciLogger>()
            .AddSingleton<IFileRepository<UserEntity>>(x => new XmlRepository<UserEntity>(dataStoreSettings.UserStorePath))
            .AddSingleton<IFileRepository<SteamAccountEntity>>(x => new XmlRepository<SteamAccountEntity>(dataStoreSettings.SteamAccountStorePath))
            .AddSingleton<IFileRepository<RewardEntity>>(x => new JsonRepository<RewardEntity>(dataStoreSettings.RewardsStorePath))
            .AddSingleton<ISteamAccountService, SteamAccountService>()
            .AddSingleton<IRewardService, RewardService>()
            .AddSingleton<IUserService, UserService>();
    }
}
