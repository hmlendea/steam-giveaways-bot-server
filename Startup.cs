using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteamGiveawaysBot.Server.Configuration;
using SteamGiveawaysBot.Server.DataAccess.DataObjects;

namespace SteamGiveawaysBot.Server
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddConfigurations(Configuration)
                .AddCustomServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ensure the stores exist
            var dataStoreSettings = app.ApplicationServices.GetRequiredService<DataStoreSettings>();
            CreateStoreIfMissing(dataStoreSettings.RewardsStorePath, nameof(RewardEntity));
            CreateStoreIfMissing(dataStoreSettings.UserStorePath, nameof(UserEntity));
            CreateStoreIfMissing(dataStoreSettings.SteamAccountStorePath, nameof(SteamAccountEntity));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static void CreateStoreIfMissing(string storePath, string entityTypeName)
        {
            var directory = Path.GetDirectoryName(storePath);

            if (!string.IsNullOrWhiteSpace(directory) &&
                !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!string.IsNullOrWhiteSpace(storePath) &&
                !File.Exists(storePath))
            {
                File.WriteAllText(storePath, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><{entityTypeName} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></{entityTypeName}>");
            }
        }
    }
}
