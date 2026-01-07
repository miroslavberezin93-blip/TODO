using System.Text.Json.Serialization;

namespace TaskServer.Dto
{
    public class TaskUpdateDto
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("completed")]
        public bool? Completed { get; set; }
    }
}