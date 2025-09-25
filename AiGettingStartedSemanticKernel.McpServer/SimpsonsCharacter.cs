using System.Text.Json.Serialization;

namespace AiGettingStartedSemanticKernel.McpServer;

public record SimpsonsCharacter
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("normalized_name")]
    public required string NormalizedName { get; init; }
    [JsonPropertyName("gender")]
    public required string Gender { get; init; }
}
