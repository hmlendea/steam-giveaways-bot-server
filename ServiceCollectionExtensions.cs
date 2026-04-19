using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NuciDAL.Repositories;
using NuciLog;
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
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            DataStoreSettings dataStoreSettings = new();
            NotificationSettings notificationSettings = new();
            SecuritySettings securitySettings = new();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(NotificationSettings), notificationSettings);
            configuration.Bind(nameof(SecuritySettings), securitySettings);

            return services
                .AddSingleton(dataStoreSettings)
                .AddSingleton(notificationSettings)
                .AddSingleton(securitySettings)
                .AddNuciNotificationsSettings(configuration)
                .AddNuciLoggerSettings(configuration);
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services) => services
            .AddSingleton<IStorefrontDataRetriever, StorefrontDataRetriever>()
            .AddSingleton<INuciNotificationsClient>(x => new NuciNotificationsClient(x.GetRequiredService<NuciNotificationsSettings>()))
            .AddSingleton<ILogger, NuciLogger>()
            .AddSingleton<IFileRepository<UserEntity>>(x => new XmlRepository<UserEntity>(x.GetRequiredService<DataStoreSettings>().UserStorePath))
            .AddSingleton<IFileRepository<SteamAccountEntity>>(x => new XmlRepository<SteamAccountEntity>(x.GetRequiredService<DataStoreSettings>().SteamAccountStorePath))
            .AddSingleton<IFileRepository<RewardEntity>>(x => new JsonRepository<RewardEntity>(x.GetRequiredService<DataStoreSettings>().RewardsStorePath))
            .AddSingleton<ISteamAccountService, SteamAccountService>()
            .AddSingleton<IRewardService, RewardService>()
            .AddSingleton<IUserService, UserService>();
    }
}
