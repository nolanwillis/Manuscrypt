using Manuscrypt.Server.Data.DTOs.Subscription;
using Manuscrypt.Server.Services;
using Manuscrypt.Server.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Manuscrypt.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
 
    [HttpGet("{subscriptionId}")]
    public async Task<ActionResult<GetSubscriptionDTO>> GetSubscriptionAsync(int subscriptionId)
    {
        try
        {
            var subscriptionDTO = await _subscriptionService.GetSubscriptionAsync(subscriptionId);
            return Ok(subscriptionDTO);
        }
        catch (SubscriptionDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<int>> CreateSubscriptionAsync([FromBody] CreateSubscriptionDTO createSubscriptionDTO)
    {
        if (createSubscriptionDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            int id = await _subscriptionService.CreateSubscriptionAsync(createSubscriptionDTO);
            return CreatedAtAction(nameof(CreateSubscriptionAsync), new { id }, new { id });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{subscriptionId}")]
    public async Task<IActionResult> DeleteSubscriptionAsync(int subscriptionId)
    {
        try
        {
            await _subscriptionService.DeleteSubscriptionAsync(subscriptionId);
            return NoContent();
        }
        catch (SubscriptionDoesNotExistException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
