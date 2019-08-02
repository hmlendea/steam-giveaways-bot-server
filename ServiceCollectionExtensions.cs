using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NuciDAL.Repositories;
using NuciLog;
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

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            dataStoreSettings = new DataStoreSettings();
            mailSettings = new MailSettings();

            configuration.Bind(nameof(DataStoreSettings), dataStoreSettings);
            configuration.Bind(nameof(MailSettings), mailSettings);

            services.AddSingleton(dataStoreSettings);
            services.AddSingleton(mailSettings);

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
                .AddScoped<ILogger, NuciLogger>()
                .AddScoped<IRepository<UserEntity>>(x => new XmlRepository<UserEntity>(dataStoreSettings.UserStorePath))
                .AddScoped<IRepository<SteamAccountEntity>>(x => new XmlRepository<SteamAccountEntity>(dataStoreSettings.SteamAccountStorePath))
                .AddScoped<IRepository<RewardEntity>>(x => new XmlRepository<RewardEntity>(dataStoreSettings.RewardsStorePath))
                .AddScoped<ISteamAccountService, SteamAccountService>()
                .AddScoped<IRewardService, RewardService>();
        }
    }
}
