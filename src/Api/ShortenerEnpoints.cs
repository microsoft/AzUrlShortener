using Azure.Data.Tables;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using Cloud5mins.ShortenerTools.Core.Service;
using Cloud5mins.ShortenerTools.Core.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

public static class ShortenerEnpoints
{
    public static void MapShortenerEnpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("api")
                .WithOpenApi();

        // GETS

        endpoints.MapGet("/", GetWelcomeMessage)
            .WithDescription("Welcome to Cloud5mins URL Shortener API");

        endpoints.MapGet("/UrlList", UrlList)
            .WithDescription("List all Urls")
            .WithDisplayName("Url List");


        // POSTS

        endpoints.MapPost("/UrlCreate", UrlCreate)
            .WithDescription("Create a new Short URL")
            .WithDisplayName("Url Create");

        endpoints.MapPost("/UrlUpdate", UrlUpdate)
            .WithDescription("Update a Url")
            .WithDisplayName("Url Update");

        endpoints.MapPost("/UrlArchive", UrlArchive)
            .WithDescription("Archive a Url")
            .WithDisplayName("Url Archive");

        endpoints.MapPost("/UrlClickStatsByDay", UrlClickStatsByDay)
            .WithDescription("Provide Click Statistics by Day")
            .WithDisplayName("Url Click Statistics By Day");

    }

    static private string GetWelcomeMessage()
    {
        return "Welcome to Cloud5mins URL Shortener API";
    }

    static private async Task<Results<
                                Created<ShortResponse>,
                                BadRequest<DetailedBadRequest>,
                                NotFound<DetailedBadRequest>,
                                Conflict<DetailedBadRequest>,
                                InternalServerError<DetailedBadRequest>
                                >> UrlCreate(ShortRequest request,
                                                TableServiceClient tblClient,
                                                HttpContext context,
                                                ILogger logger)
    {
        try
        {
            var urlServices = new UrlServices(logger, new AzStrorageTablesService(tblClient));
            var host = GetHost(context);
            ShortResponse result = await urlServices.Create(request, host);
            return TypedResults.Created($"/api/UrlCreate/{result.ShortUrl}", result);
        }
        catch (ShortenerToolException ex)
        {
            switch (ex.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return TypedResults.BadRequest<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
                case HttpStatusCode.NotFound:
                    return TypedResults.NotFound<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
                case HttpStatusCode.Conflict:
                    return TypedResults.Conflict<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
                default:
                    return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error was encountered.");
            return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
        }
    }

    static private async Task<Results<
                                    Ok,
                                    InternalServerError<DetailedBadRequest>>>
                                    UrlArchive(ShortUrlEntity shortUrl,
                                                TableServiceClient tblClient,
                                                ILogger logger)
    {
        try
        {
            var urlServices = new UrlServices(logger, new AzStrorageTablesService(tblClient));
            var result = await urlServices.Archive(shortUrl);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
        }
    }

    static private async Task<Results<
                                    Ok<ShortUrlEntity>,
                                    InternalServerError<DetailedBadRequest>>>
                                    UrlUpdate(ShortUrlEntity shortUrl,
                                                TableServiceClient tblClient,
                                                HttpContext context,
                                                ILogger logger)
    {
        try
        {
            var urlServices = new UrlServices(logger, new AzStrorageTablesService(tblClient));
            var host = GetHost(context);
            var result = await urlServices.Update(shortUrl, host);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
        }
    }



    static private async Task<Results<
                                    Ok<ClickDateList>,
                                    InternalServerError<DetailedBadRequest>>>
                                    UrlClickStatsByDay(UrlClickStatsRequest statsRequest,
                                                TableServiceClient tblClient,
                                                HttpContext context,
                                                ILogger logger)
    {
        try
        {
            var urlServices = new UrlServices(logger, new AzStrorageTablesService(tblClient));
            var host = GetHost(context);
            var result = await urlServices.ClickStatsByDay(statsRequest, host);
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
        }
    }


    static private async Task<Results<
                                Ok<ListResponse>,
                                InternalServerError<DetailedBadRequest>>>
                                UrlList(TableServiceClient tblClient,
                                        HttpContext context,
                                        ILogger logger)
    {
        try
        {
            var urlServices = new UrlServices(logger, new AzStrorageTablesService(tblClient));
            var host = GetHost(context);
            ListResponse Urls = await urlServices.List(host);
            return TypedResults.Ok(Urls);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error was encountered.");
            return TypedResults.InternalServerError<DetailedBadRequest>(new DetailedBadRequest { Message = ex.Message });
        }
    }

    private static string GetHost(HttpContext context)
    {
        string? customDomain = Environment.GetEnvironmentVariable("CustomDomain");
        var host = string.IsNullOrEmpty(customDomain) ? context.Request.Host.Value : customDomain;
        return host ?? string.Empty;
    }

}

