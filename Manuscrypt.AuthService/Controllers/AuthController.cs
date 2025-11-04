using Microsoft.AspNetCore.Mvc;
using Manuscrypt.AuthService.Exceptions;
using Manuscrypt.Shared.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace Manuscrypt.AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthDomainService _domainService;

    public AuthController(AuthDomainService authDomainService)
    {
        _domainService = authDomainService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GetUserDTO>> GetAsync(string id)
    {
        try
        {
            var userDTO = await _domainService.GetAsync(id);
            return Ok(userDTO);
        }
        catch (UserDoesNotExistWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> RegisterAsync([FromBody] CreateUserDTO createUserDTO)
    {
        if (createUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        try
        {
            var token = await _domainService.CreateUserAsync(createUserDTO);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null || string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
        {
            return BadRequest("Email and password are required.");
        }

        try
        {
            var token = await _domainService.LoginAsync(loginDTO);
            return Ok(token);
        }
        catch (UserDoesNotExistWithEmailException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO updateUserDTO)
    {
        if (updateUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        var userIdFromToken = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (userIdFromToken == null)
        {
            return BadRequest("Could not find claim in token.");
        }
        updateUserDTO.Id = userIdFromToken;

        try
        {
            await _domainService.UpdateUserAsync(updateUserDTO);
            return NoContent();
        }
        catch (UserDoesNotExistWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUserAsync()
    {
        try
        {
            var userIdFromToken = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userIdFromToken == null)
            {
                return BadRequest("Could not find claim in token.");
            }

            await _domainService.DeleteUserAsync(userIdFromToken);
            return NoContent();
        }
        catch (UserDoesNotExistWithIdException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
