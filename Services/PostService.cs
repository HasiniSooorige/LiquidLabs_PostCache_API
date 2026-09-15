using LiquidlabsPostcacheApi.ExternalApi;
using LiquidlabsPostcacheApi.Models;
using LiquidlabsPostcacheApi.Repositories;

namespace LiquidlabsPostcacheApi.Services;

public class PostService(IPostRepository repo, IJsonPlaceholderClient api) : IPostService
{
    public async Task<IEnumerable<Post>> GetPostsAsync()
    {
        var cached = (await repo.GetAllAsync()).ToList();

        if (cached.Count == 0)
        {
            var apiPosts = (await api.GetPostsAsync()).ToList();

            foreach (var post in apiPosts)
                await repo.AddAsync(post);

            return apiPosts;
        }

        var cachedIds = cached.Select(p => p.Id).ToHashSet();
        var externalData = await api.GetPostsAsync();
        var missing = externalData.Where(p => !cachedIds.Contains(p.Id)).ToList();

        foreach (var post in missing)
            await repo.AddAsync(post);

        return cached.Concat(missing).OrderBy(p => p.Id);
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        var cached = await repo.GetByIdAsync(id);
        if (cached != null)
            return cached;

        var remote = await api.GetPostByIdAsync(id);
        if (remote == null)
            return null;

        await repo.AddAsync(remote);
        return remote;
    }
}