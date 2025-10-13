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

    [HttpGet]
    public async Task<ActionResult<UserDTO>> Login([FromQuery] string email)
    {
        try
        {
            UserDTO user = await _userService.Login(email);
            return Ok(user);
        }
        catch (UserDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
