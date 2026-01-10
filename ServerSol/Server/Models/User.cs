using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}