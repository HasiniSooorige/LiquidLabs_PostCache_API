using LiquidlabsPostcacheApi.Models;
using Microsoft.Data.SqlClient;

namespace LiquidlabsPostcacheApi.Repositories;

public class PostRepository(IConfiguration config) : IPostRepository
{
    private readonly string _connectionString = config.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        var list = new List<Post>();
        const string sql = "SELECT Id, UserId, Title, Body, CreatedAt FROM PostCache ORDER BY Id";

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(MapPost(reader));
        }

        return list;
    }

    private static Post MapPost(SqlDataReader rdr) => new()
    {
        Id = rdr.GetInt32(0),
        UserId = rdr.GetInt32(1),
        Title = rdr.GetString(2),
        Body = rdr.GetString(3),
        CreatedAt = rdr.GetDateTime(4)
    };

    public async Task<Post?> GetByIdAsync(int id)
    {
        const string sql = "SELECT Id, UserId, Title, Body, CreatedAt FROM Posts WHERE Id = @Id";

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        await using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapPost(reader) : null;
    }

    public async Task AddAsync(Post post)
    {
        const string sql = @"
            INSERT INTO Posts (Id, UserId, Title, Body)
            VALUES (@Id, @UserId, @Title, @Body)";

        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@Id", post.Id);
        cmd.Parameters.AddWithValue("@UserId", post.UserId);
        cmd.Parameters.AddWithValue("@Title", post.Title);
        cmd.Parameters.AddWithValue("@Body", post.Body);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}