using Manuscrypt.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : Controller
{
    private readonly UserDomainService _domainService;

    public EventController(UserDomainService domainService)
    {
        _domainService = domainService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEventsAsync(
        [FromQuery] int startId, [FromQuery] int endId = int.MaxValue)
    {
        try
        {
            var events = await _domainService.GetEventsAsync(startId, endId);
            return Ok(events);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
