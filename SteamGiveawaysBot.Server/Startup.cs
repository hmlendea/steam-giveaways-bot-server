using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NuciAPI.Middleware.ExceptionHandling;
using NuciAPI.Middleware.Logging;
using NuciAPI.Middleware.Security;
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
                .AddNuciApiScannerProtection()
                .AddNuciApiReplayProtection()
                .AddCustomServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Ensure the stores exist
            var dataStoreSettings = app.ApplicationServices.GetRequiredService<DataStoreSettings>();
            CreateStoreIfMissing(dataStoreSettings.RewardStorePath, nameof(RewardEntity));
            CreateStoreIfMissing(dataStoreSettings.UserStorePath, nameof(UserEntity));
            CreateStoreIfMissing(dataStoreSettings.SteamAccountStorePath, nameof(SteamAccountEntity));

            app.UseNuciApiExceptionHandling();
            app.UseNuciApiScannerProtection();
            app.UseNuciApiRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseNuciApiHeaderValidation();
            app.UseNuciApiReplayProtection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        static void CreateStoreIfMissing(string storePath, string entityTypeName)
        {
            var directoryPath = Path.GetDirectoryName(storePath);

            if (!string.IsNullOrWhiteSpace(directoryPath) &&
                !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!string.IsNullOrWhiteSpace(storePath) &&
                !File.Exists(storePath))
            {
                if (storePath.EndsWith(".json"))
                {
                    File.WriteAllText(storePath, $"[]");
                    return;
                }

                File.WriteAllText(storePath, $"<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOf{entityTypeName} xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></ArrayOf{entityTypeName}>");
            }
        }
    }
}
