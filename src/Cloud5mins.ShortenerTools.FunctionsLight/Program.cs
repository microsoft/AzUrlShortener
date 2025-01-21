using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cloud5mins.ShortenerTools.Core.Domain;

var builder = FunctionsApplication.CreateBuilder(args);

// Bind configuration to ShortenerSettings
builder.Services.Configure<ShortenerSettings>(builder.Configuration.GetSection("Values"));

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();