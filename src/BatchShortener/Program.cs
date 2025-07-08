using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();

        // Add CosmosDB client as a singleton
        services.AddSingleton(sp =>
        {
            string connectionString = Environment.GetEnvironmentVariable("CosmosDbConnectionString") ??
                throw new InvalidOperationException("CosmosDbConnectionString environment variable is not set");

            var cosmosClientOptions = new CosmosClientOptions
            {
                HttpClientFactory = () =>
                {
                    HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                    {
                        // Enable for development purposes only
                        // ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    return new HttpClient(httpMessageHandler);
                },
                ConnectionMode = ConnectionMode.Gateway,
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                }
            };

            return new CosmosClient(connectionString, cosmosClientOptions);
        });
    })
    .Build();

host.Run();
