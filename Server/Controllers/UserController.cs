using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Extensions;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserTaskFacade _userTasks;
        public UserController(IUserService userService, IUserTaskFacade userTasks)
        {
            _userService = userService;
            _userTasks = userTasks;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int userId = User.GetUserId();
            var dto = await _userTasks.GetUserTasksAsync(userId);
            if(dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            int userId = User.GetUserId();
            if (!await _userService.DeleteUserAsync(userId)) return NotFound();
            return NoContent();
        }
    }
}