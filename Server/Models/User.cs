using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        [Required]
        public DateTime TokenExpiry { get; set; }
    }
}