using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace TaskServer
{
    public class CompleteDto
    {
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }

    public class TaskUpdateDto 
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    public class TaskCreateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }

    public class UserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class UserUpdateDto
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
    }
}
