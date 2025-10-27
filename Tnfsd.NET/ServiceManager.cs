using System;
using System.ServiceProcess;
using System.Diagnostics;
using System.IO;

namespace Tnfsd.NET
{
    public static class ServiceManager
    {
        public static string ServiceName = "tnfsd";
        public static string ServiceExecutable = ServiceName + ".exe";

        public static void CreateOrUpdateService(string serviceName, string description, string executablePath, string sharePath, string userId = null, string password = null)
        {
            if (ServiceExists(serviceName))
            {
                UninstallService(serviceName);
            }

            if (string.IsNullOrWhiteSpace(executablePath) || !File.Exists(executablePath))
                throw new FileNotFoundException($"Executable not found: {executablePath}");

            string args = $"create {serviceName} binPath= \"{executablePath}\" \"{sharePath}\" start= auto DisplayName= \"{description}\"";

            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(password))
            {
                args += $" obj= \"{userId}\" password= \"{password}\"";
            }

            RunScCommand(args);
        }

        public static void UninstallService(string serviceName)
        {
            if (!ServiceExists(serviceName))
                return;

            StopService(serviceName);
            RunScCommand($"delete {serviceName}");
        }

        public static bool ServiceExists(string serviceName)
        {
            foreach (ServiceController sc in ServiceController.GetServices())
            {
                if (sc.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool ServiceIsRunning(string serviceName)
        {
            if (!ServiceExists(serviceName)) return false;
            ServiceController sc = new ServiceController(serviceName);
            return sc.Status == ServiceControllerStatus.Running;
        }

        public static void StartService(string serviceName)
        {
            if (!ServiceExists(serviceName)) return;

            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status != ServiceControllerStatus.Running)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            }
        }

        public static void StopService(string serviceName)
        {
            if (!ServiceExists(serviceName)) return;

            ServiceController sc = new ServiceController(serviceName);
            if (sc.Status != ServiceControllerStatus.Stopped)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            }
        }

        public static ServiceProperties GetServiceProperties(string serviceName)
        {
            ServiceProperties props = new ServiceProperties();

            if (ServiceExists(serviceName))
            {
                using (ServiceController _ = new ServiceController(serviceName))
                {
                    props.ExecutableFolder = GetServiceExecutablePath(serviceName);
                    props.UserId = GetServiceUser(serviceName);
                }
            }

            return props;
        }

        private static string GetServiceExecutablePath(string serviceName)
        {
            try
            {
                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey($"SYSTEM\\CurrentControlSet\\Services\\{serviceName}"))
                {
                    if (key != null)
                    {
                        object obj = key.GetValue("ImagePath");
                        if (obj != null)
                        {
                            string path = Environment.ExpandEnvironmentVariables(obj.ToString());
                            path = path.Trim('\"');
                            return path;
                        }
                    }
                }
            }
            catch { }
            return string.Empty;
        }

        private static string GetServiceUser(string serviceName)
        {
            try
            {
                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey($"SYSTEM\\CurrentControlSet\\Services\\{serviceName}"))
                {
                    if (key != null)
                    {
                        object obj = key.GetValue("ObjectName");
                        if (obj != null)
                        {
                            return obj.ToString();
                        }
                    }
                }
            }
            catch { }
            return Environment.UserName;
        }

        private static void RunScCommand(string args)
        {
            var process = new Process();
            process.StartInfo.FileName = "sc.exe";
            process.StartInfo.Arguments = args;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
