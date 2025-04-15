
using Cloud5mins.ShortenerTools;
using Cloud5mins.ShortenerTools.Tools;

var builder = WebApplication.CreateBuilder(args);       
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services.AddMcpServer()
    .WithTools<UrlShortenerTool>();;

builder.AddServiceDefaults();

builder.Services.AddHttpClient<UrlManagerClient>(client => 
            {
                client.BaseAddress = new Uri("https+http://api");
            });
            
var app = builder.Build();

app.MapMcp();

app.Run();