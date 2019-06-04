using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SteamGiveawaysBot.Server.Api.Models;
using SteamGiveawaysBot.Server.Configuration;
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
            ApplicationSettings applicationSettings = new ApplicationSettings();
            MailSettings mailSettings = new MailSettings();

            configuration.Bind(nameof(ApplicationSettings), applicationSettings);
            configuration.Bind(nameof(mailSettings), mailSettings);

            services.AddSingleton(applicationSettings);
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
