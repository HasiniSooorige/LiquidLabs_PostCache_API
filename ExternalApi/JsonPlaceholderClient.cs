using System.Net;
using System.Net.Http.Json;
using LiquidlabsPostcacheApi.Models;

namespace LiquidlabsPostcacheApi.ExternalApi;

public class JsonPlaceholderClient(HttpClient httpClient) : IJsonPlaceholderClient
{
    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        var res = await httpClient.GetFromJsonAsync<List<Post>>("posts");
        return res ?? [];
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        var res = await httpClient.GetAsync($"posts/{id}");

        if (res.StatusCode == HttpStatusCode.NotFound)
            return null;

        res.EnsureSuccessStatusCode();

        return await res.Content.ReadFromJsonAsync<Post>();
    }
}