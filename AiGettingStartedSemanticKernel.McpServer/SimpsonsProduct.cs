using System.Text.Json.Serialization;

namespace AiGettingStartedSemanticKernel.McpServer;

public record SimpsonsProduct
{
    [JsonPropertyName("id")]
    public required int Id { get; init; }
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("category")]
    public required string Category{ get; init; }
    [JsonPropertyName("Details")]
    public required string Details { get; init; }
}
