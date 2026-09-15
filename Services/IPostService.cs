using LiquidlabsPostcacheApi.Models;

namespace LiquidlabsPostcacheApi.Services;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post?> GetPostByIdAsync(int id);
}