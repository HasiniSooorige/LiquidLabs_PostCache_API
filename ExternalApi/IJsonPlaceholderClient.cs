using LiquidlabsPostcacheApi.Models;

namespace LiquidlabsPostcacheApi.ExternalApi;

public interface IJsonPlaceholderClient
{
	Task<IEnumerable<Post>> GetPostsAsync();
	Task<Post?> GetPostByIdAsync(int id);
}