using System;
using System.IO;
using Microsoft.Win32.TaskScheduler;

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
            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                return task != null && task.State == TaskState.Running;
            }
        }

        public static void StartTask(string taskName)
        {
            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                task?.Run();
            }
        }

        public static void StopTask(string taskName)
        {
            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                task?.Stop();
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
