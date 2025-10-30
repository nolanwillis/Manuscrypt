using Manuscrypt.Shared.DTOs.User;

namespace Manuscrypt.Shared;

public class UserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetUserDTO> GetAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/user/{userId}");
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var user = await System.Text.Json.JsonSerializer.DeserializeAsync<GetUserDTO>(contentStream);

        return user!;
    }
}
