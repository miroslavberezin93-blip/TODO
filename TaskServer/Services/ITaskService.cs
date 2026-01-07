using TaskServer.Dto;

namespace TaskServer.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetTasksAsync(int userId);
        Task<TaskDto> CreateTaskAsync(int userId, TaskCreateDto taskCreateDto);
        Task<TaskDto?> UpdateTaskByIdAsync(int userId, int taskId, TaskUpdateDto taskUpdateDto);
        Task<bool> DeleteTaskByIdAsync(int userId, int taskId);
    }
}