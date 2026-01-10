using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Server.Services;
using Server.Extensions;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserDeleteFacade _userDelete;
        private readonly IUserTaskFacade _userTasks;
        public UserController(IUserDeleteFacade userDelete, IUserTaskFacade userTasks)
        {
            _userDelete = userDelete;
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
            if (!await _userDelete.DeleteUserAndTasksAsync(userId)) return NotFound();
            return NoContent();
        }
    }
}