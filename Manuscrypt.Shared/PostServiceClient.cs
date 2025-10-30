using Manuscrypt.Shared.DTOs.Post;

namespace Manuscrypt.Shared;

public class PostServiceClient
{
    private readonly HttpClient _httpClient;

    public PostServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetPostDTO> GetAsync(int postId)
    {
        var response = await _httpClient.GetAsync($"/post/{postId}");
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var user = await System.Text.Json.JsonSerializer.DeserializeAsync<GetPostDTO>(contentStream);

        return user!;
    }
}
