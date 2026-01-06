using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TaskServer
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return Conflict(new { message = "Username already exists" });
            var user = new User{
                Username = dto.Username,
                PasswordHash = dto.PasswordHash
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(int id,[FromBody] UserUpdateDto updated)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();
            if (updated.Username != null)
            {
                var usernameTaken = await _context.Users
                    .AnyAsync(u => u.Username == updated.Username && u.UserId != id);
                if (usernameTaken)
                    return Conflict(new { message = "Username already exists" });
            }
            user.PasswordHash = updated.PasswordHash ?? user.PasswordHash;
            user.Username = updated.Username ?? user.Username;
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if (user == null) return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}