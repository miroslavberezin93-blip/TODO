using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskServer
{
    [ApiController]
    [Route("tasks")]
    public class TaskController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            Console.WriteLine("GET recieved");

            var tasks = await _context.Tasks.ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewTask([FromBody]TaskCreateDto created)
        {
            Console.WriteLine($"new task:{created.Title}");

            var task = new TaskItem {
                Title = created.Title ?? "default",
                Description = created.Description ?? "default",
                Completed = false
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskById(int id)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            Console.WriteLine($"deleted task:{task.Title}");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangeTaskStateById(int id, [FromBody]CompleteDto completed)
        {
            Console.WriteLine($"completion changed: id:{id}, completed:{completed.Completed}");

            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            task.Completed = completed.Completed;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateTaskContentById(int id, [FromBody]TaskUpdateDto update)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();
            if (update.Title != null) task.Title = update.Title;
            if (update.Description != null) task.Description = update.Description;

            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}