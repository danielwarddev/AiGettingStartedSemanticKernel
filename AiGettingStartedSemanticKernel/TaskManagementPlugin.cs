using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace AiGettingStartedSemanticKernel;

public class TaskModel
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required string Priority { get; set; }
}

public class TaskManagementPlugin
{
    public static readonly List<TaskModel> Tasks = [
        new() { Id = 1, Title = "Design homepage", Description = "Create a modern homepage layout", Status = "In Progress", Priority = "High" },
        new() { Id = 2, Title = "Fix login bug", Description = "Resolve the issue with login sessions timing out", Status = "To Do", Priority = "Critical" },
        new() { Id = 3, Title = "Fix UI", Description = "Fix the UI not appearing correctly and hiding some controls", Status = "To Do", Priority = "Critical" },
        new() { Id = 4, Title = "Update documentation", Description = "Improve API reference for developers", Status = "Completed", Priority = "Medium" }
    ];
    
    [KernelFunction("get_all_tasks")]
    [Description("Gets a list of all tasks")]
    [return: Description("A list of tasks")]
    public List<TaskModel> GetAllTasks()
    {
        return Tasks.ToList();
    }
    
    [KernelFunction("get_critical_tasks")]
    [Description("Gets a list of all tasks marked as 'Critical' priority")]
    [return: Description("A list of critical tasks")]
    public List<TaskModel> GetCriticalTasks()
    {
        return Tasks.Where(task => task.Priority.Equals("Critical", StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    [KernelFunction("complete_task")]
    [Description("Updates the status of the specified task to Completed")]
    [return: Description("The updated task; will return null if the task does not exist")]
    public TaskModel? CompleteTask(int id)
    {
        var task = Tasks.FirstOrDefault(task => task.Id == id);

        if (task == null)
        {
            return null;
        }

        task.Status = "Completed";
        return task;
    }
}