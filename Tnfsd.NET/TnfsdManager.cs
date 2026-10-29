using System.ComponentModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace Tnfsd.NET
{
    public static class TnfsdManager
    {
        private const string VersionFileName = "tnfsd.version";
        private static readonly Regex VersionPattern = new(@"tnfsd version (\S+)");

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

        // Returns the installed version.
        public static async Task<string> InstallAsync(string folder)
        {
            var release = await TnfsdReleases.GetConfiguredReleaseAsync();

            Directory.CreateDirectory(folder);
            string tempZip = Path.Combine(Path.GetTempPath(), Path.GetFileName(new Uri(release.DownloadUrl).LocalPath));

            try
            {
                using (var response = await TnfsdReleases.Http.GetAsync(release.DownloadUrl, HttpCompletionOption.ResponseHeadersRead))
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

            string version = TnfsdReleases.Normalize(release.Version);
            await File.WriteAllTextAsync(Path.Combine(folder, VersionFileName), version);
            return version;
        }

        // Stops tnfsd if it is running so its exe can be replaced, then restarts it.
        public static async Task<string> UpdateAsync(string folder)
        {
            bool wasRunning = await Task.Run(() => TaskSchedulerManager.IsTaskRunning(TaskName));
            if (wasRunning)
            {
                await StopAsync();
                await WaitForExitAsync(GetExecutablePath(folder), TimeSpan.FromSeconds(10));
            }

            try
            {
                return await InstallAsync(folder);
            }
            finally
            {
                if (wasRunning)
                    await StartAsync();
            }
        }

        // Asks the exe first; builds without -v fall back to the file written at install.
        public static async Task<string?> GetInstalledVersionAsync(string folder)
        {
            string exePath = GetExecutablePath(folder);
            if (!File.Exists(exePath))
                return null;

            string? version = await QueryExecutableVersionAsync(exePath);
            if (version != null)
                return version;

            string versionFile = Path.Combine(folder, VersionFileName);
            if (!File.Exists(versionFile))
                return null;

            string recorded = (await File.ReadAllTextAsync(versionFile)).Trim();
            return recorded.Length > 0 ? recorded : null;
        }

        private static async Task<string?> QueryExecutableVersionAsync(string exePath)
        {
            var startInfo = new ProcessStartInfo(exePath, "-v")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = Path.GetDirectoryName(exePath)!
            };

            try
            {
                using var process = Process.Start(startInfo);
                if (process == null)
                    return null;

                var stdout = process.StandardOutput.ReadToEndAsync();
                var stderr = process.StandardError.ReadToEndAsync();

                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                try
                {
                    await process.WaitForExitAsync(timeout.Token);
                }
                catch (OperationCanceledException)
                {
                    process.Kill();
                    return null;
                }

                var match = VersionPattern.Match(await stdout + await stderr);
                return match.Success ? match.Groups[1].Value : null;
            }
            catch (Exception ex) when (ex is Win32Exception or InvalidOperationException)
            {
                return null;
            }
        }

        private static async Task WaitForExitAsync(string exePath, TimeSpan timeout)
        {
            string name = Path.GetFileNameWithoutExtension(exePath);
            var deadline = DateTime.UtcNow + timeout;
            while (Process.GetProcessesByName(name).Length > 0 && DateTime.UtcNow < deadline)
                await Task.Delay(250);
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
