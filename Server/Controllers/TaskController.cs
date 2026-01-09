using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Extensions;
using Server.Dto;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = User.GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskCreateDto taskCreateDto)
        {
            int userId = User.GetUserId();
            var task = await _taskService.CreateTaskAsync(
                userId,
                taskCreateDto.Title,
                taskCreateDto.Description
            );
            return Ok(task);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TaskUpdateDto taskUpdateDto)
        {
            int userId = User.GetUserId();
            var task = await _taskService.UpdateTaskByIdAsync(
                userId,
                id,
                taskUpdateDto.Title,
                taskUpdateDto.Description,
                taskUpdateDto.Completed
            );
            if (task == null) return BadRequest();
            return Ok(task);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = User.GetUserId();
            if(!await _taskService.DeleteTaskByIdAsync(userId, id)) return NotFound();
            return NoContent();
        }
    }
}