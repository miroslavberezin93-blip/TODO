using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Extensions;
using Server.Dto;
using Server.Services;
using Microsoft.Extensions.Options;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SecurityOptions _options;
        private readonly ISecurityService _securityService;
        private readonly IAuthService _authService;
        public AuthController(ISecurityService serviceService, IAuthService authService, IOptions<SecurityOptions> options)
        {
            _securityService = serviceService;
            _authService = authService;
            _options = options.Value;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            TokenResponseDto tokens = await _authService.RegisterAsync(
                registerDto.Username,
                registerDto.Password
            );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            TokenResponseDto tokens = await _authService.LoginAsync(
                loginDto.Username,
                loginDto.Password
            );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }

        [HttpPatch("update/username")]
        [Authorize]
        public async Task<IActionResult> UpdateUsername([FromBody] UsernameUpdateDto usernameUpdateDto)
        {
            TokenResponseDto tokens = await _authService.UpdateUsernameAsync(
                usernameUpdateDto.NewUsername,
                usernameUpdateDto.OldUsername,
                usernameUpdateDto.Password
                );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }
        
        [HttpPatch("update/password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            TokenResponseDto tokens = await _authService.UpdatePasswordAsync(
                passwordUpdateDto.Username,
                passwordUpdateDto.OldPassword,
                passwordUpdateDto.NewPassword
                );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!Request.Cookies.TryGetValue(_options.RefreshTokenCookieName, out var refreshToken))
                return Unauthorized();
            TokenResponseDto tokens = await _authService.RefreshTokenAsync(refreshToken);
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            int userId = User.GetUserId();
            await _authService.LogoutAsync(userId);
            _securityService.AppendTokenForCookie(Response, null, true);
            return NoContent();
        }
    }
}