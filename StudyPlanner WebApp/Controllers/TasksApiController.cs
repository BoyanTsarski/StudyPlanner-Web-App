using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksApiController(ITaskService taskService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _taskService.GetAllTasksAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] StudyTask task)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            task.UserId = _userManager.GetUserId(User);
            await _taskService.CreateTaskAsync(task);
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] StudyTask task)
        {
            if (id != task.Id) return BadRequest();
            await _taskService.UpdateTaskAsync(task);
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return Ok();
        }
    }
}