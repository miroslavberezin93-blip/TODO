using Server.Models;
using Server.Dto;
using Server.DATA;
using Microsoft.EntityFrameworkCore;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.UserId == userId);
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.Username == username);
        }
        //TODO: Not secured metod, need to create model for token and add id in it(refactor)
        public async Task<User?> GetUserByTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(
                u => u.RefreshToken == token);
        }

        public async Task<User?> CreateUserAsync(string username, string passwordHash)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username)) return null;
            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash
            };
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to create user {Username}: possible unique constraint violation",
                    username
                );
                return null;
            }
            return user;
        }

        public async Task<User?> UpdatePasswordAsync(int userId, string passwordHash)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;
            user.PasswordHash = passwordHash;
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User?> UpdateUsernameAsync(int userId, string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username)) return null;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return null;
            user.Username = username;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserTokenAsync(int userId, string? refreshToken, long expiry)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;
            user.RefreshToken = refreshToken;
            user.TokenExpiry = expiry;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}