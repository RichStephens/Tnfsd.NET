using System.Text.Json;

namespace Tnfsd.NET
{
    public record TnfsdRelease(string Version, string DownloadUrl);

    public static class TnfsdReleases
    {
        public static readonly HttpClient Http = CreateClient();

        public static async Task<TnfsdRelease> GetConfiguredReleaseAsync()
        {
            string? pinnedUrl = AppSettings.TNFSDownloadURL;
            if (!string.IsNullOrWhiteSpace(pinnedUrl))
                return new TnfsdRelease(TagFromDownloadUrl(pinnedUrl) ?? "unknown", pinnedUrl);

            string repo = AppSettings.TNFSDRepository;
            string version = AppSettings.TNFSDVersion;
            string path = version.Equals("latest", StringComparison.OrdinalIgnoreCase)
                ? "releases/latest"
                : $"releases/tags/{Uri.EscapeDataString(version)}";

            string json = await Http.GetStringAsync($"https://api.github.com/repos/{repo}/{path}");
            using var doc = JsonDocument.Parse(json);
            string tag = doc.RootElement.GetProperty("tag_name").GetString() ?? version;

            foreach (var asset in doc.RootElement.GetProperty("assets").EnumerateArray())
            {
                string name = asset.GetProperty("name").GetString() ?? "";
                if (name.EndsWith(AppSettings.TNFSDAsset, StringComparison.OrdinalIgnoreCase))
                    return new TnfsdRelease(tag, asset.GetProperty("browser_download_url").GetString()!);
            }

            throw new InvalidOperationException($"Release {tag} of {repo} has no file ending in \"{AppSettings.TNFSDAsset}\".");
        }

        public static string Normalize(string version)
        {
            return version.Trim().TrimStart('v', 'V');
        }

        public static bool IsNewer(string candidate, string installed)
        {
            string a = Normalize(candidate), b = Normalize(installed);
            if (Version.TryParse(a, out var va) && Version.TryParse(b, out var vb))
                return va > vb;
            return !a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }

        // GitHub download URLs look like .../releases/download/<tag>/<file>.
        private static string? TagFromDownloadUrl(string url)
        {
            string[] segments = new Uri(url).AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            int i = Array.IndexOf(segments, "download");
            return i >= 0 && i + 1 < segments.Length ? segments[i + 1] : null;
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Tnfsd.NET");
            return client;
        }
    }
}
