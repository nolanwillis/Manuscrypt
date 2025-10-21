using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCommentDTO>>> GetAllCommentsAsync()
    {
        try
        {
            var commentDTOs = await _commentService.GetCommentsAsync();
            return Ok(commentDTOs);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{commentId}")]
    public async Task<ActionResult<GetCommentDTO>> GetCommentAsync(int commentId)
    {
        try
        {
            var postDTO = await _commentService.GetCommentAsync(commentId);
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
    
    [HttpPost]
    public async Task<ActionResult<int>> CreateCommentAsync([FromBody] CreateCommentDTO commentDTO)
    {
        if (commentDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            int commentId = await _commentService.CreateCommentAsync(commentDTO);
            return CreatedAtAction(nameof(CreateCommentAsync), new { id = commentId });
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
            await _commentService.UpdateCommentAsync(updateCommentDTO);
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

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteCommentAsync(int commentId)
    {
        try
        {
            await _commentService.DeleteCommentAsync(commentId);
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
