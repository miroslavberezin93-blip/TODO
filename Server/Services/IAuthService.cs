using Server.Dto;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
        Task<TokenResponseDto> UpdateAsync(UpdateUserDto updateUserDto);
        Task<TokenResponseDto> RefreshTokenAsync(int userId, string refreshToken);
    }
}