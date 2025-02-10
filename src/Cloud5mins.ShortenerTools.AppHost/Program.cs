
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var connectionString = builder.AddConnectionString("data-storage-connstr");
var customDomain = builder.AddParameter("CustomDomain");
var defaultRedirectUrl = builder.AddParameter("DefaultRedirectUrl");

var urlStorage = builder.AddAzureStorage("url-data");

if (builder.Environment.IsDevelopment())
{
    urlStorage.RunAsEmulator();
}

var strTables = urlStorage.AddTables("strTables");

var azFuncLight = builder.AddAzureFunctionsProject<Projects.Cloud5mins_ShortenerTools_FunctionsLight>("azfunc-light")
							.WithReference(strTables)
							.WaitFor(strTables)
							.WithExternalHttpEndpoints();

var manAPI = builder.AddProject<Projects.Cloud5mins_ShortenerTools_Api>("api")
						//.WithEnvironment("data-storage-connstr",connectionString)
						.WithReference(strTables)
						.WaitFor(strTables)
						.WithEnvironment("CustomDomain",customDomain)
						.WithEnvironment("DefaultRedirectUrl",defaultRedirectUrl)
						.WithExternalHttpEndpoints(); // only while debugging

// builder.AddProject<Projects.Cloud5mins_ShortenerTools_TinyBlazorAdmin>("admin")
// 		.WithExternalHttpEndpoints()
// 		.WithReference(manAPI);

builder.Build().Run();
