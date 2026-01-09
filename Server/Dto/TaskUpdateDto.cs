using System.Text.Json.Serialization;

namespace Server.Dto
{
    public class TaskUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
    }
}