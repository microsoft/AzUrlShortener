using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cloud5mins.domain
{
    public static class Utility
    {
        //reshuffled for randomisation, same unique characters just jumbled up, use your own for security
        private const string ConversionCode = "aoq6lewdfit0nbvp3ukz8mc941gsj57r2hyx";
        private static readonly int Base = ConversionCode.Length;
        private const int MinVanityLength = 4;
        //private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        //private static readonly int Base = Alphabet.Length;

        public static async Task<string> GetValidEndUrl(string vanity, StorageTableHelper stgHelper)
        {
            if (string.IsNullOrEmpty(vanity))
            {
                var newKey = await stgHelper.GetNextTableId();
                string getCode() => Encode(newKey, MinVanityLength);
                return string.Join(string.Empty, getCode());
            }
            else
            {
                return string.Join(string.Empty, vanity);
            }
        }

        public static string Encode(int i, int minVanityLength)
        {
            if (i == 0)
                return ConversionCode[0].ToString();
            var s = string.Empty;
            while (i > 0)
            {
                s += ConversionCode[i % Base];
                i = i / Base;
                //if we setting a minimum length just extend the code accordingly
                if (minVanityLength > 0)
                {
                    while (s.Length < minVanityLength)
                    {
                        s += ConversionCode[s.Length % Base];
                    }
                }
            }

            return string.Join(string.Empty, s.Reverse());
        }

        public static string GetShortUrl(string host, string vanity)
        {
            return host + "/" + vanity;
        }

        public static IActionResult CatchUnauthorize(ClaimsPrincipal principal, ILogger log)
        {
            if (principal == null)
            {
                log.LogWarning("No principal.");
                return new UnauthorizedResult();
            }

            if (principal.Identity == null)
            {
                log.LogWarning("No identity.");
                return new UnauthorizedResult();
            }

            if (!principal.Identity.IsAuthenticated)
            {
                log.LogWarning("Request was not authenticated.");
                return new UnauthorizedResult();
            }

            if (principal.FindFirst(ClaimTypes.GivenName) is null)
            {
                log.LogError("Claim not Found");
                return new BadRequestObjectResult(new
                {
                    message = "Claim not Found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            return null;
        }
    }
}