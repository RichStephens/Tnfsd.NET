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
                td.Principal.UserId = "SYSTEM";
                td.Principal.RunLevel = TaskRunLevel.Highest;

                td.Triggers.Add(new BootTrigger()); // run at system startup

                // Quote both exe path and argument if needed
                string quotedArgs = string.IsNullOrWhiteSpace(arguments) ? "" : $"\"{arguments.Trim()}\"";

                td.Actions.Add(new ExecAction($"{exePath}", quotedArgs, Path.GetDirectoryName(exePath)));

                // Register the task
                ts.RootFolder.RegisterTaskDefinition(taskName, td,
                    TaskCreation.CreateOrUpdate, null, null,
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

            // Always re-fetch to avoid stale state
            var t = ts.GetTask(taskName) ?? ts.FindTask(taskName, true);
            if (t == null)
                return false;

            // 1) Scheduler reports Running
            if (t.State == TaskState.Running)
                return true;

            // 2) Check if this task appears in the system's active task list
            if (ts.GetRunningTasks(true)
                  .Any(rt => string.Equals(rt.Path, t.Path, System.StringComparison.OrdinalIgnoreCase)))
                return true;

            // 3) Fallback: detect the actual EXE process from the ExecAction
            var execPath = (t.Definition.Actions.FirstOrDefault() as ExecAction)?.Path;
            if (!string.IsNullOrWhiteSpace(execPath))
            {
                var exeName = Path.GetFileNameWithoutExtension(execPath);
                if (Process.GetProcessesByName(exeName).Any())
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
