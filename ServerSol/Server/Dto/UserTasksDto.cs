using System.Text.Json.Serialization;

namespace Server.Dto
{
    public class UserTasksDto
    {
        public string Username { get; set; } = string.Empty;
        public IReadOnlyList<TaskItemDto> Tasks { get; set; } = [];
    }
}