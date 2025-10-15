using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreatePost([FromBody] CreatePostDTO createPostDTO)
    {
        if (createPostDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            int postId = await _postService.CreatePostAsync(createPostDTO);
            return CreatedAtAction(nameof(CreatePost), new { id = postId });
        }
        catch (ChannelDoesNotExistException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
