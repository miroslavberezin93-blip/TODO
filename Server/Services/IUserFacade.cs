using Server.Dto;

namespace Server.Services
{
    public interface IUserFacade
    {
        Task<UserTasksDto?> GetUserTasksAsync(int userId);
        Task<TokenResponseDto> UpdateUsernameAsync(string newUsername, string oldUsername, string password);
        Task<TokenResponseDto> UpdatePasswordAsync(string username, string oldPassword, string newPassword);
    }
}