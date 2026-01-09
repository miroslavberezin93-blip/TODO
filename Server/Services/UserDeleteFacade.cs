namespace Server.Services
{
    public class UserDeleteFacade : IUserDeleteFacade
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        public UserDeleteFacade(IUserService userService, ITaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<bool> DeleteUserAndTasksAsync(int userId)
        {
            await _taskService.DeleteTasksByUserId(userId);
            if(!await _userService.DeleteUserAsync(userId)) return false;
            return true;
        }
    }
}