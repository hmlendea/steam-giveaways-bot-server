using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NuciDAL.Repositories;
using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;
using NuciSecurity.HMAC;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Communication;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.Client;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;
using SteamGiveawaysBot.Server.Security;
using SteamGiveawaysBot.Server.Service;

namespace SteamGiveawaysBot.Server
{
    public static class ServiceCollectionExtensions
    {
        static DataStoreSettings dataStoreSettings;
        static MailSettings mailSettings;
        static TelegramSettings telegramSettings;
        static NuciLoggerSettings loggingSettings;

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            dataStoreSettings = new DataStoreSettings();
            mailSettings = new MailSettings();
            telegramSettings = new TelegramSettings();
            loggingSettings = new NuciLoggerSettings();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(MailSettings), mailSettings);
            configuration.Bind(nameof(TelegramSettings), telegramSettings);
            configuration.Bind(nameof(NuciLoggerSettings), loggingSettings);

            services.AddSingleton(dataStoreSettings);
            services.AddSingleton(mailSettings);
            services.AddSingleton(telegramSettings);
            services.AddSingleton(loggingSettings);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHmacEncoder<SteamAccountRequest>, SteamAccountRequestHmacEncoder>()
                .AddSingleton<IHmacEncoder<SteamAccountResponse>, SteamAccountResponseHmacEncoder>()
                .AddSingleton<IHmacEncoder<RecordRewardRequest>, RecordRewardRequestHmacEncoder>()
                .AddSingleton<IStorefrontDataRetriever, StorefrontDataRetriever>()
                .AddSingleton<IMailSender, GmailMailSender>()
                .AddSingleton<INotificationSender, TelegramNotificationSender>()
                .AddSingleton<ILogger, NuciLogger>()
                .AddSingleton<IRewardNotifier, RewardNotifier>()
                .AddSingleton<IRepository<UserEntity>>(x => new XmlRepository<UserEntity>(dataStoreSettings.UserStorePath))
                .AddSingleton<IRepository<SteamAccountEntity>>(x => new XmlRepository<SteamAccountEntity>(dataStoreSettings.SteamAccountStorePath))
                .AddSingleton<IRepository<RewardEntity>>(x => new XmlRepository<RewardEntity>(dataStoreSettings.RewardsStorePath))
                .AddSingleton<ISteamAccountService, SteamAccountService>()
                .AddSingleton<IRewardService, RewardService>();
        }
    }
}
