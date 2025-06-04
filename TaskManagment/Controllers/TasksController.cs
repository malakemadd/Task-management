using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TaskManagment.IRepoServices;
using TaskManagment.Models;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TaskController(UserManager<AppUser> userManager, ITaskServices taskService, IHttpClientFactory httpClientFactory, HttpClient _httpClient) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTask(Tasks task)
    {
        task.UserId = userManager.GetUserId(User);
        taskService.InsertTask(task);
        await NotifyTaskChange(task, "created");
        return Ok(task);
    }


    [HttpGet]
    public IActionResult GetTasks(int pageNumber = 1, int pageSize = 10)
    {
        var userId = userManager.GetUserId(User);
        var tasks = taskService.SelectAllTasks().Where(t => t.UserId == userId)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();
        return Ok(new { PageNumber = pageNumber, PageSize = pageSize, Tasks = tasks });
    }

    [HttpGet("{id}")]
    public IActionResult GetTaskById(int id)
    {
        var task = taskService.SelectTaskByID(id);
        if (task == null || task.UserId != userManager.GetUserId(User)) return NotFound();
        return Ok(task);
    }

    [HttpPut("{id}")]
   
    public async Task<IActionResult> UpdateTask(int id, Tasks taskUpdate)
    {
        var task = taskService.SelectTaskByID(id);
        if (task == null || task.UserId != userManager.GetUserId(User)) return NotFound();

        task.TaskName = taskUpdate.TaskName;
        task.TaskDate = taskUpdate.TaskDate;
        task.Description = taskUpdate.Description;
        task.Completed = taskUpdate.Completed;

        taskService.UpdateTask(task);
        await NotifyTaskChange(task, "updated");
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(int id)
    {
        var task = taskService.SelectTaskByID(id);
        if (task == null || task.UserId != userManager.GetUserId(User)) return NotFound();

        taskService.DeleteTask(id);
        return Ok("Task deleted successfully");
    }

   
    private async Task NotifyTaskChange(Tasks task, string action)
    {
        var notification = new
        {
            TaskId = task.ID,
            Action = action,
            Description = task.Description,
            UserId = task.UserId
        };
        await _httpClient.PostAsJsonAsync("http://localhost:5266/api/notifications", notification);
    }
}