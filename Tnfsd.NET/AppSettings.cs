using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tnfsd.NET
{
    public static class AppSettings
    {
        private static IConfigurationRoot _config;

        static AppSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string TNFSDownloadURL => _config["TNFSDownloadURL"];
    }
}