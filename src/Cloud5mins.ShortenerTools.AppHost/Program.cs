var builder = DistributedApplication.CreateBuilder(args);

var urlData = builder.AddAzureStorage("urlData");

var azFuncLight = builder.AddAzureFunctionsProject<Projects.Cloud5mins_ShortenerTools_FunctionsLight>("azFuncLight")
						 	.WithHostStorage(urlData)
							.WithExternalHttpEndpoints();

builder.Build().Run();
