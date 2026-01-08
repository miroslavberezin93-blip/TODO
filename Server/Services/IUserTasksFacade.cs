using Server.Dto;

namespace Server.Services
{
    public interface IUserTaskFacade
    {
        Task<UserTasksDto?> GetUserTasksAsync(int userId);
    }
}