using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dto;
using Server.Extensions;
using Server.Services;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserDeleteFacade _userDelete;
        private readonly IUserFacade _userFacade;
        private readonly ISecurityService _securityService;
        public UserController(IUserDeleteFacade userDelete, IUserFacade userFacade, ISecurityService securityService)
        {
            _userDelete = userDelete;
            _userFacade = userFacade;
            _securityService = securityService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = User.GetUserId();
            var dto = await _userFacade.GetUserTasksAsync(userId);
            if(dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            int userId = User.GetUserId();
            if (!await _userDelete.DeleteUserAndTasksAsync(userId)) return NotFound();
            return NoContent();
        }

        [HttpPatch("update/username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UsernameUpdateDto usernameUpdateDto)
        {
            TokenResponseDto tokens = await _userFacade.UpdateUsernameAsync(
                usernameUpdateDto.NewUsername,
                usernameUpdateDto.OldUsername,
                usernameUpdateDto.Password
                );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }

        [HttpPatch("update/password")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto passwordUpdateDto)
        {
            TokenResponseDto tokens = await _userFacade.UpdatePasswordAsync(
                passwordUpdateDto.Username,
                passwordUpdateDto.OldPassword,
                passwordUpdateDto.NewPassword
                );
            _securityService.AppendTokenForCookie(Response, tokens.RefreshToken, false);
            return Ok(new { accessToken = tokens.AccessToken });
        }
    }
}