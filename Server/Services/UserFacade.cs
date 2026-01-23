using Server.Dto;
using Server.Exceptions;
using Server.Models;

namespace Server.Services
{
    public class UserFacade : IUserFacade
    {
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly ISecurityService _securityService;
        public UserFacade(IUserService userService, ITaskService taskService, IAuthService authService, ISecurityService securityService)
        {
            _userService = userService;
            _taskService = taskService;
            _authService = authService;
            _securityService = securityService;
        }

        public async Task<UserTasksDto?> GetUserTasksAsync(int userId)
        {
            var user = await _userService.GetUserAsync(userId);
            if (user == null) return null;
            var tasks = await _taskService.GetTasksAsync(userId);
            return CreateUserTaskDto(user, tasks);
        }

        public async Task<TokenResponseDto> UpdateUsernameAsync(string newUsername, string oldUsername, string password)
        {
            _securityService.ValidateNullOrWhiteSpace(newUsername, nameof(newUsername));
            _securityService.ValidateNullOrWhiteSpace(oldUsername, nameof(oldUsername));
            _securityService.ValidateNullOrWhiteSpace(password, nameof(password));
            var user = await _userService.GetUserAsync(oldUsername) ??
                throw new InvalidCredentialsException("User not found");
            if (user.Username == newUsername)
                throw new InvalidInputException("Username cannot be same as last");
            if (!_securityService.ValidatePassword(password, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid password");
            user = await _userService.UpdateUsernameAsync(user.UserId, newUsername) ??
                throw new ConflictException("User already exists");
            return await _authService.GetTokenDtoAndUpdate(user);
        }

        public async Task<TokenResponseDto> UpdatePasswordAsync(string username, string oldPassword, string newPassword)
        {
            _securityService.ValidateNullOrWhiteSpace(username, nameof(username));
            _securityService.ValidateNullOrWhiteSpace(oldPassword, nameof(oldPassword));
            _securityService.ValidateNullOrWhiteSpace(newPassword, nameof(newPassword));
            var user = await _userService.GetUserAsync(username) ??
                throw new InvalidCredentialsException("User not found");
            if (!_securityService.ValidatePassword(oldPassword, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid password");
            if (_securityService.ValidatePassword(newPassword, user.PasswordHash))
                throw new InvalidInputException("New password can not be same as last");
            var hashed = _securityService.HashPassword(newPassword);
            await _userService.UpdatePasswordAsync(user.UserId, hashed);
            return await _authService.GetTokenDtoAndUpdate(user);
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