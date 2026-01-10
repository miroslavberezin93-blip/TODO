using Server.Dto;
using Server.Models;

namespace Server.Services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(int userId);
        Task<User?> GetUserAsync(string username);
        Task<User?> GetUserByTokenAsync(string tolken);
        Task<User?> CreateUserAsync(string username, string passwordHash);
        Task<User?> UpdatePasswordAsync(int userId, string passwordHash);
        Task<User?> UpdateUsernameAsync(int userId, string username);
        Task<bool> UpdateUserTokenAsync(int userId, string? refreshToken, DateTime expiry);
        Task<bool> DeleteUserAsync(int userId);
    }
}