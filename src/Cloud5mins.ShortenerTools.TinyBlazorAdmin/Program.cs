using Microsoft.FluentUI.AspNetCore.Components;
using Cloud5mins.ShortenerTools.TinyBlazorAdmin.Components;
using Cloud5mins.ShortenerTools.TinyBlazorAdmin;

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
