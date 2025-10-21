using Microsoft.AspNetCore.Mvc;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Manuscrypt.Server.Data.DTOs.User;
using Manuscrypt.Server.Data.DTOs.Post;
using Manuscrypt.Server.Data.DTOs.Comment;
using Manuscrypt.Server.Data.DTOs.Subscription;

namespace Manuscrypt.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserDTO>> GetUserAsync(int id)
    {
        try
        {
            var userDto = await _userService.GetUserAsync(id);
            return Ok(userDto);
        }
        catch (UserDNEWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{userId}/comments")]
    public async Task<ActionResult<IEnumerable<GetCommentDTO>>> GetCommentsByUserAsync(int userId)
    {
        try
        {
            var commentDTOs = await _userService.GetCommentsByUserAsync(userId);
            return Ok(commentDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{userId}/posts")]
    public async Task<ActionResult<IEnumerable<GetPostDTO>>> GetPostsForUserAsync(int userId)
    {
        try
        {
            var postDTOs = await _userService.GetPostsForUserAsync(userId);
            return Ok(postDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{userId}/subscribers")]
    public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> GetSubscribersForUserAsync(int userId)
    {
        try
        {
            var subscriptionDTOs = await _userService.GetSubscribersForUserAsync(userId);
            return Ok(subscriptionDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{userId}/subscriptions")]
    public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> GetSubscrptionsForUserAsync(int userId)
    {
        try
        {
            var subscriptionDTOs = await _userService.GetSubscriptionsForUserAsync(userId);
            return Ok(subscriptionDTOs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateUserAsync([FromBody] CreateUserDTO createUserDTO)
    {
        if (createUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        try
        {
            int userId = await _userService.CreateUserAsync(createUserDTO);
            return CreatedAtAction(nameof(CreateUserAsync), new { id = userId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("login")]
    public async Task<ActionResult<int>> LoginAsync([FromBody] LoginDTO loginDto)
    {
        if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest("Email and password are required.");
        }

        try
        {
            int userId = await _userService.LoginAsync(loginDto);
            return Ok(new { id = userId });
        }
        catch (UserDNEWithEmailException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO updateUserDTO)
    {
        if (updateUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        try
        {
            await _userService.UpdateUserAsync(updateUserDTO);
            return NoContent();
        }
        catch (UserDNEWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        catch (UserDNEWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
