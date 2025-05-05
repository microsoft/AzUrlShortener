
using Microsoft.Extensions.Hosting;
using Azure.Storage.Blobs.Models;

var builder = DistributedApplication.CreateBuilder(args);

var customDomain = builder.AddParameter("CustomDomain");
var defaultRedirectUrl = builder.AddParameter("DefaultRedirectUrl");

var urlStorage = builder.AddAzureStorage("url-data");

if (builder.Environment.IsDevelopment())
{
	urlStorage.RunAsEmulator();
}

var strTables = urlStorage.AddTables("strTables");
var strBlobs = urlStorage.AddBlobs("strBlobs");

var azFuncLight = builder.AddAzureFunctionsProject<Projects.Cloud5mins_ShortenerTools_FunctionsLight>("azfunc-light")
							.WithReference(strTables)
							.WaitFor(strTables)
							.WithEnvironment("DefaultRedirectUrl", defaultRedirectUrl)
							.WithExternalHttpEndpoints();

var manAPI = builder.AddProject<Projects.Cloud5mins_ShortenerTools_Api>("api")
						.WithReference(strTables)
						.WaitFor(strTables)
						.WithReference(strBlobs)
						.WaitFor(strBlobs)
						.WithEnvironment("CustomDomain", customDomain)
						.WithEnvironment("DefaultRedirectUrl", defaultRedirectUrl);
//.WithExternalHttpEndpoints(); // If you want to access the API directly

builder.AddProject<Projects.Cloud5mins_ShortenerTools_TinyBlazorAdmin>("admin")
		.WithExternalHttpEndpoints()
		.WithReference(manAPI);

builder.Build().Run();
