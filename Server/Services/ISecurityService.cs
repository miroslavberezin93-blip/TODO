namespace Server.Services
{
    public interface ISecurityService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string hash);
        string GenerateAccessToken(int userId);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(string token, string storedId, DateTime expiry);
    }
}