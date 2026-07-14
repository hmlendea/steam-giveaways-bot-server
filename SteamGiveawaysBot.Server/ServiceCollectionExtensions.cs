using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NuciDAL.Repositories;

using NuciLog;
using NuciLog.Core;

using NuciNotifications.Client;
using NuciNotifications.Client.Configuration;

using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurations(
            this IServiceCollection services,
            IConfiguration configuration)
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
            .AddSingleton<INuciNotificationsClient>(
                serviceProvider => new NuciNotificationsClient(
                    serviceProvider.GetRequiredService<NuciNotificationsSettings>()))
            .AddSingleton<ILogger, NuciLogger>()
            .AddSingleton<IFileRepository<UserDataObject>>(
                serviceProvider => new XmlRepository<UserDataObject>(
                    serviceProvider.GetRequiredService<DataStoreSettings>().UserStorePath))
            .AddSingleton<IFileRepository<SteamAccountDataObject>>(
                serviceProvider => new XmlRepository<SteamAccountDataObject>(
                    serviceProvider
                        .GetRequiredService<DataStoreSettings>()
                        .SteamAccountStorePath))
            .AddSingleton<IFileRepository<RewardDataObject>>(
                serviceProvider => new JsonRepository<RewardDataObject>(
                    serviceProvider.GetRequiredService<DataStoreSettings>().RewardStorePath))
            .AddSingleton<ISteamAccountService, SteamAccountService>()
            .AddSingleton<IRewardService, RewardService>()
            .AddSingleton<IUserService, UserService>();
    }
}
