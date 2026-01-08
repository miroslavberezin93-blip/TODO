using Microsoft.Extensions.Options;
using Server.Dto;
using Server.Models;

namespace Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private readonly SecurityOptions _options;

        public AuthService(IUserService userService, ISecurityService securityService, IOptions<SecurityOptions> options)
        {
            _options = options.Value;
            _userService = userService;
            _securityService = securityService;
        }

        public async Task<TokenResponseDto> RegisterAsync(string username, string password)
        {
            ValidateNullOrWhiteSpace(username, "Username");
            ValidateNullOrWhiteSpace(password, "Password");
            var hashedPassword = _securityService.HashPassword(password);
            var user = await _userService.CreateUserAsync(username, hashedPassword) ??
                throw new InvalidOperationException("User already exists");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> LoginAsync(string username, string password)
        {
            ValidateNullOrWhiteSpace(username, "Username");
            ValidateNullOrWhiteSpace(password, "Password");
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidOperationException("User not found");
            if (!_securityService.ValidatePassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> UpdatePasswordAsync(string username, string currentPassword, string newPassword)
        {
            ValidateNullOrWhiteSpace(username, "Username");
            ValidateNullOrWhiteSpace(currentPassword, "Current Password");
            ValidateNullOrWhiteSpace(newPassword, "New Password");
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidOperationException("User not found");
            if (!_securityService.ValidatePassword(currentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");
            if (_securityService.ValidatePassword(newPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("New password can not be same as last");
            var hashed = _securityService.HashPassword(user.PasswordHash);
            await _userService.UpdatePasswordAsync(user.UserId, hashed);
            return await GetTokenDtoAndUpdate(user);
        }
        public async Task<TokenResponseDto> UpdateUsernameAsync(string username, string currentPassword, string newPassword)
        {
            ValidateNullOrWhiteSpace(username, "Username");
            ValidateNullOrWhiteSpace(currentPassword, "Current Password");
            ValidateNullOrWhiteSpace(newPassword, "New Password");
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidOperationException("User not found");
            if (!_securityService.ValidatePassword(currentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");
            if (_securityService.ValidatePassword(newPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("New password can not be same as last");
            var hashed = _securityService.HashPassword(user.PasswordHash);
            await _userService.UpdatePasswordAsync(user.UserId, hashed);
            return await GetTokenDtoAndUpdate(user);
        }

        private async Task<TokenResponseDto> GetTokenDtoAndUpdate(User user)
        {
            var refreshToken = _securityService.GenerateRefreshToken();
            var accessToken = _securityService.GenerateAccessToken(user.UserId);
            var expiry = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiryDays);
            await _userService.UpdateUserTokenAsync(user.UserId, refreshToken, expiry);
            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private static void ValidateNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} cannot be null or whitespace", paramName);
        }
    }
}