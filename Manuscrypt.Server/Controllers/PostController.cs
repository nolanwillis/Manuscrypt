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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetAllPostsAsync()
    {
        try
        {
            var postDTOs = await _postService.GetPostsAsync();
            return Ok(postDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }  
    [HttpGet("{postId}")]
    public async Task<ActionResult<PostDTO>> GetPostAsync(int postId)
    {
        try
        {
            var postDTO = await _postService.GetPostAsync(postId);
            return Ok(postDTO);
        }
        catch (PostDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{postId}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForPostAsync(int postId)
    {
        try
        {
            var commentDTOs = await _postService.GetCommentsForPostAsync(postId);
            return Ok(commentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreatePostAsync([FromBody] CreatePostDTO createPostDTO)
    {
        if (createPostDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            int postId = await _postService.CreatePostAsync(createPostDTO);
            return CreatedAtAction(nameof(CreatePostAsync), new { id = postId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
