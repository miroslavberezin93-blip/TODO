using System.Text.Json.Serialization;

namespace Server.Dto
{
    public class UserTasksDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("tasks")]
        public List<TaskItemDto> Tasks { get; set; } = [];
    }
}