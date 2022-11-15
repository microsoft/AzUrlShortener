using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Cloud5mins.domain;
using Microsoft.Extensions.DependencyInjection;

namespace Cloud5mins.AdminApi
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
                })
                .Build();

            host.Run();
        }
    }
}