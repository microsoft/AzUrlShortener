using Cloud5mins.ShortenerTools.Core.Domain;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;



using Microsoft.Azure.Functions.Worker.Http;
using Cloud5mins.ShortenerTools.Core.Messages;
using System.IO;
using System.Text.Json;

namespace Cloud5mins.ShortenerTools
{
    public static class InputValidator
    {

        public static async Task<ShortRequest> ValidateShortRequest(HttpRequestData req)
        {
            ShortRequest input;
            if (req == null)
            {
                return null;
            }

            using var reader = new StreamReader(req.Body);
            var strBody = await reader.ReadToEndAsync();
            input = JsonSerializer.Deserialize<ShortRequest>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (input == null)
            {
                return null;
            }
            return input;
        }


        public static async Task<ShortUrlEntity> ValidateShortUrlEntity(HttpRequestData req)
        {
            ShortUrlEntity input;
            if (req == null)
            {
                return null;
            }

            using var reader = new StreamReader(req.Body);
            var strBody = await reader.ReadToEndAsync();
            input = JsonSerializer.Deserialize<ShortUrlEntity>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (input == null)
            {
                return null;
            }
            return input;
        }

        public static async Task<UrlClickStatsRequest> ValidateUrlClickStatsRequest(HttpRequestData req)
        {
            UrlClickStatsRequest input;
            if (req == null)
            {
                return null;
            }

            using var reader = new StreamReader(req.Body);
            var strBody = await reader.ReadToEndAsync();
            input = JsonSerializer.Deserialize<UrlClickStatsRequest>(strBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (input == null)
            {
                return null;
            }
            return input;
        }



    }
}