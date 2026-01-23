using Microsoft.Extensions.Options;
using Server.Dto;
using Server.Models;
using Server.Exceptions;

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
            _securityService.ValidateNullOrWhiteSpace(username, nameof(username));
            _securityService.ValidateNullOrWhiteSpace(password, nameof(password));
            var hashedPassword = _securityService.HashPassword(password);
            var user = await _userService.CreateUserAsync(username, hashedPassword) ??
                throw new ConflictException("User already exists");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> LoginAsync(string username, string password)
        {
            _securityService.ValidateNullOrWhiteSpace(username, nameof(username));
            _securityService.ValidateNullOrWhiteSpace(password, nameof(password));
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidCredentialsException("User not found");
            if (!_securityService.ValidatePassword(password, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid password");
            return await GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            if (refreshToken == null)
                throw new UnauthorizedAccessException("no token");
            var user = await _userService.GetUserByTokenAsync(refreshToken) ??
                throw new InvalidCredentialsException("Invalid token");
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

        public async Task<TokenResponseDto> GetTokenDtoAndUpdate(User user)
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
    }
}