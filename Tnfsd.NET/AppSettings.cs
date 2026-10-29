using Microsoft.Extensions.Configuration;

namespace Tnfsd.NET
{
    public static class AppSettings
    {
        private static IConfigurationRoot _config;

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string TNFSDRepository => Get("TNFSDRepository", "FujiNetWIFI/tnfsd");

        // "latest" or a release tag such as "v26.1015.1".
        public static string TNFSDVersion => Get("TNFSDVersion", "latest");

        public static string TNFSDAsset => Get("TNFSDAsset", "_win-x64.zip");

        // Older settings files pin a full download URL; it takes precedence when present.
        public static string? TNFSDownloadURL => _config["TNFSDownloadURL"];

        private static string Get(string key, string defaultValue)
        {
            string? value = _config[key];
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
        }
    }
}
