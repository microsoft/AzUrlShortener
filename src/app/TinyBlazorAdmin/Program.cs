using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyBlazorAdmin.Data;
using Syncfusion.Blazor;
using Microsoft.Azure.Functions.Authentication.WebAssembly;

namespace TinyBlazorAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddStaticWebAppsAuthentication();

            //Add SycnFusion Controls
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzgwNjM5QDMxMzgyZTMzMmUzMGhPbXloTFpmTVFQTEgrMUZ2NjZONkFEZmhLOG16RkhYSkYyZmZpOHRVUkU9"); 
            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }
    }
}
