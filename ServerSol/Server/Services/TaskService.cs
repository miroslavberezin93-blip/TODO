using Microsoft.EntityFrameworkCore;
using Server.Dto;
using Server.DATA;
using Server.Models;

namespace Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TaskItemDto>> GetTasksAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId).Select(
                t => CreateDto(t))
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskItemDto> CreateTaskAsync(int userId, string title, string description)
        {
            var task = new TaskItem
            {
                Title = title,
                Description = description,
                Completed = false,
                UserId = userId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            var taskDto = CreateDto(task);
            return taskDto;
        }

        public async Task<TaskItemDto?> UpdateTaskByIdAsync(int userId, int taskId, string? title, string? description, bool completed)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(
                t => t.Id == taskId && t.UserId == userId);
            if (task == null) return null;
            if (title == null &&
                description == null &&
                task.Completed == completed) return null;
            task.Title = title ?? task.Title;
            task.Description = description ?? task.Description;
            task.Completed = completed;
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

        public async Task<bool> DeleteTasksByUserId(int userId)
        {
            var tasks = await _context.Tasks.Where(t =>
                t.UserId == userId)
                .ToListAsync();
            if (tasks.Count == 0) return false;
            _context.RemoveRange(tasks);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TaskItemDto CreateDto(TaskItem taskItem)
        {
            var dto = new TaskItemDto
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