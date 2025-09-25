using System.Text.Json.Serialization;

namespace AiGettingStartedSemanticKernel.McpServer;

public record SimpsonsEpisode
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("season")]
    public required int Season { get; init; }
    [JsonPropertyName("episode")]
    public required int Episode { get; init; }
    [JsonPropertyName("description")]
    public required string Description { get; init; }
    [JsonPropertyName("airDate")]
    public required string AirDate { get; init; }
    [JsonPropertyName("thumbnailUrl")]
    public required string ThumbnailUrl { get; init; }
}
