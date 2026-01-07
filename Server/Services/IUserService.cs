using Server.Dto;
using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserByIdAsync(int userId, UpdateUserDto updateUserDto);
        Task DeleteUserByIdAsync(int userId);
        Task<UserTasksDto> GetUserAsync(int userId);
    }
}