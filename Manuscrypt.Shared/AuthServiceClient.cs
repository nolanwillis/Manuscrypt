using Manuscrypt.Shared.DTOs.User;

namespace Manuscrypt.Shared;

public class AuthServiceClient
{
    private readonly HttpClient _httpClient;

    public AuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetUserDTO> GetAsync(int userId)
    {
        // For testing purposes only.
        var response = await _httpClient.GetAsync($"/auth/dev/{userId}");
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var user = await System.Text.Json.JsonSerializer.DeserializeAsync<GetUserDTO>(contentStream);

        return user!;


        /* var response = await _httpClient.GetAsync($"/auth/{userId}");
        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var user = await System.Text.Json.JsonSerializer.DeserializeAsync<GetUserDTO>(contentStream);

        return user!; */
    }
}
