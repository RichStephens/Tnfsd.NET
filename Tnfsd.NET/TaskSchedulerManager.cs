using Microsoft.Win32.TaskScheduler;
using System.Diagnostics;

namespace Tnfsd.NET
{
    public static class TaskSchedulerManager
    {
        public static string TaskName = "TNFSD";
        public static string ExecutableName = "tnfsd.exe";

        /// <summary>
        /// Creates or updates a task to start TNFSD at system boot under LocalSystem.
        /// </summary>
        public static void CreateOrUpdateTask(string taskName, string exePath, string arguments = "")
        {
            if (string.IsNullOrWhiteSpace(taskName))
                taskName = TaskName;

            if (!File.Exists(exePath))
                throw new FileNotFoundException($"Executable not found: {exePath}");

            using (TaskService ts = new TaskService())
            {
                // Delete existing task if present
                var existing = ts.GetTask(taskName);
                if (existing != null)
                    ts.RootFolder.DeleteTask(taskName);

                // Define new task
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "TNFSD network file server";

                // Run as LocalSystem
                td.Principal.UserId = "NT AUTHORITY\\SYSTEM";
                td.Principal.LogonType = TaskLogonType.ServiceAccount;
                td.Principal.RunLevel = TaskRunLevel.Highest;

                // Trigger: run at system startup
                td.Triggers.Add(new BootTrigger());

                // Settings suitable for a long-running service-like task
                td.Settings.Enabled = true;
                td.Settings.StartWhenAvailable = true; // try to start if missed
                td.Settings.AllowDemandStart = true;
                td.Settings.RunOnlyIfNetworkAvailable = false;
                td.Settings.DisallowStartIfOnBatteries = false;
                td.Settings.ExecutionTimeLimit = TimeSpan.Zero; // no time limit (run indefinitely)
                td.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew; // single instance behavior

                // Quote argument (if present)
                string quotedArgs = string.IsNullOrWhiteSpace(arguments) ? "" : $"\"{arguments.Trim()}\"";

                td.Actions.Add(new ExecAction(exePath, quotedArgs, Path.GetDirectoryName(exePath)));

                // Register the task as LocalSystem
                ts.RootFolder.RegisterTaskDefinition(
                    taskName,
                    td,
                    TaskCreation.CreateOrUpdate,
                    "NT AUTHORITY\\SYSTEM",
                    null,
                    TaskLogonType.ServiceAccount);
            }
        }


        public static void DeleteTask(string taskName)
        {
            if (IsTaskRunning(taskName))
            {
                StopTask(taskName);
            }

            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                if (task != null)
                    ts.RootFolder.DeleteTask(taskName);
            }
        }

        public static bool TaskExists(string taskName)
        {
            using (TaskService ts = new TaskService())
            {
                return ts.GetTask(taskName) != null;
            }
        }

        public static bool IsTaskRunning(string taskName)
        {
            using var ts = new TaskService();

            var t = ts.GetTask(taskName) ?? ts.FindTask(taskName, true);
            if (t == null)
                return false;

            if (t.State == TaskState.Running)
                return true;

            // Most reliable for this library: ask the task for its running instances
            try
            {
                using var instances = t.GetInstances();
                if (instances != null && instances.Count > 0)
                    return true;
            }
            catch
            {
                // Optional: swallow/log. Access/security context can affect visibility.
                // (Same caveat exists for GetRunningTasks.) :contentReference[oaicite:2]{index=2}
            }

            // Fallback: detect the EXE process from the first ExecAction
            var execPath = t.Definition?.Actions?.OfType<ExecAction>().FirstOrDefault()?.Path;
            if (!string.IsNullOrWhiteSpace(execPath))
            {
                var exeName = Path.GetFileNameWithoutExtension(execPath);
                if (!string.IsNullOrWhiteSpace(exeName) &&
                    Process.GetProcessesByName(exeName).Length > 0)
                    return true;
            }

            return false;
        }

        public static void StartTask(string taskName)
        {
            if (TaskExists(taskName))
            {
                using (TaskService ts = new TaskService())
                {
                    var task = ts.GetTask(taskName);
                    task?.Run();
                }
            }
        }

        public static void StopTask(string taskName)
        {
            if (TaskExists(taskName))
            {
                using (TaskService ts = new TaskService())
                {
                    var task = ts.GetTask(taskName);
                    task?.Stop();
                }
            }
        }

        /// <summary>
        /// Returns the executable folder and share folder (argument) used by the TNFSD task.
        /// </summary>
        public static TaskProperties GetTaskProperties(string taskName)
        {
            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                if (task == null || task.Definition.Actions.Count == 0)
                    return null;

                var action = task.Definition.Actions[0] as ExecAction;
                if (action == null)
                    return null;

                string exeFolder = Path.GetDirectoryName(action.Path);
                string shareFolder = null;

                if (!string.IsNullOrWhiteSpace(action.Arguments))
                {
                    // Simple parse: first argument is the share folder path
                    shareFolder = action.Arguments.Trim().Trim('"');
                }

                return new TaskProperties
                {
                    ExecutableFolder = exeFolder,
                    ShareFolder = shareFolder
                };
            }
        }
    }

}
