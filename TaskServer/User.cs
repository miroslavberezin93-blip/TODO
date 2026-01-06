using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskServer
{
    public class User
    {
        [Key]
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("password")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}