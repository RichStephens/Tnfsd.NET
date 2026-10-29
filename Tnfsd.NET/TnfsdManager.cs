using System.IO.Compression;

namespace Tnfsd.NET
{
    public static class TnfsdManager
    {
        private static string TaskName => TaskSchedulerManager.TaskName;

        public static Task<TnfsdStatus> GetStatusAsync() => Task.Run(() =>
        {
            bool exists = TaskSchedulerManager.TaskExists(TaskName);
            var props = exists ? TaskSchedulerManager.GetTaskProperties(TaskName) : null;

            return new TnfsdStatus(
                exists,
                exists && TaskSchedulerManager.IsTaskRunning(TaskName),
                FirewallManager.FirewallExceptionExists(TaskName),
                props?.ExecutableFolder != null && ExecutableExists(props.ExecutableFolder),
                props?.ExecutableFolder,
                props?.ShareFolder);
        });

        public static string GetExecutablePath(string folder)
        {
            return Path.Combine(folder, TaskSchedulerManager.ExecutableName);
        }

        public static bool ExecutableExists(string folder)
        {
            return File.Exists(GetExecutablePath(folder));
        }

        public static bool IsFolderEmpty(string folder)
        {
            return !Directory.Exists(folder) || !Directory.EnumerateFileSystemEntries(folder).Any();
        }

        public static async Task InstallAsync(string folder)
        {
            string? url = AppSettings.TNFSDownloadURL;
            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("TNFSDownloadURL is missing from appsettings.json.");

            Directory.CreateDirectory(folder);
            string tempZip = Path.Combine(Path.GetTempPath(), Path.GetFileName(new Uri(url).LocalPath));

            try
            {
                using (var httpClient = new HttpClient())
                using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    await using var stream = await response.Content.ReadAsStreamAsync();
                    await using var fileStream = new FileStream(tempZip, FileMode.Create, FileAccess.Write, FileShare.None);
                    await stream.CopyToAsync(fileStream);
                }

                await Task.Run(() => ZipFile.ExtractToDirectory(tempZip, folder, overwriteFiles: true));
            }
            finally
            {
                File.Delete(tempZip);
            }
        }

        public static Task CreateAsync(string executableFolder, string shareFolder, bool addFirewallRule) => Task.Run(() =>
        {
            string exePath = GetExecutablePath(executableFolder);

            TaskSchedulerManager.CreateOrUpdateTask(TaskName, exePath, shareFolder);

            if (addFirewallRule)
                FirewallManager.AddFirewallException(exePath, TaskName);

            if (!TaskSchedulerManager.IsTaskRunning(TaskName))
                TaskSchedulerManager.StartTask(TaskName);
        });

        // Returns whether a firewall rule was removed.
        public static Task<bool> DeleteAsync() => Task.Run(() =>
        {
            TaskSchedulerManager.DeleteTask(TaskName);
            return FirewallManager.RemoveFirewallException(TaskName);
        });

        public static Task StartAsync() => Task.Run(() => TaskSchedulerManager.StartTask(TaskName));

        public static Task StopAsync() => Task.Run(() => TaskSchedulerManager.StopTask(TaskName));
    }
}
