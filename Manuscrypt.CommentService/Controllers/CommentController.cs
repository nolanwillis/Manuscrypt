using Manuscrypt.Shared.DTOs.Comment;
using Manuscrypt.CommentService.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.CommentService.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentDomainService _domainService;

    public CommentController(CommentDomainService commentDomainService)
    {
        _domainService = commentDomainService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetCommentDTO>> GetCommentAsync(int id)
    {
        try
        {
            var postDTO = await _domainService.GetCommentAsync(id);
            return Ok(postDTO);
        }
        catch (CommentDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("User/{id:int}/Comments")]
    public async Task<ActionResult<IEnumerable<GetCommentDTO>>> GetCommentsByUserAsync(int id)
    {
        try
        {
            var getPostDTOs = await _domainService.GetCommentsByUserAsync(id);
            return Ok(getPostDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("Post/{id:int}/Comments")]
    public async Task<ActionResult<IEnumerable<GetCommentDTO>>> GetCommentsForPostAsync(int id)
    {
        try
        {
            var getPostDTOs = await _domainService.GetCommentsForPostAsync(id);
            return Ok(getPostDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<GetCommentDTO>> CreateCommentAsync([FromBody] CreateCommentDTO createCommentDTO)
    {
        if (createCommentDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            var created = await _domainService.CreateCommentAsync(createCommentDTO);
            return Created($"comment/{created.Id}", created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentDTO updateCommentDTO)
    {
        if (updateCommentDTO == null)
        {
            return BadRequest("Comment data is required.");
        }

        try
        {
            await _domainService.UpdateCommentAsync(updateCommentDTO);
            return NoContent();
        }
        catch (CommentDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCommentAsync(int id)
    {
        try
        {
            await _domainService.DeleteCommentAsync(id);
            return NoContent();
        }
        catch (CommentDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
