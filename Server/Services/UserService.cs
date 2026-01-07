using Server.Models;
using Server.Dto;
using Server.DATA;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ITaskService _taskService;
        public UserService(AppDbContext context, ITaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {

        }
    }
}