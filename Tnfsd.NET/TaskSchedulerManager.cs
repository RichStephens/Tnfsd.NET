using Microsoft.Win32.TaskScheduler;

namespace Tnfsd.NET
{
    public static class TaskSchedulerManager
    {
        public static string TaskName = "tnfsd";
        public static string TaskExecutable = TaskName + ".exe";
        public static void CreateOrUpdateTask(string taskName, string description, bool RunWithHighestPriv, string userId, string password, string executablePath, string arguments = "")
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
            td.Principal.LogonType = TaskLogonType.Password;
            if (RunWithHighestPriv)
            {
                td.Principal.RunLevel = TaskRunLevel.Highest;
            }
            else
            {
                td.Principal.RunLevel = TaskRunLevel.LUA;
            }

            // ✅ Register task
            ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.CreateOrUpdate, userId, password, TaskLogonType.Password);
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

        public static TaskProperties GetTaskProperties(string taskName)
        {
            TaskProperties taskProperties = new TaskProperties();

            using TaskService ts = new TaskService();
            {
                if (TaskExists(taskName))
                {
                    Microsoft.Win32.TaskScheduler.Task task = ts.GetTask(taskName);

                    if (task != null)
                    {

                        TaskDefinition taskDefinition = task.Definition;

                        taskProperties.RunWithHighestPrivilege = taskDefinition.Principal.RunLevel == TaskRunLevel.Highest;
                        taskProperties.UserId = taskDefinition.Principal.UserId;

                        foreach (Microsoft.Win32.TaskScheduler.Action action in taskDefinition.Actions)
                        {
                            if (action.ActionType == TaskActionType.Execute)
                            {
                                Microsoft.Win32.TaskScheduler.ExecAction execAction = (Microsoft.Win32.TaskScheduler.ExecAction)action;
                                taskProperties.ExecutableFolder =  execAction.Path.Replace($"\\{TaskExecutable}", "");
                                taskProperties.ShareFolder =  execAction.Arguments.Replace("\"", "");
                            }
                        }
                    }
                }

            }

            return taskProperties;
        }
        
    }
}

