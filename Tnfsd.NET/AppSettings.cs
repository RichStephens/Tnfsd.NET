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

        public static string? TNFSDownloadURL => _config["TNFSDownloadURL"];
    }
}
