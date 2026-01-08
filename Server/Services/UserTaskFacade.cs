using Server.Dto;
using Server.Models;

namespace Server.Services
{
    public class UserTaskFacade : IUserTaskFacade
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        public UserTaskFacade(IUserService userService, ITaskService taskService)
        {
            _userService = userService;
            _taskService = taskService;
        }

        public async Task<UserTasksDto?> GetUserTasksAsync(int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return null;
            var tasks = await _taskService.GetTasksAsync(userId);
            return CreateUserTaskDto(user, tasks);
        }

        private static UserTasksDto CreateUserTaskDto(User user, IReadOnlyList<TaskItemDto> task)
        {
            return new UserTasksDto
            {
                Username = user.Username,
                Tasks = task
            };
        }
    }
}