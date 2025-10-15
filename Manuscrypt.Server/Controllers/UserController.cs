using Microsoft.AspNetCore.Mvc;
using Manuscrypt.Server.Data.DTOs;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;

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


    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDTO createUserDTO)
    {
        if (createUserDTO == null)
        {
            return BadRequest("User data is required.");
        }

        try
        {
            int userId = await _userService.CreateUser(createUserDTO);
            return CreatedAtAction(nameof(CreateUser), new { id = userId });
        }
        catch (UserExistsException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<int>> Login([FromBody] LoginDTO loginDto)
    {
        if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest("Email and password are required.");
        }

        try
        {
            int userId = await _userService.Login(loginDto);
            return Ok(new { id = userId});
        }
        catch (UserDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
