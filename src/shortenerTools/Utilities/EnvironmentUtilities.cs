using System;
using System.Diagnostics.CodeAnalysis;

namespace shortenerTools.Utilities
{
    [ExcludeFromCodeCoverage]
    public static class EnvironmentUtilities
    {
        public static string GetVariable(string settingName) => Environment.GetEnvironmentVariable(settingName, EnvironmentVariableTarget.Process);
        public static string AspNetCoreEnvironment() => GetVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
        public static bool IsLocal()
        {
            var aspNetCoreEnvironment = AspNetCoreEnvironment();
            return string.IsNullOrWhiteSpace(aspNetCoreEnvironment) || aspNetCoreEnvironment.Equals("local", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}