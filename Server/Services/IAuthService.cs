using Server.Dto;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(string username, string password);
        Task<TokenResponseDto> LoginAsync(string username, string password);
        Task<TokenResponseDto> UpdateUsernameAsync(string username, string password);
        Task<TokenResponseDto> UpdatePasswordAsync(string username, string currentPassword, string newPassword);
        Task<TokenResponseDto?> RefreshTokenAsync(string refreshToken);
        Task<TokenResponseDto?> LogoutAsync(int userId);
    }
}