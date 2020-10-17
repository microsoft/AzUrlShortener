using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using shortenerTools.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace shortenerTools.Abstractions
{
    [ExcludeFromCodeCoverage]
    public abstract class FunctionStartupBase : FunctionsStartup
    {
        protected IConfiguration CreateConfiguration(
            IFunctionsHostBuilder builder)
        {
            var configuration = builder.Services.CreateConfiguration();
            
            return configuration;
        }
    }
}