using Microsoft.AspNetCore.Mvc;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Services;

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
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] PostDTO postDto)
    {
        if (postDto == null)
        {
            return BadRequest("Post data is required.");
        }

        PostDTO createdPost = await _postService.CreatePostAsync(postDto);

        if (createdPost == null)
        {
            return StatusCode(500, "A problem happened with creating the post.");
        }

        return CreatedAtAction(nameof(CreatePost), new { id = createdPost.Id }, createdPost);
    }
}
