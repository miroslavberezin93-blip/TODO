namespace TaskServer.Dto
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}