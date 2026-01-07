using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly SecurityOptions _options;
        public SecurityService(IOptions<SecurityOptions> options) 
        {
            _options = options.Value;
        }
        public string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_options.SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: _options.Pbkdf2Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: _options.HashSize
            );
            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                throw new FormatException("Invalid hash format");

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: salt,
                iterations: _options.Pbkdf2Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: _options.HashSize
            );
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        public string GenerateAccessToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.JwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            byte[] randomBytes = new byte[64];
            RandomNumberGenerator.Fill(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public bool ValidateRefreshToken(string token, string storedToken, DateTime expiry)
        {
            if (storedToken != token)
                return false;
            if (expiry < DateTime.UtcNow)
                return false;
            return true;
        }
    }
}