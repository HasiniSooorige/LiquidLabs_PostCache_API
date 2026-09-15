using LiquidlabsPostcacheApi.Models;

namespace LiquidlabsPostcacheApi.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllAsync();

}