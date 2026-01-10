namespace Server.Dto
{
    public class UsernameUpdateDto
    {
        public string OldUsername { get; set; } = string.Empty;
        public string NewUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}