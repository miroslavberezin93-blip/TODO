public class SecurityOptions
{
    public int Pbkdf2Iterations { get; set; } = 100_000;
    public int SaltSize { get; set; } = 16;
    public int HashSize { get; set; } = 32;
    public string JwtSecret { get; set; } = string.Empty;
    public int AccessTokenExpiryMinutes { get; set; } = 15;
    public int RefreshTokenExpiryDays { get; set; } = 7;
    public string RefreshTokenCookieName {  get; set; } = string.Empty;
}