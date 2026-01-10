using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Extensions
{
    public static class CLaimPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new UnauthorizedAccessException("User ID not found in token");
            if (!int.TryParse(idClaim, out int userId))
                throw new UnauthorizedAccessException("Invalid User ID in token");
            return userId;
        }
    }
}