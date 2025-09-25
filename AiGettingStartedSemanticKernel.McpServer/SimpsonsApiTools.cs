using System.ComponentModel;
using System.Net.Http.Json;
using ModelContextProtocol.Server;

namespace AiGettingStartedSemanticKernel.McpServer;

[McpServerToolType]
public class SimpsonsApiTools
{
    private readonly HttpClient _httpClient;

    public SimpsonsApiTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [McpServerTool, Description("Get data about all characters from the Simpsons")]
    public async Task<List<SimpsonsCharacter>> GetAllSimpsonsCharacters()
    {
        return (await _httpClient.GetFromJsonAsync<List<SimpsonsCharacter>>("characters"))!;
    }

    [McpServerTool, Description("Get data about a specific character from the Simpsons by id")]
    public async Task<SimpsonsCharacter> GetSimpsonsCharacter(int characterId)
    {
        return (await _httpClient.GetFromJsonAsync<SimpsonsCharacter>($"characters/{characterId}"))!;
    }

    [McpServerTool, Description("Get data about all episodes from the Simpsons")]
    public async Task<List<SimpsonsEpisode>> GetAllSimpsonsEpisodes()
    {
        return (await _httpClient.GetFromJsonAsync<List<SimpsonsEpisode>>("episodes"))!;
    }

    [McpServerTool, Description("Get data about a specific episode from the Simpsons by id")]
    public async Task<SimpsonsEpisode> GetSimpsonsEpisode(int episodeId)
    {
        return (await _httpClient.GetFromJsonAsync<SimpsonsEpisode>($"episodes/{episodeId}"))!;
    }

    [McpServerTool, Description("Get data about all products from the Simpsons")]
    public async Task<List<SimpsonsProduct>> GetAllSimpsonsProducts()
    {
        return (await _httpClient.GetFromJsonAsync<List<SimpsonsProduct>>("products"))!;
    }

    [McpServerTool, Description("Get data about a specific product from the Simpsons by id")]
    public async Task<SimpsonsProduct> GetSimpsonsProduct(int productId)
    {
        return (await _httpClient.GetFromJsonAsync<SimpsonsProduct>($"products/{productId}"))!;
    }
}
