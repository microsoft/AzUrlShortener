using Microsoft.FluentUI.AspNetCore.Components;
using Cloud5mins.ShortenerTools.TinyBlazorAdmin.Components;
using Cloud5mins.ShortenerTools.TinyBlazorAdmin;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpClient<UrlManagerClient>(client => 
            {
                client.BaseAddress = new Uri("https+http://api");
            });

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddScoped<ITooltipService, TooltipService>();

// regiser fusion blazor service
// Community Licence for your personal use ONLY. Thank you Syncfusion for this generous offer.
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzYyMzI1QDMyMzAyZTMxMmUzMFY0cEZ3MVozdkwvekVhek8xTWdPMkg2NlhvdVFNR1lvZHdhQWJWUlNjZW89"); 
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();
app.MapDefaultEndpoints();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
