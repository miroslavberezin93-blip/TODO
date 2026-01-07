using Microsoft.EntityFrameworkCore;
using TaskServer.Dto;
using TaskServer.DATA;
using TaskServer.Models;

namespace TaskServer.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId).Select(
                t => CreateDto(t))
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskDto> CreateTaskAsync(int userId, TaskCreateDto taskCreateDto)
        {
            var task = new TaskItem
            {
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                Completed = false,
                UserId = userId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            var taskDto = CreateDto(task);
            return taskDto;
        }

        public async Task<TaskDto?> UpdateTaskByIdAsync(int userId, int taskId, TaskUpdateDto taskUpdateDto)
        {
            if (taskUpdateDto.Title == null &&
                taskUpdateDto.Description == null &&
                taskUpdateDto.Completed == null) return null;
            var task = await _context.Tasks.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId);
            if (task == null) return null;
            task.Title = taskUpdateDto.Title ?? task.Title;
            task.Description = taskUpdateDto.Description ?? task.Description;
            task.Completed = taskUpdateDto.Completed ?? task.Completed;
            await _context.SaveChangesAsync();
            var taskDto = CreateDto(task);
            return taskDto;
        }

        public async Task<bool> DeleteTaskByIdAsync(int userId, int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId);
            if (task == null) return false;
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TaskDto CreateDto(TaskItem taskItem)
        {
            var dto = new TaskDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                Completed = taskItem.Completed,
            };
            return dto;
        }
    }
}