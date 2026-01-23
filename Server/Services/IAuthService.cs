using Server.Dto;
using Server.Models;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(string username, string password);
        Task<TokenResponseDto> LoginAsync(string username, string password);
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(int userId);
        Task<TokenResponseDto> GetTokenDtoAndUpdate(User user);
    }
}