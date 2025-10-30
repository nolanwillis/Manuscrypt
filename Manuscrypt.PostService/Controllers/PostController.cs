using Manuscrypt.Shared.DTOs.Post;
using Manuscrypt.PostService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.PostService.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly PostDomainService _domainService;

    public PostController(PostDomainService domainService)
    {
        _domainService = domainService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPostDTO>> GetPostAsync(int id)
    {
        try
        {
            var postDTO = await _domainService.GetPostAsync(id);
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

    [HttpGet("User/{id:int}/Posts")]
    public async Task<ActionResult<IEnumerable<GetPostDTO>>> GetPostsByUserAsync(int id)
    {
        try
        {
            var getPostDTOs = await _domainService.GetPostsByUserAsync(id);
            return Ok(getPostDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetPostDTO>> CreatePostAsync([FromBody] CreatePostDTO createPostDTO)
    {
        if (createPostDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            var created = await _domainService.CreatePostAsync(createPostDTO);
            return Created($"/post/{created.Id}", created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePostAsync([FromBody] UpdatePostDTO updatePostDTO)
    {
        if (updatePostDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            await _domainService.UpdatePostAsync(updatePostDTO);
            return NoContent();
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePostAsync(int id)
    {
        try
        {
            await _domainService.DeletePostAsync(id);
            return NoContent();
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
}
