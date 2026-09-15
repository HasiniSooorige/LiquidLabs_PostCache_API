using LiquidlabsPostcacheApi.Models;

namespace LiquidlabsPostcacheApi.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task AddAsync(Post post);
}