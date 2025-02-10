using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddAzureTableClient("strTables");

builder.Services.AddTransient<ILogger>(sp => 
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    return loggerFactory.CreateLogger("shortenerLogger");
});

//to remove just while migration
builder.Services.AddSingleton<WeatherService>(); // Register WeatherService

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseHttpsRedirection();

app.MapShortenerEnpoints();

app.MapGet("/weatherforecast", (WeatherService weatherService) => weatherService.GetWeatherForecast())
   .WithName("GetWeatherForecast");

app.Run();

