using Microsoft.AspNetCore.Mvc;
using Manuscrypt.UserService.Exceptions;
using Manuscrypt.Shared.DTOs.User;

namespace Manuscrypt.UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDomainService _domainService;

    public UserController(UserDomainService userService)
    {
        _domainService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserDTO>> GetUserAsync(int id)
    {
        try
        {
            var userDTO = await _domainService.GetUserAsync(id);
            return Ok(userDTO);
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

    [HttpPost]
    public async Task<ActionResult<GetUserDTO>> CreateUserAsync([FromBody] CreateUserDTO createUserDTO)
    {
        if (createUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        try
        {
            var created = await _domainService.CreateUserAsync(createUserDTO);
            return Created($"/user/{created.Id}", created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<int>> LoginAsync([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return BadRequest("Email and password are required.");
        }

        try
        {
            int userId = await _domainService.LoginAsync(loginDTO);
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
            await _domainService.UpdateUserAsync(updateUserDTO);
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
            await _domainService.DeleteUserAsync(id);
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
