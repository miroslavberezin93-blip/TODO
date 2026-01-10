using System.Security.Claims;

namespace Server.Services
{
    public interface ISecurityService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string hash);
        string GenerateAccessToken(int userId);
        string GenerateRefreshToken();
        void AppendTokenForCookie(HttpResponse response, string? refreshToken, bool isRevoking);
    }
}