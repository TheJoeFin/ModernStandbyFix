using Microsoft.Win32.TaskScheduler;
using System;

namespace ModernStandbyFix;

internal class TaskSchedulerUtils
{
    private const string TaskName = "ModernStandbyFix";

    public static bool IsRunningOnStartup(string applicationPath)
    {
        try
        {
            return TaskService.Instance.GetTask(TaskName) is not null;
        }
        catch (Exception ex)
        {
            App.LogIntoFile(ex.Message);
            return false;
        }
    }

    public static bool ToggleRunOnStartup(string applicationPath)
    {
        try
        {
            Task? startUpTask = TaskService.Instance.GetTask(TaskName);

            if (startUpTask is null)
            {
                TaskService taskService = new();
                TaskDefinition taskDefinition = taskService.NewTask();
                taskDefinition.RegistrationInfo.Description = "Modern Standby Fix";
                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
                taskDefinition.Triggers.Add(new LogonTrigger());
                taskDefinition.Actions.Add(new ExecAction(applicationPath, "/minimized"));
                taskDefinition.Settings.MultipleInstances = TaskInstancesPolicy.IgnoreNew;
                taskDefinition.Settings.AllowHardTerminate = true;
                taskDefinition.Settings.DisallowStartIfOnBatteries = false;

                taskService.RootFolder.RegisterTaskDefinition(TaskName, taskDefinition);
            }
            else
                TaskService.Instance.RootFolder.DeleteTask(TaskName);
        }
        catch (Exception ex)
        {
            App.LogIntoFile(ex.Message);
            return false;
        }
        return true;
    }
}
