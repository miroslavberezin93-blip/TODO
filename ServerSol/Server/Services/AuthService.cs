using Microsoft.Extensions.Options;
using Server.Dto;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            ValidateNullOrWhiteSpace(username, nameof(username));
            ValidateNullOrWhiteSpace(password, nameof(password));
            var hashedPassword = _securityService.HashPassword(password);
            var user = await _userService.CreateUserAsync(username, hashedPassword) ??
                throw new InvalidOperationException("User already exists");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> LoginAsync(string username, string password)
        {
            ValidateNullOrWhiteSpace(username, nameof(username));
            ValidateNullOrWhiteSpace(password, nameof(password));
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidOperationException("User not found");
            if (!_securityService.ValidatePassword(password, user.PasswordHash))
                throw new ArgumentException("Invalid password", nameof(password));
            return await GetTokenDtoAndUpdate(user);
        }
        public async Task<TokenResponseDto> UpdateUsernameAsync(string newUsername, string oldUsername, string password)
        {
            ValidateNullOrWhiteSpace(newUsername, nameof(newUsername));
            ValidateNullOrWhiteSpace(oldUsername, nameof(oldUsername));
            ValidateNullOrWhiteSpace(password, nameof(password));
            var user = await _userService.GetUserAsync(oldUsername) ??
                throw new InvalidOperationException("User not found");
            if (user.Username == newUsername)
                throw new ArgumentException("Username cannot be same as last", nameof(newUsername));
            if (!_securityService.ValidatePassword(password, user.PasswordHash))
                throw new ArgumentException("Invalid password", nameof(password));
            user = await _userService.UpdateUsernameAsync(user.UserId, newUsername) ??
                throw new ArgumentException("User already exists", nameof(newUsername));
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> UpdatePasswordAsync(string username, string oldPassword, string newPassword)
        {
            ValidateNullOrWhiteSpace(username, nameof(username));
            ValidateNullOrWhiteSpace(oldPassword, nameof(oldPassword));
            ValidateNullOrWhiteSpace(newPassword, nameof(newPassword));
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidOperationException("User not found");
            if (!_securityService.ValidatePassword(oldPassword, user.PasswordHash))
                throw new ArgumentException("Invalid password", nameof(oldPassword));
            if (_securityService.ValidatePassword(newPassword, user.PasswordHash))
                throw new ArgumentException("New password can not be same as last", nameof(newPassword));
            var hashed = _securityService.HashPassword(newPassword);
            await _userService.UpdatePasswordAsync(user.UserId, hashed);
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (refreshToken == null)
                throw new UnauthorizedAccessException("no token");
            var user = await _userService.GetUserByTokenAsync(refreshToken) ??
                throw new ArgumentException("Invalid token", nameof(refreshToken));
            if (user.TokenExpiry < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                throw new UnauthorizedAccessException("Refresh token expired");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task LogoutAsync(int userId)
        {
            await _userService.UpdateUserTokenAsync(
                userId,
                null,
                DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeSeconds()
            );
        }

        private async Task<TokenResponseDto> GetTokenDtoAndUpdate(User user)
        {
            var refreshToken = _securityService.GenerateRefreshToken();
            var accessToken = _securityService.GenerateAccessToken(user.UserId);
            var expiry = DateTimeOffset.UtcNow.AddDays(_options.RefreshTokenExpiryDays).ToUnixTimeSeconds();
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