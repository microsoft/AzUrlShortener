using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cloud5mins.domain
{
    public static class Utility
    {
        //reshuffled for randomisation, same unique characters just jumbled up, you can replace with your own version
        private const string ConversionCode = "FjTG0s5dgWkbLf_8etOZqMzNhmp7u6lUJoXIDiQB9-wRxCKyrPcv4En3Y21aASHV";
        private static readonly int Base = ConversionCode.Length;
        //sets the length of the unique code to add to vanity
        private const int MinVanityCodeLength = 5;

        public static async Task<string> GetValidEndUrl(string vanity, StorageTableHelper stgHelper)
        {
            if (string.IsNullOrEmpty(vanity))
            {
                var newKey = await stgHelper.GetNextTableId();
                string getCode() => Encode(newKey);
                if (await stgHelper.IfShortUrlEntityExistByVanity(getCode()))
                    return await GetValidEndUrl(vanity, stgHelper);
              
                return string.Join(string.Empty, getCode());
            }
            else
            {
                return string.Join(string.Empty, vanity);
            }
        }

        public static string Encode(int i)
        {
            if (i == 0)
                return ConversionCode[0].ToString();

            return GenerateUniqueRandomToken(i);
        }

        public static string GetShortUrl(string host, string vanity)
        {
            return host + "/" + vanity;
        }

        // generates a unique, random, and alphanumeric token for the use as a url 
        //(not entirely secure but not sequential so generally not guessable)
        public static string GenerateUniqueRandomToken(int uniqueId)
        {
            using (var generator = new RNGCryptoServiceProvider())
            {
                //minimum size I would suggest is 5, longer the better but we want short URLs!
                var bytes = new byte[MinVanityCodeLength];
                generator.GetBytes(bytes);
                var chars = bytes
                    .Select(b => ConversionCode[b % ConversionCode.Length]);
                var token = new string(chars.ToArray());
                var reversedToken = string.Join(string.Empty, token.Reverse());
                return uniqueId + reversedToken;
            }
        }

        public static IActionResult CheckAuthRole(ClaimsPrincipal principal, ILogger log, string requiredRole)
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

            bool isAppOnlyToken = IsAppOnlyToken(principal);

            if (!isAppOnlyToken)
            {
                log.LogWarning("Request for user was denied.");
                return new UnauthorizedResult();
            }

            const string ROLES_CLAIM = "roles";

            var allRoles = principal.Claims.Where(
                    c => c.Type == ROLES_CLAIM || c.Type == ClaimTypes.Role)
                    .SelectMany(c => c.Value.Split(' '));

            if (!allRoles.Any())
            {
                log.LogError("Role not found");

                return new BadRequestObjectResult(new
                {
                    message = "Role not found",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            if (!allRoles.Contains(requiredRole))
            {
                log.LogError("Required role missing");
                return new BadRequestObjectResult(new
                {
                    message = "Required role missing",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            return null;
        }

        public static bool IsAppOnlyToken(ClaimsPrincipal principal)
        {
            string oid = principal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            if (string.IsNullOrEmpty(oid))
            {
                oid = principal.FindFirst("oid")?.Value;
            }

            string sub = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sub))
            {
                sub = principal.FindFirst("sub")?.Value;
            }

            bool isAppOnlyToken = oid == sub;
            return isAppOnlyToken;
        }

        public static IActionResult CheckUserImpersonatedAuth(ClaimsPrincipal principal, ILogger log)
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