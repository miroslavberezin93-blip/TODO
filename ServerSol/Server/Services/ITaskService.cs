using Server.Dto;

namespace Server.Services
{
    public interface ITaskService
    {
        Task<IReadOnlyList<TaskItemDto>> GetTasksAsync(int userId);
        Task<TaskItemDto> CreateTaskAsync(int userId, string title, string description);
        Task<TaskItemDto?> UpdateTaskByIdAsync(int userId, int taskId, string? title, string? description, bool completed);
        Task<bool> DeleteTaskByIdAsync(int userId, int taskId);
        Task<bool> DeleteTasksByUserId(int userId);
    }
}