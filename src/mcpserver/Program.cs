using Cloud5mins.ShortenerTools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

var builder = Host.CreateApplicationBuilder(args);
            
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.AddServiceDefaults();

// builder.Services.AddHttpClient<UrlManagerClient>(client => 
//             {
//                 client.BaseAddress = new Uri("https+http://api");
//             });
            
await builder.Build().RunAsync();