using TaskServer.Dto;

namespace TaskServer.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
        Task<TokenResponseDto> UpdateAsync(UpdateUserDto updateUserDto);
    }
}