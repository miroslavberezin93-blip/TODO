using System.Text.Json.Serialization;

namespace Server.Dto
{
    public class TaskItemDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}