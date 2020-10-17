using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using shortenerTools.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace shortenerTools.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IConfiguration CreateConfiguration(this IServiceCollection services)
        {
            var providers = new List<IConfigurationProvider>();
            var serviceProvider = services.BuildServiceProvider();
            if (serviceProvider.GetService<IConfiguration>() is IConfigurationRoot existingConfigInstance)
            {
                providers.AddRange(existingConfigInstance.Providers);
            }

            var configBuilder = new ConfigurationBuilder();
            var customSettingsPath = GetCustomSettingsPath(serviceProvider);
            var config = configBuilder
                .SetBasePath(customSettingsPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{EnvironmentUtilities.AspNetCoreEnvironment()}.json", true, true)
                .AddJsonFile("settings.json", true, true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                .AddEnvironmentVariables()
                .Build();

            var configuration = new ConfigurationRoot(providers);

            services.AddSingleton<IConfiguration>(configuration);

            return configuration;
        }

        private static string GetCustomSettingsPath(IServiceProvider serviceProvider)
        {
            if (EnvironmentUtilities.IsLocal()) return Directory.GetCurrentDirectory();
            return serviceProvider?.GetService<IOptions<ExecutionContextOptions>>()?.Value?.AppDirectory
                   ?? Environment.CurrentDirectory;
        }
    }
}