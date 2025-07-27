using Microsoft.Win32.TaskScheduler;
using System.Reflection.Metadata.Ecma335;

namespace Tnfsd.NET
{
    public static class TaskSchedulerManager
    {
        public static void CreateOrUpdateTask(string taskName, string description, string executablePath, string arguments = "")
        {
            using TaskService ts = new TaskService();

            // ✅ Only delete if task exists
            if (TaskExists(taskName))
            {
                ts.RootFolder.DeleteTask(taskName, false);
            }

            // 🆕 Create a new task definition
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = description;

            // 🔁 Run when the system boots
            td.Triggers.Add(new BootTrigger());

            // ⚙️ Set the action
            td.Actions.Add(new ExecAction(executablePath, arguments, null));

            // 👤 Run with highest privileges, even if user is not logged on
            td.Principal.UserId = Environment.UserName;
            td.Principal.LogonType = TaskLogonType.S4U; // Run without storing password
            td.Principal.RunLevel = TaskRunLevel.Highest;

            // ✅ Register task
            ts.RootFolder.RegisterTaskDefinition(taskName, td);
        }

        public static void DeleteTask(string taskName)
        {
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task.IsActive && task.State == TaskState.Running)
                {
                    // Stop the task
                    task.Stop();
                }
                
                ts.RootFolder.DeleteTask(taskName, false);
            }
        }

        public static bool TaskExists(string taskName)
        {
            using TaskService ts = new TaskService();
            return ts.GetTask(taskName) != null;
        }

        public static bool TaskIsRunning(string taskName)
        {
            bool isRunning = false;
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task != null)
                {
                    // Check if the task is currently running
                    if (task.IsActive && task.State == TaskState.Running)
                    {
                        isRunning = true;
                    }
                }
            }
            return isRunning;
        }

        public static void StartTask(string taskName)
        {
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task.IsActive && task.State != TaskState.Running) 
                {
                    // Start the task
                    task.Run();
                }
            }
        }

        public static void StopTask(string taskName)
        {
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task.IsActive && task.State == TaskState.Running)
                {
                    // Stop the task
                    task.Stop();
                }
            }
        }

        public static string GetTaskExecPath(string taskName)
        {
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task != null)
                {
                    foreach (Microsoft.Win32.TaskScheduler.Action action in task.Definition.Actions)
                    {
                        if (action.ActionType == TaskActionType.Execute)
                        {
                            Microsoft.Win32.TaskScheduler.ExecAction execAction = (Microsoft.Win32.TaskScheduler.ExecAction)action;
                            return execAction.Path; // Return the executable path
                        }
                    }
                }
            }

            return string.Empty; // Task or executable action not found
        }

        public static string GetTaskArguments(string taskName)
        {
            using TaskService ts = new TaskService();

            if (TaskExists(taskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                if (task != null)
                {
                    foreach (Microsoft.Win32.TaskScheduler.Action action in task.Definition.Actions)
                    {
                        if (action.ActionType == TaskActionType.Execute)
                        {
                            Microsoft.Win32.TaskScheduler.ExecAction execAction = (Microsoft.Win32.TaskScheduler.ExecAction)action;
                            return execAction.Arguments; // Return the arguments
                        }
                    }
                }
            }

            return string.Empty; // Task or executable action not found
        }
    }
}

