using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Cloud5mins.ShortenerTools
{
    public class Program
    {
        public static void Main()
        {
            ShortenerSettings shortenerSettings = null;

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    // Add our global configuration instance
                    services.AddSingleton(options =>
                    {
                        var configuration = context.Configuration;
                        shortenerSettings = new ShortenerSettings();
                        configuration.Bind(shortenerSettings);
                        return configuration;
                    });

                    // Add our configuration class
                    services.AddSingleton(options => { return shortenerSettings; });

                    // Add OpenAPI definition
                    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
                    {
                        var options = new OpenApiConfigurationOptions()
                        {
                            Info = new OpenApiInfo()
                            {
                                Version = "1.0.0",
                                Title = "Azure Url Shortener",
                                Description = "Azure Url Shortener",
                                TermsOfService = new Uri("https://github.com/microsoft/AzUrlShortener"),
                                Contact = new OpenApiContact()
                                {
                                    Name = "Contact the developer",
                                    Url = new Uri("https://github.com/microsoft/AzUrlShortener/issues"),
                                },
                                License = new OpenApiLicense()
                                {
                                    Name = "MIT",
                                    Url = new Uri("https://github.com/microsoft/AzUrlShortener/blob/main/LICENSE"),
                                }
                            },
                            Servers = DefaultOpenApiConfigurationOptions.GetHostNames(),
                            OpenApiVersion = OpenApiVersionType.V2,
                            IncludeRequestingHostName = true,
                            ForceHttps = false,
                            ForceHttp = false,
                        };

                        return options;
                    });
                })
                .Build();

            host.Run();
        }
    }
}