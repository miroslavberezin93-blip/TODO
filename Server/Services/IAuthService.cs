using Server.Dto;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(string username, string password);
        Task<TokenResponseDto> LoginAsync(string username, string password);
        Task<TokenResponseDto> UpdateUsernameAsync(string newUsername, string oldUsername, string password);
        Task<TokenResponseDto> UpdatePasswordAsync(string username, string oldPassword, string newPassword);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(int userId);
    }
}