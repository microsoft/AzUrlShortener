using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using shortenerTools;
using shortenerTools.Abstractions;
using shortenerTools.Implementations;
using System;
using System.Diagnostics.CodeAnalysis;

[assembly: FunctionsStartup(typeof(Startup))]
namespace shortenerTools
{
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionStartupBase
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = CreateConfiguration(builder);

            builder.Services.AddHttpClient<IUserIpLocationService, UserIpLocationService>(client =>
            {
                client.BaseAddress = new Uri(config.GetSection("IpLocationService:Url").Value);
            });
        }
    }
}