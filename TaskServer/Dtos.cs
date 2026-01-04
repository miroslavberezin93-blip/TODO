using System.Text.Json.Serialization;

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
}
