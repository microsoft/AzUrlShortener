using System.ComponentModel;
using System.Globalization;
using Cloud5mins.ShortenerTools.Core.Domain;
using Cloud5mins.ShortenerTools.Core.Messages;
using CsvHelper.Configuration.Attributes;
using ModelContextProtocol.Server;

namespace Cloud5mins.ShortenerTools.Tools;
[McpServerToolType]
public class UrlShortenerTool
{
    private readonly UrlManagerClient _urlManager;

    public UrlShortenerTool(UrlManagerClient urlManager)
    {
        _urlManager = urlManager;
    }

    [McpServerTool, Description("Shortens the given URL.")]
    public string ShortenUrl(string longUrl, string? vanity)
    {
        ShortRequest urlRequest = new ShortRequest()
        {
            Url = longUrl,
            Vanity = vanity ?? string.Empty
        };

        var response = _urlManager.UrlCreate(urlRequest).Result;
        if (response.Item1)
        {
            return "Short URL created successfully";
        }
        else
        {
            return $"Failed to create short URL: {response.Item2}";
        }
    }

    [McpServerTool, Description("Provide a list of all short URLs.")]
    public List<ShortUrlEntity> ListUrl()
    {
        var urlList = _urlManager.GetUrls().Result.ToList<ShortUrlEntity>();
        return urlList;
    }

    [McpServerTool(Name="UrlClickStatsByDay"), Description("Provide the clicks statistics for all short URLs, or for a specific URL when the vanity is provided.")]
    public async Task<List<ClickDate>?> UrlClickStatsByDay(string? vanity)
    {
        try
        {
            var response = await _urlManager.UrlClickStatsByDay(new UrlClickStatsRequest(vanity));

            if (response != null)
            {
                {
                    return response.Items;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }

    [McpServerTool, Description("Archive a short URL.")]
    public string ArchiveUrl(string vanity)
    {
        var urlEntity = new ShortUrlEntity{
            RowKey = vanity
        };

        var result = _urlManager.UrlArchive(urlEntity).Result;
        return result ? "Short URL archived successfully" : "Failed to archive short URL";
    }

    [McpServerTool, Description("Update basic properties of a short URL. Scheduled not supported.")]
    public string UpdateUrl(string vanity, string newLongUrl, string? title)
    {
        var urlEntity = new ShortUrlEntity
        {
            RowKey = vanity,
            Url = newLongUrl,
            Title = title ?? string.Empty
        };

        var updatedUrl = _urlManager.UrlUpdate(urlEntity).Result;
        return updatedUrl != null ? "Short URL updated successfully" : "Failed to update short URL";
    }

    
    [McpServerTool, Description("Import short URLs from a CSV file.")]
    public async Task<string> ImportUrlDataAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return "Error: File not found";
            }
            
            UrlDetails urlData = await Utility.ExtractUrlsDataFromCSV(filePath);
            var result = await _urlManager.ImportUrlDataAsync(urlData);
            if (result)
            {
                return "URLs imported successfully";
            }
            else
            {
                return $"Failed to import URLs";
            }
        }
        catch (Exception ex)
        {
            return $"Error importing URLs: {ex.Message}";
        }
    }

    [McpServerTool, Description("Import click statistics from a CSV file.")]
    public async Task<string> ImportClickStatsAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return "Error: File not found";
            }

            var clickStats = await Utility.ExtractClickStatsFromCSV(filePath);
            var result = await _urlManager.ImportClickStatsAsync(clickStats);
            if (result)
            {
                return "Click statistics imported successfully";
            }
            else
            {
                return "Failed to import click statistics";
            }
        }
        catch (Exception ex)
        {
            return $"Error importing click statistics: {ex.Message}";
        }
    }
}