using LiquidlabsPostcacheApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiquidlabsPostcacheApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var data = await _postService.GetPostsAsync();
        return Ok(data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest("Invalid ID");

        var result = await _postService.GetPostByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}