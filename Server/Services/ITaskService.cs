using Server.Dto;

namespace Server.Services
{
    public interface ITaskService
    {
        Task<IReadOnlyList<TaskItemDto>> GetTasksAsync(int userId);
        Task<TaskItemDto> CreateTaskAsync(int userId, TaskCreateDto taskCreateDto);
        Task<TaskItemDto?> UpdateTaskByIdAsync(int userId, int taskId, TaskUpdateDto taskUpdateDto);
        Task<bool> DeleteTaskByIdAsync(int userId, int taskId);
    }
}