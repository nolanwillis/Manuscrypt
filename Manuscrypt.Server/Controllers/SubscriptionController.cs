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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetSubscriptionDTO>>> GetAllSubscriptionsAsync()
    {
        try
        {
            var subscriptionDTOs = await _subscriptionService.GetSubscriptionsAsync();
            return Ok(subscriptionDTOs);
        }
        catch(Exception ex) 
        {
            return BadRequest(ex.Message);
        }
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
    public async Task<ActionResult<int>> CreateSubscriptionAsync([FromBody] GetSubscriptionDTO subscriptionDTO)
    {
        if (subscriptionDTO == null)
        {
            return BadRequest("Post data is required.");
        }

        try
        {
            int subscriptionId = await _subscriptionService.CreateSubscriptionAsync(subscriptionDTO);
            return CreatedAtAction(nameof(CreateSubscriptionAsync), new { id = subscriptionId });
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
