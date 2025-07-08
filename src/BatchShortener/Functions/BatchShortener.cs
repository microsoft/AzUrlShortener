using System.Net.Http.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Cloud5mins.ShortenerTools.BatchShortener.Models;
using Cloud5mins.ShortenerTools.Core.Messages;

namespace Cloud5mins.ShortenerTools.BatchShortener.Functions
{
    public class BatchShortener
    {
        private readonly ILogger _logger;
        private readonly CosmosClient _cosmosClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public BatchShortener(
            ILogger<BatchShortener> logger,
            CosmosClient cosmosClient,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _cosmosClient = cosmosClient;
            _httpClientFactory = httpClientFactory;
        }

        [Function("BatchShortener")]
        public async Task Run([TimerTrigger("%SCHEDULE%")] TimerInfo timer)
        {
            try
            {
                string dbName = Environment.GetEnvironmentVariable("CosmosDbName") ?? throw new InvalidOperationException("CosmosDbName environment variable is not set");
                string containerName = Environment.GetEnvironmentVariable("CosmosDbContainerName") ?? throw new InvalidOperationException("CosmosDbContainerName environment variable is not set");
                string apiUrl = Environment.GetEnvironmentVariable("ApiUrl") ?? throw new InvalidOperationException("ApiUrl environment variable is not set");

                var container = _cosmosClient.GetContainer(dbName, containerName);

                // Query for items that will/are published and are missing a Share_Url
                var query = "SELECT * FROM c WHERE (c.Status = 'Ready to Publish' OR c.Status = 'Published') AND (NOT IS_DEFINED(c.Share_Url) OR c.Share_Url = '' OR c.Share_Url = null)";
                var queryDefinition = new QueryDefinition(query);
                using var resultsIterator = container.GetItemQueryIterator<CosmosDBEntry>(queryDefinition);

                int updatedCount = 0;

                while (resultsIterator.HasMoreResults)
                {
                    var response = await resultsIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        if (string.IsNullOrEmpty(item.Article_GUID))
                        {
                            _logger.LogWarning($"Item '{item.id}' has no Article_GUID to use for a shortened URL. Skipping.");
                            continue;
                        }

                        var url = $"https://thenimblenerd.com/guid/{item.Article_GUID}";

                        _logger.LogInformation($"Processing item '{item.id}' with the URL '{url}'");

                        try
                        {
                            var httpClient = _httpClientFactory.CreateClient();

                            var shortRequest = new ShortRequest
                            {
                                Url = url,
                                Title = item.Article_Title ?? $"Auto-generated URL for {item.Article_GUID}"
                            };
                            var apiResponse = await httpClient.PostAsJsonAsync(apiUrl, shortRequest);
                            apiResponse.EnsureSuccessStatusCode();

                            // Get shortened URL
                            var shortResponse = await apiResponse.Content.ReadFromJsonAsync<ShortResponse>();

                            if (shortResponse != null && !string.IsNullOrEmpty(shortResponse.ShortUrl))
                            {
                                // Update the Cosmos DB Short_URL field only
                                item.Share_Url = shortResponse.ShortUrl;
                                var patchOperations = new List<PatchOperation>
                                {
                                    PatchOperation.Add("/Share_Url", item.Share_Url)
                                };
                                await container.PatchItemAsync<CosmosDBEntry>(
                                    id: item.id,
                                    partitionKey: new PartitionKey(item.id),
                                    patchOperations: patchOperations
                                );
                                _logger.LogInformation($"Updated item '{item.id}' with Share_Url '{item.Share_Url}'");
                                updatedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error updating item {item.id}: {ex.Message}");
                        }
                    }
                }

                _logger.LogInformation($"Created {updatedCount} shortened URLs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in BatchShortener function: {ex.Message}");
            }
        }
    }
}