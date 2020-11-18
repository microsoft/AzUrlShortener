using System.Linq;
using System.Threading.Tasks;

namespace Cloud5mins.domain
{
    public static class Utility
    {
        //reshuffled for randomisation, same unique characters just jumbled up, use your own for security
        private const string ConversionCode = "aoq6lewdfit0nbvp3ukz8mc941gsj57r2hyx";
        private static readonly int Base = ConversionCode.Length;
        private const int MinVanityLength = 4;

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
    }
}