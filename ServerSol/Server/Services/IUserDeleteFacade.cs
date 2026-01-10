namespace Server.Services;

public interface IUserDeleteFacade
{
    Task<bool> DeleteUserAndTasksAsync(int userId);
}